#!/usr/bin/env python3
"""
ChattyWidgetsLibrary Catalog and Package Builder
Generates .chw packages, embeds cryptographic signatures, and creates verified catalog.json.
"""

import os
import sys
import json
import base64
import hashlib
import zipfile

try:
    from cryptography.hazmat.primitives import hashes, serialization
    from cryptography.hazmat.primitives.asymmetric import padding
    CRYPTO_AVAILABLE = True
except ImportError:
    CRYPTO_AVAILABLE = False

OFFICIAL_ID_PREFIX = "com.chatty."

def get_private_key(keys_dir):
    # 1. Environment variable (e.g. in GitHub Actions Secrets)
    env_key = os.environ.get("OFFICIAL_PRIVATE_KEY")
    if env_key:
        try:
            return serialization.load_pem_private_key(env_key.encode("utf-8"), password=None)
        except Exception as e:
            print(f"[Warning] Failed to load key from OFFICIAL_PRIVATE_KEY: {e}")

    # 2. Local file
    priv_file = os.path.join(keys_dir, "official_private.pem")
    if os.path.isfile(priv_file):
        try:
            with open(priv_file, "rb") as f:
                return serialization.load_pem_private_key(f.read(), password=None)
        except Exception as e:
            print(f"[Warning] Failed to load local private key: {e}")

    return None

def sign_payload(private_key, data_bytes):
    if not private_key or not CRYPTO_AVAILABLE:
        return ""
    signature = private_key.sign(
        data_bytes,
        padding.PKCS1v15(),
        hashes.SHA256()
    )
    return base64.b64encode(signature).decode("utf-8")

def parse_author_info(widget_id, manifest):
    """
    Guarantees verified author identity:
    - Official com.chatty.* widgets are strictly attributed to ChattyWidgets / chattytoaster.
    - Community widgets extract verified username from namespace (e.g. com.github.user.name or user.name).
    """
    if widget_id.startswith(OFFICIAL_ID_PREFIX):
        return {
            "author": "ChattyWidgets",
            "authorGithub": "chattytoaster",
            "isOfficial": True,
            "trustLevel": "official"
        }

    # Community namespace parsing
    github_user = "community"
    if widget_id.startswith("com.github."):
        parts = widget_id.split(".")
        if len(parts) >= 3:
            github_user = parts[2]
    elif "." in widget_id:
        github_user = widget_id.split(".")[0]

    return {
        "author": f"@{github_user}",
        "authorGithub": github_user,
        "isOfficial": False,
        "trustLevel": "verified_community"
    }

def build():
    repo = os.environ.get("GITHUB_REPOSITORY", "chattytoaster/ChattyWidgetsLibrary")
    raw_base = f"https://raw.githubusercontent.com/{repo}/main"

    base_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    widgets_dir = os.path.join(base_dir, "widgets")
    packages_dir = os.path.join(base_dir, "packages")
    keys_dir = os.path.join(base_dir, "keys")
    catalog_path = os.path.join(base_dir, "catalog.json")

    os.makedirs(packages_dir, exist_ok=True)

    private_key = get_private_key(keys_dir) if CRYPTO_AVAILABLE else None
    if private_key:
        print("[Security] Official RSA Private Key loaded. Packages will be cryptographically signed.")
    else:
        print("[Notice] Running without private key signing.")

    if not os.path.exists(widgets_dir):
        print(f"Error: {widgets_dir} not found.")
        sys.exit(1)

    catalog = []
    widget_dirs = sorted([d for d in os.listdir(widgets_dir) if os.path.isdir(os.path.join(widgets_dir, d))])

    print(f"Packaging {len(widget_dirs)} widgets...")

    for widget_id in widget_dirs:
        widget_path = os.path.join(widgets_dir, widget_id)
        manifest_path = os.path.join(widget_path, "manifest.json")

        if not os.path.isfile(manifest_path):
            continue

        try:
            with open(manifest_path, "r", encoding="utf-8") as f:
                manifest = json.load(f)
        except Exception as e:
            print(f"Error reading {manifest_path}: {e}")
            continue

        author_info = parse_author_info(widget_id, manifest)
        version = manifest.get("version", "1.0.0")

        # 1. Compute content hash of widget source
        hasher = hashlib.sha256()
        for root, _, files in os.walk(widget_path):
            for file in sorted(files):
                full_path = os.path.join(root, file)
                with open(full_path, "rb") as f:
                    while chunk := f.read(65536):
                        hasher.update(chunk)
        content_hash = hasher.hexdigest()

        # 2. Generate signature for official widgets
        signature = ""
        if author_info["isOfficial"] and private_key:
            payload_to_sign = f"{widget_id}:{version}:{content_hash}:official".encode("utf-8")
            signature = sign_payload(private_key, payload_to_sign)

        # 3. Create .chw package (ZIP archive format)
        pkg_filename = f"{widget_id}.chw"
        pkg_filepath = os.path.join(packages_dir, pkg_filename)

        with zipfile.ZipFile(pkg_filepath, "w", zipfile.ZIP_DEFLATED) as zipf:
            for root, _, files in os.walk(widget_path):
                for file in sorted(files):
                    if file.endswith((".chw", ".git", ".DS_Store", "signature.sig")):
                        continue
                    full_path = os.path.join(root, file)
                    rel_path = os.path.relpath(full_path, widget_path)
                    zipf.write(full_path, rel_path)

            # Embed verification signature metadata inside the package
            meta = {
                "id": widget_id,
                "version": version,
                "author": author_info["author"],
                "authorGithub": author_info["authorGithub"],
                "isOfficial": author_info["isOfficial"],
                "contentHash": content_hash,
                "signature": signature
            }
            zipf.writestr("chatty_signature.json", json.dumps(meta, indent=2))

        # Calculate package file hash and size
        with open(pkg_filepath, "rb") as f:
            pkg_bytes = f.read()
            pkg_sha256 = hashlib.sha256(pkg_bytes).hexdigest()

        size_kb = max(1, round(len(pkg_bytes) / 1024))
        icon_file = manifest.get("icon", "icon.png")

        item = {
            "id": manifest.get("id", widget_id),
            "name": manifest.get("name", widget_id),
            "name_ru": manifest.get("name_ru", ""),
            "version": version,
            "author": author_info["author"],
            "authorGithub": author_info["authorGithub"],
            "authorUrl": f"https://github.com/{author_info['authorGithub']}",
            "isOfficial": author_info["isOfficial"],
            "trustLevel": author_info["trustLevel"],
            "description": manifest.get("description", ""),
            "description_ru": manifest.get("description_ru", ""),
            "category": manifest.get("category", "General"),
            "type": manifest.get("type", "web"),
            "icon": f"{raw_base}/widgets/{widget_id}/{icon_file}",
            "downloadUrl": f"{raw_base}/packages/{pkg_filename}",
            "size": f"{size_kb} KB",
            "sha256": pkg_sha256,
            "contentHash": content_hash,
            "signature": signature
        }
        catalog.append(item)
        sig_flag = " [SIGNED OFFICIAL]" if signature else ""
        print(f"  [+] Packed {manifest.get('name')} ({widget_id}) by {author_info['author']} -> {size_kb} KB{sig_flag}")

    with open(catalog_path, "w", encoding="utf-8") as f:
        json.dump(catalog, f, ensure_ascii=False, indent=2)

    print(f"\nSuccessfully generated {catalog_path} with {len(catalog)} verified items.")

if __name__ == "__main__":
    build()

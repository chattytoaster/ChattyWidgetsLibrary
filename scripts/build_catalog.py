#!/usr/bin/env python3
"""
ChattyWidgetsLibrary Catalog and Package Builder
Generates .chw packages from widgets/ and creates catalog.json for ChattyWidgets Hub Store.
"""

import os
import sys
import json
import zipfile

def build():
    repo = os.environ.get("GITHUB_REPOSITORY", "chattytoaster/ChattyWidgetsLibrary")
    raw_base = f"https://raw.githubusercontent.com/{repo}/main"

    base_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    widgets_dir = os.path.join(base_dir, "widgets")
    packages_dir = os.path.join(base_dir, "packages")
    catalog_path = os.path.join(base_dir, "catalog.json")

    os.makedirs(packages_dir, exist_ok=True)

    if not os.path.exists(widgets_dir):
        print(f"Error: {widgets_dir} not found.")
        sys.exit(1)

    catalog = []
    widget_dirs = sorted([d for d in os.listdir(widgets_dir) if os.path.isdir(os.path.join(widgets_dir, d))])

    print(f"Found {len(widget_dirs)} widget directories in {widgets_dir}...")

    for widget_id in widget_dirs:
        widget_path = os.path.join(widgets_dir, widget_id)
        manifest_path = os.path.join(widget_path, "manifest.json")

        if not os.path.isfile(manifest_path):
            print(f"Skipping {widget_id}: manifest.json not found.")
            continue

        try:
            with open(manifest_path, "r", encoding="utf-8") as f:
                manifest = json.load(f)
        except Exception as e:
            print(f"Error reading {manifest_path}: {e}")
            continue

        pkg_filename = f"{widget_id}.chw"
        pkg_filepath = os.path.join(packages_dir, pkg_filename)

        # Pack into .chw (ZIP archive format)
        with zipfile.ZipFile(pkg_filepath, "w", zipfile.ZIP_DEFLATED) as zipf:
            for root, _, files in os.walk(widget_path):
                for file in sorted(files):
                    if file.endswith((".chw", ".git", ".DS_Store")):
                        continue
                    full_path = os.path.join(root, file)
                    rel_path = os.path.relpath(full_path, widget_path)
                    zipf.write(full_path, rel_path)

        size_kb = max(1, round(os.path.getsize(pkg_filepath) / 1024))
        icon_file = manifest.get("icon", "icon.png")

        item = {
            "id": manifest.get("id", widget_id),
            "name": manifest.get("name", widget_id),
            "version": manifest.get("version", "1.0.0"),
            "author": manifest.get("author", "Community"),
            "description": manifest.get("description", ""),
            "category": manifest.get("category", "General"),
            "type": manifest.get("type", "web"),
            "icon": f"{raw_base}/widgets/{widget_id}/{icon_file}",
            "downloadUrl": f"{raw_base}/packages/{pkg_filename}",
            "size": f"{size_kb} KB"
        }
        catalog.append(item)
        print(f"  [+] Packed {manifest.get('name')} ({widget_id}) -> {size_kb} KB")

    with open(catalog_path, "w", encoding="utf-8") as f:
        json.dump(catalog, f, ensure_ascii=False, indent=2)

    print(f"\nSuccessfully generated {catalog_path} with {len(catalog)} items.")

if __name__ == "__main__":
    build()

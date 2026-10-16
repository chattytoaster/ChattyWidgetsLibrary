#!/usr/bin/env python3
"""
ChattyWidgetsLibrary Widget Validator
Enforces:
1. Strict namespace policies (com.chatty.* for official; com.github.<author>.* for community)
2. Identity spoofing prevention (community cannot pretend to be ChattyWidgets or other users)
3. Manifest schema and security constraints
"""

import os
import sys
import json
import re

DANGEROUS_EXTENSIONS = {".exe", ".bat", ".cmd", ".vbs", ".ps1", ".msi", ".scr", ".pif", ".dll"}
MAX_PACKAGE_SIZE_BYTES = 25 * 1024 * 1024  # 25 MB
OFFICIAL_ID_PREFIX = "com.chatty."
RESERVED_AUTHORS = {"chattywidgets", "chattytoaster", "official", "администрация", "официальный"}

def get_pr_author():
    # 1. Environment variable override
    if os.environ.get("PR_AUTHOR"):
        return os.environ.get("PR_AUTHOR").strip().lower()

    # 2. GitHub Actions event payload
    event_path = os.environ.get("GITHUB_EVENT_PATH")
    if event_path and os.path.isfile(event_path):
        try:
            with open(event_path, "r", encoding="utf-8") as f:
                event_data = json.load(f)
                pr_user = event_data.get("pull_request", {}).get("user", {}).get("login")
                if pr_user:
                    return pr_user.strip().lower()
        except Exception:
            pass
    return None

def validate():
    base_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    widgets_dir = os.path.join(base_dir, "widgets")

    if not os.path.exists(widgets_dir):
        print(f"Error: {widgets_dir} not found.")
        sys.exit(1)

    pr_author = get_pr_author()
    if pr_author:
        print(f"Validating PR submitted by authenticated GitHub user: @{pr_author}")

    widget_dirs = sorted([d for d in os.listdir(widgets_dir) if os.path.isdir(os.path.join(widgets_dir, d))])
    errors = []

    print(f"Validating {len(widget_dirs)} widgets...")

    for widget_id in widget_dirs:
        widget_path = os.path.join(widgets_dir, widget_id)
        manifest_path = os.path.join(widget_path, "manifest.json")

        # 1. Check ID pattern
        if not re.match(r"^[a-zA-Z0-9_\-\.]+$", widget_id):
            errors.append(f"[{widget_id}] Invalid folder name: must only contain letters, digits, dash, dot, underscore.")

        # 2. Check manifest.json existence
        if not os.path.isfile(manifest_path):
            errors.append(f"[{widget_id}] Missing manifest.json.")
            continue

        # 3. Check JSON parsing
        try:
            with open(manifest_path, "r", encoding="utf-8") as f:
                manifest = json.load(f)
        except Exception as e:
            errors.append(f"[{widget_id}] Failed to parse manifest.json: {e}")
            continue

        # 4. Check manifest ID matches folder name
        if manifest.get("id") != widget_id:
            errors.append(f"[{widget_id}] Manifest id '{manifest.get('id')}' does not match directory name '{widget_id}'.")

        # 5. Required fields
        for field in ["id", "name", "version", "author", "type", "entry"]:
            if not manifest.get(field):
                errors.append(f"[{widget_id}] Missing required field in manifest: '{field}'.")

        # 6. Anti-Spoofing & Namespace Enforcement
        is_official = widget_id.startswith(OFFICIAL_ID_PREFIX)

        if pr_author:
            # If submitted in a PR by a community user:
            if pr_author != "chattytoaster":
                # Non-owner PR cannot touch or create com.chatty.* widgets!
                if is_official:
                    errors.append(
                        f"[{widget_id}] Security error: The '{OFFICIAL_ID_PREFIX}' prefix is strictly reserved for official ChattyWidgets widgets. Community widgets must use 'com.github.{pr_author}.<name>' or '{pr_author}.<name>'."
                    )

                # Check that community widget matches PR author login
                expected_prefix_1 = f"com.github.{pr_author}."
                expected_prefix_2 = f"{pr_author}."
                if not (widget_id.lower().startswith(expected_prefix_1) or widget_id.lower().startswith(expected_prefix_2)):
                    errors.append(
                        f"[{widget_id}] Namespace error: Widget ID must start with your authenticated GitHub username: '{expected_prefix_1}' or '{expected_prefix_2}' (Got: '{widget_id}')."
                    )

                # Cannot claim reserved official names in author field
                raw_author = str(manifest.get("author", "")).strip().lower()
                for reserved in RESERVED_AUTHORS:
                    if reserved in raw_author:
                        errors.append(
                            f"[{widget_id}] Anti-spoofing error: Community widgets cannot claim reserved author name '{raw_author}'. Your verified GitHub username will be used automatically."
                        )

                if manifest.get("isOfficial") is True:
                    errors.append(
                        f"[{widget_id}] Anti-spoofing error: 'isOfficial: true' is reserved exclusively for officially signed ChattyWidgets packages."
                    )

        # 7. Check entry file exists
        entry_file = manifest.get("entry", "")
        entry_path = os.path.join(widget_path, entry_file)
        if not os.path.isfile(entry_path):
            errors.append(f"[{widget_id}] Specified entry file '{entry_file}' not found.")

        # 8. Check icon exists
        icon_file = manifest.get("icon", "icon.png")
        icon_path = os.path.join(widget_path, icon_file)
        if not os.path.isfile(icon_path):
            print(f"  [Notice] [{widget_id}] Warning: icon '{icon_file}' not found.")

        # 9. Security & Size check
        total_size = 0
        for root, _, files in os.walk(widget_path):
            for file in files:
                ext = os.path.splitext(file)[1].lower()
                if ext in DANGEROUS_EXTENSIONS:
                    errors.append(f"[{widget_id}] Security error: Dangerous executable file found: '{file}'.")
                full_path = os.path.join(root, file)
                total_size += os.path.getsize(full_path)

        if total_size > MAX_PACKAGE_SIZE_BYTES:
            errors.append(f"[{widget_id}] Widget total size ({total_size / (1024*1024):.1f} MB) exceeds maximum allowed 25 MB.")

        print(f"  [OK] {widget_id} passed validation.")

    if errors:
        print("\n=== VALIDATION FAILED ===")
        for err in errors:
            print(f" - {err}")
        sys.exit(1)
    else:
        print("\n=== ALL WIDGETS VALIDATED SUCCESSFULLY ===")

if __name__ == "__main__":
    validate()

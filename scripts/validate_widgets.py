#!/usr/bin/env python3
"""
ChattyWidgetsLibrary Widget Validator
Validates widget structure, manifest.json, size limits, and security before PR merge.
"""

import os
import sys
import json
import re

DANGEROUS_EXTENSIONS = {".exe", ".bat", ".cmd", ".vbs", ".ps1", ".msi", ".scr", ".pif"}
MAX_PACKAGE_SIZE_BYTES = 25 * 1024 * 1024  # 25 MB

def validate():
    base_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    widgets_dir = os.path.join(base_dir, "widgets")

    if not os.path.exists(widgets_dir):
        print(f"Error: {widgets_dir} not found.")
        sys.exit(1)

    widget_dirs = [d for d in os.listdir(widgets_dir) if os.path.isdir(os.path.join(widgets_dir, d))]
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

        # 6. Check entry file exists
        entry_file = manifest.get("entry", "")
        entry_path = os.path.join(widget_path, entry_file)
        if not os.path.isfile(entry_path):
            errors.append(f"[{widget_id}] Specified entry file '{entry_file}' not found.")

        # 7. Check icon exists
        icon_file = manifest.get("icon", "icon.png")
        icon_path = os.path.join(widget_path, icon_file)
        if not os.path.isfile(icon_path):
            print(f"  [Notice] [{widget_id}] Warning: icon '{icon_file}' not found.")

        # 8. Security & Size check
        total_size = 0
        for root, _, files in os.walk(widget_path):
            for file in files:
                ext = os.path.splitext(file)[1].lower()
                if ext in DANGEROUS_EXTENSIONS:
                    errors.append(f"[{widget_id}] Potentially dangerous executable file found: '{file}'.")
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

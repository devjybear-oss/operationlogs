#!/usr/bin/env python3
"""Extract SQL blocks from Chillpay-Operation-Logs.md to docs/sql/*.sql"""
import re
from pathlib import Path

DOCS = Path(__file__).parent
MD = DOCS / "Chillpay-Operation-Logs.md"
OUT_DIR = DOCS / "sql"

SECTIONS = [
    ("ChillpayOperationLogs-Table.sql", "ChillpayOperationLogs-Table.sql"),
    ("ChillpayOperationLogs-Index.sql", "ChillpayOperationLogs-Index.sql"),
    ("ChillpayOperationLogs-View.sql", "ChillpayOperationLogs-View.sql"),
]


def extract_section(text: str, filename: str) -> str:
    pattern = rf"#### [^\n]*{re.escape(filename)}[\s\S]*?```sql\n([\s\S]*?)```"
    match = re.search(pattern, text)
    if not match:
        raise SystemExit(f"SQL block for {filename} not found in markdown")
    return match.group(1).strip() + "\n"


def main() -> None:
    text = MD.read_text(encoding="utf-8")
    OUT_DIR.mkdir(parents=True, exist_ok=True)

    parts: list[str] = []
    for marker, filename in SECTIONS:
        sql = extract_section(text, marker)
        path = OUT_DIR / filename
        path.write_text(sql, encoding="utf-8")
        parts.append(sql)
        print(f"Written: {path}")

    deploy = OUT_DIR / "ChillpayOperationLogs-Deploy.sql"
    deploy.write_text(
        "/* Chillpay Operation Logs — Full deploy (Table + Index + View) */\n"
        + "\n".join(parts),
        encoding="utf-8",
    )
    print(f"Written: {deploy}")


if __name__ == "__main__":
    main()

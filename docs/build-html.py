#!/usr/bin/env python3
"""Convert Chillpay-Operation-Logs.md to standalone HTML (content unchanged)."""
import re
import html as html_lib
from pathlib import Path

import markdown
from markdown.extensions.tables import TableExtension
from markdown.extensions.fenced_code import FencedCodeExtension

DOCS = Path(__file__).parent
MD_FILE = DOCS / "Chillpay-Operation-Logs.md"
OUT_FILE = DOCS / "Chillpay-Operation-Logs.html"

MERMAID_PLACEHOLDER = "<!--MERMAID_BLOCK_{i}-->"


def github_slug(title: str) -> str:
    t = title.strip().lower()
    t = t.replace("\u2014", "--").replace("\u2013", "-")
    t = re.sub(r"[,.:;!?()\[\]\"'`]", "", t)
    t = re.sub(r"\s+", "-", t)
    t = re.sub(r"-{3,}", "--", t)
    return t.strip("-")


def protect_mermaid(text: str) -> tuple[str, list[str]]:
    blocks: list[str] = []

    def repl(m: re.Match) -> str:
        blocks.append(m.group(1).strip())
        return MERMAID_PLACEHOLDER.format(i=len(blocks) - 1)

    out = re.sub(r"```mermaid\n([\s\S]*?)```", repl, text)
    return out, blocks


def restore_mermaid(html_body: str, blocks: list[str]) -> str:
    for i, block in enumerate(blocks):
        escaped = html_lib.escape(block)
        diagram = f'<pre class="mermaid">{escaped}</pre>'
        html_body = html_body.replace(MERMAID_PLACEHOLDER.format(i=i), diagram)
    return html_body


def add_header_ids(html_body: str) -> str:
    def repl(m: re.Match) -> str:
        level, content = m.group(1), m.group(2)
        plain = re.sub(r"<[^>]+>", "", content)
        hid = github_slug(plain)
        return f'<h{level} id="{hid}">{content}</h{level}>'

    return re.sub(r"<h([2-4])>(.*?)</h\1>", repl, html_body, flags=re.DOTALL)


def main() -> None:
    md_text = MD_FILE.read_text(encoding="utf-8")
    md_text, mermaid_blocks = protect_mermaid(md_text)

    md = markdown.Markdown(
        extensions=[
            TableExtension(),
            FencedCodeExtension(),
        ]
    )
    body = md.convert(md_text)
    body = restore_mermaid(body, mermaid_blocks)
    body = add_header_ids(body)

    # Fix image paths (keep ./images/ as images/)
    body = body.replace('src="./images/', 'src="images/')

    template = f"""<!DOCTYPE html>
<html lang="th">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Chillpay Operation Logs</title>
  <style>
    :root {{
      --bg: #ffffff;
      --text: #1a1a2e;
      --muted: #5c5c7a;
      --border: #e2e8f0;
      --code-bg: #f6f8fa;
      --accent: #2563eb;
      --blockquote-bg: #f0f7ff;
    }}
    * {{ box-sizing: border-box; }}
    body {{
      font-family: "Segoe UI", system-ui, -apple-system, sans-serif;
      line-height: 1.65;
      color: var(--text);
      background: var(--bg);
      max-width: 960px;
      margin: 0 auto;
      padding: 2rem 1.5rem 4rem;
    }}
    h1 {{ font-size: 2rem; border-bottom: 2px solid var(--border); padding-bottom: .5rem; }}
    h2 {{ font-size: 1.5rem; margin-top: 2.5rem; border-bottom: 1px solid var(--border); padding-bottom: .35rem; }}
    h3 {{ font-size: 1.2rem; margin-top: 1.75rem; }}
    h4 {{ font-size: 1.05rem; margin-top: 1.25rem; }}
    blockquote {{
      margin: 1rem 0;
      padding: .75rem 1rem;
      background: var(--blockquote-bg);
      border-left: 4px solid var(--accent);
      color: var(--muted);
    }}
    blockquote p {{ margin: 0; }}
    table {{
      width: 100%;
      border-collapse: collapse;
      margin: 1rem 0;
      font-size: .92rem;
    }}
    th, td {{
      border: 1px solid var(--border);
      padding: .5rem .75rem;
      text-align: left;
      vertical-align: top;
    }}
    th {{ background: #f8fafc; font-weight: 600; }}
    tr:nth-child(even) {{ background: #fafbfc; }}
    code {{
      font-family: Consolas, "Courier New", monospace;
      font-size: .88em;
      background: var(--code-bg);
      padding: .15em .35em;
      border-radius: 3px;
    }}
    pre {{
      background: var(--code-bg);
      border: 1px solid var(--border);
      border-radius: 6px;
      padding: 1rem;
      overflow-x: auto;
      font-size: .82rem;
      line-height: 1.5;
    }}
    pre code {{ background: none; padding: 0; }}
    pre.mermaid {{
      background: #fff;
      border: 1px dashed var(--border);
      text-align: center;
    }}
    img {{
      max-width: 100%;
      height: auto;
      border: 1px solid var(--border);
      border-radius: 6px;
      margin: 1rem 0;
    }}
    hr {{ border: none; border-top: 1px solid var(--border); margin: 2rem 0; }}
    ul, ol {{ padding-left: 1.5rem; }}
    li {{ margin: .25rem 0; }}
    a {{ color: var(--accent); text-decoration: none; }}
    a:hover {{ text-decoration: underline; }}
    em {{ color: var(--muted); }}
    .toc ul {{ list-style: none; padding-left: 0; }}
    .toc > ul > li {{ margin: .35rem 0; }}
    .toc ul ul {{ padding-left: 1.25rem; font-size: .95rem; }}
  </style>
  <script type="module">
    import mermaid from "https://cdn.jsdelivr.net/npm/mermaid@11/dist/mermaid.esm.min.mjs";
    mermaid.initialize({{ startOnLoad: true, theme: "default" }});
  </script>
</head>
<body>
{body}
</body>
</html>
"""

    OUT_FILE.write_text(template, encoding="utf-8")
    print(f"Written: {OUT_FILE}")


if __name__ == "__main__":
    main()

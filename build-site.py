#!/usr/bin/env python3
"""Build multi-page HTML site from Chillpay-Operation-Logs.md."""
import re
import html as html_lib
from pathlib import Path

import markdown
from markdown.extensions.fenced_code import FencedCodeExtension
from markdown.extensions.tables import TableExtension

DOCS = Path(__file__).parent
SITE = DOCS / "site"
ASSETS = SITE / "assets"
MD_FILE = DOCS / "Chillpay-Operation-Logs.md"

MERMAID_PH = "<!--MERMAID_{i}-->"

PAGES = [
    {
        "id": "index",
        "file": "index.html",
        "nav": "ภาพรวม",
        "subtitle": "Summary & Checklist",
        "icon": "◈",
        "sections": [0, 1, 2],
        "mermaid": False,
    },
    {
        "id": "registry",
        "file": "registry.html",
        "nav": "Registry",
        "subtitle": "ModuleType · MenuType · LogType",
        "icon": "▣",
        "sections": [3],
        "mermaid": False,
    },
    {
        "id": "ui",
        "file": "ui.html",
        "nav": "UI",
        "subtitle": "List · Detail · ปุ่ม Logs",
        "icon": "▦",
        "sections": [4],
        "mermaid": True,
    },
    {
        "id": "search",
        "file": "search.html",
        "nav": "Search",
        "subtitle": "การค้นหา & Filter",
        "icon": "⌕",
        "sections": [5],
        "mermaid": True,
    },
    {
        "id": "api",
        "file": "api.html",
        "nav": "API",
        "subtitle": "Endpoints & Parameters",
        "icon": "⟡",
        "sections": [6],
        "mermaid": False,
    },
    {
        "id": "database",
        "file": "database.html",
        "nav": "Database",
        "subtitle": "Schema · Index · SQL Script",
        "icon": "⬡",
        "sections": [7],
        "mermaid": False,
    },
    {
        "id": "files",
        "file": "files.html",
        "nav": "ไฟล์ที่เกี่ยวข้อง",
        "subtitle": "Repos & Source files",
        "icon": "◎",
        "sections": [8],
        "mermaid": False,
    },
    {
        "id": "deploy",
        "file": "deploy.html",
        "nav": "Deploy",
        "subtitle": "ลำดับ deploy & Smoke test",
        "icon": "▶",
        "sections": [9],
        "mermaid": False,
    },
    {
        "id": "sync",
        "file": "sync.html",
        "nav": "Registry Sync",
        "subtitle": "Manual sync checklist",
        "icon": "↻",
        "sections": [10],
        "mermaid": False,
    },
]


def protect_mermaid(text: str) -> tuple[str, list[str]]:
    blocks: list[str] = []

    def repl(m: re.Match) -> str:
        blocks.append(m.group(1).strip())
        return MERMAID_PH.format(i=len(blocks) - 1)

    return re.sub(r"```mermaid\n([\s\S]*?)```", repl, text), blocks


def restore_mermaid(html_body: str, blocks: list[str]) -> str:
    for i, block in enumerate(blocks):
        diagram = f'<pre class="mermaid">{html_lib.escape(block)}</pre>'
        html_body = html_body.replace(MERMAID_PH.format(i=i), diagram)
    return html_body


def add_header_ids(html_body: str) -> str:
    def repl(m: re.Match) -> str:
        level, content = m.group(1), m.group(2)
        plain = re.sub(r"<[^>]+>", "", content)
        slug = re.sub(r"\s+", "-", plain.strip().lower())
        slug = slug.replace("—", "--").replace("–", "-")
        slug = re.sub(r"[^\w\u0e00-\u0e7f-]", "", slug, flags=re.UNICODE)
        slug = re.sub(r"-{3,}", "--", slug)
        return f'<h{level} id="{slug}">{content}</h{level}>'

    return re.sub(r"<h([2-4])>(.*?)</h\1>", repl, html_body, flags=re.DOTALL)


def md_to_html(md_chunk: str) -> str:
    md_chunk, mermaid = protect_mermaid(md_chunk)
    converter = markdown.Markdown(extensions=[TableExtension(), FencedCodeExtension()])
    body = converter.convert(md_chunk)
    body = restore_mermaid(body, mermaid)
    body = add_header_ids(body)
    body = body.replace('src="./images/', 'src="../images/')
    return body


def parse_sections(md_text: str) -> tuple[str, dict[int, str]]:
    md_text = re.sub(r"## สารบัญ[\s\S]*?---\n", "", md_text, count=1)
    md_text = re.sub(r"\n---\n", "\n", md_text)

    footer_m = re.search(r"\n(\*เอกสารฉบับนี้[\s\S]*)$", md_text)
    footer = footer_m.group(1).strip() if footer_m else ""
    if footer_m:
        md_text = md_text[: footer_m.start()].rstrip()

    header_m = re.match(r"(^[\s\S]*?)(?=\n## )", md_text)
    header = header_m.group(1).strip() if header_m else ""

    sections: dict[int, str] = {}
    for part in re.split(r"\n(?=## )", md_text):
        part = part.strip()
        if not part.startswith("## "):
            continue
        title_m = re.match(r"## (.+)", part)
        if not title_m:
            continue
        num_m = re.match(r"(\d+)", title_m.group(1))
        if num_m:
            sections[int(num_m.group(1))] = part

    if footer:
        sections[99] = footer
    return header, sections


def build_nav_cards(exclude_id: str = "index") -> str:
    cards = []
    for p in PAGES:
        if p["id"] == exclude_id:
            continue
        cards.append(
            f'<a class="nav-card" href="{p["file"]}">'
            f'<span class="nav-card-icon">{p["icon"]}</span>'
            f'<span class="nav-card-title">{p["nav"]}</span>'
            f'<span class="nav-card-desc">{p["subtitle"]}</span>'
            f"</a>"
        )
    return f'<div class="nav-cards">{"".join(cards)}</div>'


def build_nav(active_id: str) -> str:
    items = []
    for p in PAGES:
        cls = "nav-link active" if p["id"] == active_id else "nav-link"
        items.append(
            f'<a class="{cls}" href="{p["file"]}">'
            f'<span class="nav-icon">{p["icon"]}</span>'
            f'<span class="nav-text"><strong>{p["nav"]}</strong>'
            f'<small>{p["subtitle"]}</small></span></a>'
        )
    return "\n".join(items)


def render_page(page: dict, header: str, sections: dict[int, str]) -> str:
    chunks = []
    if 0 in page["sections"] and header:
        chunks.append(header)
    for n in page["sections"]:
        if n in sections and n != 0:
            chunks.append(sections[n])
    if page["id"] == "sync" and 99 in sections:
        chunks.append(sections[99])

    content = md_to_html("\n\n".join(c for c in chunks if c))
    nav = build_nav(page["id"])
    cards = build_nav_cards() if page["id"] == "index" else ""
    mermaid_script = ""
    if page["mermaid"]:
        mermaid_script = """
  <script type="module">
    import mermaid from "https://cdn.jsdelivr.net/npm/mermaid@11/dist/mermaid.esm.min.mjs";
    mermaid.initialize({ startOnLoad: true, theme: "neutral", securityLevel: "loose" });
  </script>"""

    return f"""<!DOCTYPE html>
<html lang="th">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>{page["nav"]} — Chillpay Operation Logs</title>
  <link rel="preconnect" href="https://fonts.googleapis.com">
  <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
  <link href="https://fonts.googleapis.com/css2?family=IBM+Plex+Sans+Thai:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500&display=swap" rel="stylesheet">
  <link rel="stylesheet" href="assets/style.css">
{mermaid_script}
</head>
<body>
  <button class="sidebar-toggle" id="sidebarToggle" aria-label="เปิดเมนู">☰</button>
  <div class="sidebar-overlay" id="sidebarOverlay"></div>

  <aside class="sidebar" id="sidebar">
    <div class="sidebar-brand">
      <div class="brand-icon">CP</div>
      <div>
        <div class="brand-title">Operation Logs</div>
        <div class="brand-sub">Chillpay Docs</div>
      </div>
    </div>
    <nav class="sidebar-nav">
      {nav}
    </nav>
    <div class="sidebar-footer">
      <span>อัปเดต 2026-06-28</span>
      <span>Phase 1</span>
    </div>
  </aside>

  <main class="main">
    <header class="page-header">
      <p class="page-eyebrow">Chillpay Operation Logs</p>
      <h1 class="page-title">{page["nav"]}</h1>
      <p class="page-subtitle">{page["subtitle"]}</p>
    </header>
    {cards}
    <article class="content prose">
      {content}
    </article>
    <footer class="page-footer">
      <a href="index.html" class="footer-link">← กลับภาพรวม</a>
    </footer>
  </main>

  <script src="assets/app.js"></script>
</body>
</html>"""


def write_css() -> None:
    ASSETS.mkdir(parents=True, exist_ok=True)
    (ASSETS / "style.css").write_text(CSS, encoding="utf-8")


def write_js() -> None:
    (ASSETS / "app.js").write_text(JS, encoding="utf-8")


CSS = r""":root {
  --sidebar-w: 280px;
  --bg: #f8fafc;
  --surface: #ffffff;
  --text: #0f172a;
  --text-muted: #64748b;
  --border: #e2e8f0;
  --accent: #2563eb;
  --accent-soft: #eff6ff;
  --sidebar-bg: linear-gradient(180deg, #0f172a 0%, #1e293b 100%);
  --sidebar-text: #cbd5e1;
  --sidebar-active: rgba(59, 130, 246, 0.18);
  --shadow: 0 1px 3px rgba(15, 23, 42, 0.08);
  --radius: 12px;
  --font: "IBM Plex Sans Thai", "IBM Plex Sans", system-ui, sans-serif;
  --mono: "JetBrains Mono", Consolas, monospace;
}

*, *::before, *::after { box-sizing: border-box; }

html { scroll-behavior: smooth; }

body {
  margin: 0;
  font-family: var(--font);
  color: var(--text);
  background: var(--bg);
  line-height: 1.7;
}

/* Sidebar */
.sidebar {
  position: fixed;
  top: 0; left: 0; bottom: 0;
  width: var(--sidebar-w);
  background: var(--sidebar-bg);
  color: var(--sidebar-text);
  display: flex;
  flex-direction: column;
  z-index: 100;
  border-right: 1px solid rgba(255,255,255,0.06);
}

.sidebar-brand {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 1.5rem 1.25rem 1.25rem;
  border-bottom: 1px solid rgba(255,255,255,0.08);
}

.brand-icon {
  width: 42px; height: 42px;
  background: linear-gradient(135deg, #3b82f6, #6366f1);
  border-radius: 10px;
  display: grid;
  place-items: center;
  font-weight: 700;
  font-size: 0.85rem;
  color: #fff;
  letter-spacing: -0.5px;
}

.brand-title { font-weight: 700; color: #f8fafc; font-size: 1rem; }
.brand-sub { font-size: 0.75rem; color: #94a3b8; margin-top: 2px; }

.sidebar-nav {
  flex: 1;
  overflow-y: auto;
  padding: 1rem 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 0.65rem 0.85rem;
  border-radius: 10px;
  text-decoration: none;
  color: var(--sidebar-text);
  transition: background 0.15s, color 0.15s;
}

.nav-link:hover {
  background: rgba(255,255,255,0.06);
  color: #f1f5f9;
}

.nav-link.active {
  background: var(--sidebar-active);
  color: #fff;
  box-shadow: inset 3px 0 0 #3b82f6;
}

.nav-icon {
  width: 28px;
  text-align: center;
  font-size: 1rem;
  opacity: 0.85;
}

.nav-text { display: flex; flex-direction: column; line-height: 1.3; }
.nav-text strong { font-size: 0.9rem; font-weight: 600; }
.nav-text small { font-size: 0.72rem; color: #94a3b8; margin-top: 2px; }
.nav-link.active .nav-text small { color: #93c5fd; }

.sidebar-footer {
  padding: 1rem 1.25rem;
  border-top: 1px solid rgba(255,255,255,0.08);
  font-size: 0.72rem;
  color: #64748b;
  display: flex;
  justify-content: space-between;
}

/* Main */
.main {
  margin-left: var(--sidebar-w);
  min-height: 100vh;
  padding: 2.5rem 3rem 4rem;
}

.page-header {
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid var(--border);
}

.page-eyebrow {
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: var(--accent);
  margin: 0 0 0.5rem;
}

.page-title {
  font-size: 2.25rem;
  font-weight: 700;
  margin: 0;
  letter-spacing: -0.02em;
  color: var(--text);
}

.page-subtitle {
  margin: 0.5rem 0 0;
  color: var(--text-muted);
  font-size: 1.05rem;
}

.content {
  max-width: 860px;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 2rem 2.25rem;
  box-shadow: var(--shadow);
}

.page-footer {
  max-width: 860px;
  margin-top: 1.5rem;
}

.footer-link {
  color: var(--text-muted);
  text-decoration: none;
  font-size: 0.9rem;
}
.footer-link:hover { color: var(--accent); }

/* Prose */
.prose h1 { font-size: 1.75rem; margin: 0 0 1rem; display: none; }
.prose h2 {
  font-size: 1.35rem;
  margin: 2rem 0 1rem;
  padding-bottom: 0.4rem;
  border-bottom: 2px solid var(--accent-soft);
  color: var(--text);
}
.prose h2:first-child { margin-top: 0; }
.prose h3 { font-size: 1.1rem; margin: 1.75rem 0 0.75rem; color: #1e293b; }
.prose h4 { font-size: 1rem; margin: 1.25rem 0 0.5rem; }

.prose blockquote {
  margin: 1rem 0 1.5rem;
  padding: 1rem 1.25rem;
  background: linear-gradient(135deg, #eff6ff, #f0fdf4);
  border-left: 4px solid var(--accent);
  border-radius: 0 8px 8px 0;
  color: #334155;
}
.prose blockquote p { margin: 0.25rem 0; }

.prose table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  margin: 1rem 0 1.5rem;
  font-size: 0.88rem;
  border: 1px solid var(--border);
  border-radius: 8px;
  overflow: hidden;
}

.prose th, .prose td {
  padding: 0.6rem 0.85rem;
  text-align: left;
  border-bottom: 1px solid var(--border);
  vertical-align: top;
}

.prose th {
  background: #f1f5f9;
  font-weight: 600;
  color: #334155;
}

.prose tr:last-child td { border-bottom: none; }
.prose tr:hover td { background: #fafbfc; }

.prose code {
  font-family: var(--mono);
  font-size: 0.84em;
  background: #f1f5f9;
  padding: 0.15em 0.4em;
  border-radius: 5px;
  color: #be185d;
}

.prose pre {
  background: #1e293b;
  color: #e2e8f0;
  border-radius: 10px;
  padding: 1.1rem 1.25rem;
  overflow-x: auto;
  font-size: 0.8rem;
  line-height: 1.55;
  margin: 1rem 0 1.5rem;
  border: 1px solid #334155;
}

.prose pre code {
  background: none;
  color: inherit;
  padding: 0;
  font-size: inherit;
}

.prose pre.mermaid {
  background: #fff;
  color: var(--text);
  border: 1px dashed var(--border);
  text-align: center;
  padding: 1.5rem;
}

.prose img {
  max-width: 100%;
  height: auto;
  border-radius: 10px;
  border: 1px solid var(--border);
  margin: 1rem 0 1.5rem;
  box-shadow: 0 4px 12px rgba(15,23,42,0.08);
}

.prose hr {
  border: none;
  border-top: 1px solid var(--border);
  margin: 2rem 0;
}

.prose ul, .prose ol { padding-left: 1.4rem; }
.prose li { margin: 0.3rem 0; }
.prose a { color: var(--accent); text-decoration: none; }
.prose a:hover { text-decoration: underline; }
.prose p { margin: 0.75rem 0; }
.prose em { color: var(--text-muted); }

.prose strong { font-weight: 600; }

/* Index nav cards */
.nav-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 12px;
  max-width: 860px;
  margin-bottom: 1.5rem;
}

.nav-card {
  display: flex;
  flex-direction: column;
  gap: 4px;
  padding: 1rem 1.1rem;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  text-decoration: none;
  color: var(--text);
  transition: border-color 0.15s, box-shadow 0.15s, transform 0.15s;
  box-shadow: var(--shadow);
}

.nav-card:hover {
  border-color: #93c5fd;
  box-shadow: 0 4px 16px rgba(37, 99, 235, 0.12);
  transform: translateY(-2px);
}

.nav-card-icon { font-size: 1.25rem; }
.nav-card-title { font-weight: 600; font-size: 0.95rem; }
.nav-card-desc { font-size: 0.75rem; color: var(--text-muted); line-height: 1.4; }

/* Mobile */
.sidebar-toggle {
  display: none;
  position: fixed;
  top: 1rem; left: 1rem;
  z-index: 200;
  width: 44px; height: 44px;
  border: none;
  border-radius: 10px;
  background: #1e293b;
  color: #fff;
  font-size: 1.25rem;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(0,0,0,0.2);
}

.sidebar-overlay {
  display: none;
  position: fixed;
  inset: 0;
  background: rgba(15,23,42,0.5);
  z-index: 90;
  backdrop-filter: blur(2px);
}

@media (max-width: 900px) {
  .sidebar {
    transform: translateX(-100%);
    transition: transform 0.25s ease;
  }
  .sidebar.open { transform: translateX(0); }
  .sidebar-overlay.open { display: block; }
  .sidebar-toggle { display: grid; place-items: center; }
  .main { margin-left: 0; padding: 5rem 1.25rem 3rem; }
  .content { padding: 1.25rem 1rem; }
  .page-title { font-size: 1.75rem; }
}
"""

JS = r"""document.addEventListener('DOMContentLoaded', () => {
  const sidebar = document.getElementById('sidebar');
  const overlay = document.getElementById('sidebarOverlay');
  const toggle = document.getElementById('sidebarToggle');

  function closeSidebar() {
    sidebar?.classList.remove('open');
    overlay?.classList.remove('open');
  }

  toggle?.addEventListener('click', () => {
    sidebar?.classList.toggle('open');
    overlay?.classList.toggle('open');
  });

  overlay?.addEventListener('click', closeSidebar);

  document.querySelectorAll('.sidebar-nav a').forEach((a) => {
    a.addEventListener('click', () => {
      if (window.innerWidth <= 900) closeSidebar();
    });
  });
});
"""


def main() -> None:
    SITE.mkdir(parents=True, exist_ok=True)
    write_css()
    write_js()

    header, sections = parse_sections(MD_FILE.read_text(encoding="utf-8"))

    for page in PAGES:
        html = render_page(page, header, sections)
        out = SITE / page["file"]
        out.write_text(html, encoding="utf-8")
        print(f"  {out.relative_to(DOCS)}")

    print(f"\nDone — open {SITE / 'index.html'}")


if __name__ == "__main__":
    main()

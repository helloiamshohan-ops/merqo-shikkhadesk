# WORK LOG — MERQO ShikkhaDesk

Append-only running log of engineering sessions. Each entry records what was
done, the evidence, and the honest verification status. Newest entries at the
bottom.

---

## 2026-10-19 — Session 1 (branch `arena/f4a56233-merqo-shikkhadesk`)

**Goal (PART 33):** evidence-based audit of the actual repository and
environment, then begin implementation without fabricating progress.

**Done:**
- Audited repository: single commit `5bb9eef`, one tracked file (`README.md`
  containing only `# merqo-shikkhadesk`), clean tree. No prior implementation.
- Audited environment (measured): Debian 12 Linux, Python 3.11.2, Node 22.22.3,
  Pillow 13.0.0 (installed from PyPI). **No .NET, no Mono, no Java.**
- Probed network: `github.com`, `codeload.github.com`, `api.github.com`, PyPI and
  npm reachable; `dot.net`, `builds.dotnet.microsoft.com`, `deb.debian.org`, and
  the GitHub release-asset host (`objects.githubusercontent.com`) **blocked**.
- **Recorded the central blocker:** no .NET SDK is obtainable and WPF cannot be
  built/run on Linux, so no executable/installer/executed-test can be produced in
  this environment. Recorded in `PROJECT-STATE.md` and `KNOWN-ISSUES.md`.
- Created layered solution directory structure under `src/` and `tests/`.
- Wrote checkpoint foundation: `README.md`, `docs/PROJECT-STATE.md`, this log.

**Verification status:** audit facts are measured and reproducible. No build or
test was run (none is possible here). Nothing is claimed as compiled/working.

**Next:** competitor research → brand assets (genuine ICO) → full documentation
suite → .NET source scaffold.

**Continued (same day):**
- Completed competitor research (public sources, 2026-10-19); confirmed the
  **Shikkha/Shiksha + …Desk naming-conflict** the prompt anticipated (ShikkhaPlus,
  Shikkha, CoachDesk, etc.) → documented in `COMPETITOR-RESEARCH.md`, escalated as
  DEC-1 (no silent rename). Wrote the 5 research docs + gap matrix + positioning.
- Generated **original brand assets**: SVG vector logo + mark, multi-resolution
  PNGs, light/dark lockups, and a **genuine multi-resolution Windows `.ico`**
  (16–256px, verified ICONDIR type=1, 7 embedded PNG entries — not a renamed PNG).
  Bundled SIL-OFL Noto Sans Bengali font. Wrote `BRAND-GUIDELINES.md` +
  `BANGLA-TERMINOLOGY.md`.
- Wrote the full documentation suite: requirements, feature matrix, architecture,
  data model, accounting design, security plan, license design, license-manager
  guide, LAN setup, backup/restore, installation guide, UI design spec, Bangla
  user guide, test plan, test results, changelog.
- Scaffolded the layered .NET solution (`src/ShikkhaDesk.sln` + 11 projects + 3
  test projects) with **real, reviewable logic**: `Money` (integer poisha),
  double-entry `JournalEntry` (balance + immutability + reversal), `Ledger`
  (trial balance, profit), `PostingRules` (all standard mappings), and the
  canonical signed institution-bound `Licensing` (sign/verify). Authored xUnit
  suites for the financial invariants and license cases.
- **Verification status:** docs/assets are real and (for the ICO) verified. All C#
  is **authored, NOT compiled or tested** — no .NET runtime here. `TEST-RESULTS.md`
  records 0 executed tests; all release gates remain OPEN.

---

_(Subsequent sessions append here.)_

# CHANGELOG — MERQO ShikkhaDesk

All notable changes to this project. Format follows Keep a Changelog. Product
name and versioning are set by the owner; entries reflect real work only.

## [Unreleased]

### Added
- **Foundation & audit:** repository/environment audit; checkpoint docs
  (`PROJECT-STATE`, `WORK-LOG`, `KNOWN-ISSUES`, `OPEN-DECISIONS`,
  `RELEASE-CHECKLIST`, `DEVELOPMENT-PLAN`).
- **Research:** `COMPETITOR-RESEARCH`, `COMPETITOR-GAP-MATRIX`,
  `PRODUCT-POSITIONING`, `FEATURE-COMPARISON`, `UX-RESEARCH` (public sources,
  2026-10-19; facts separated from analysis).
- **Brand assets:** original vector logo (`assets/logo/logo.svg`, `mark.svg`),
  multi-resolution PNGs, light/dark lockups, and a **genuine multi-resolution
  Windows `.ico`** (`assets/icons/shikkhadesk.ico`, 16–256px, verified).
  Bundled SIL-OFL Noto Sans Bengali font (`assets/fonts/`).
- **Docs:** `BRAND-GUIDELINES`, `BANGLA-TERMINOLOGY`, `REQUIREMENTS`,
  `FEATURE-MATRIX`, `ARCHITECTURE`, `DATA-MODEL`, `ACCOUNTING-DESIGN`,
  `SECURITY-PLAN`, `LICENSE-DESIGN`, `LICENSE-MANAGER-GUIDE`, `LAN-SETUP`,
  `BACKUP-RESTORE`, `INSTALLATION-GUIDE`, `UI-DESIGN-SPEC`, `USER-GUIDE-BN`,
  `TEST-PLAN`, `TEST-RESULTS`.
- **Source scaffold:** layered .NET solution projects under `src/` (Domain,
  Accounting, Licensing, Persistence, Application, LanTransport, Reporting,
  Backup, Diagnostics, Presentation, LicenseManager) with foundational source
  and a documented build/test path.

### Honest status
- **No executable, installer, or executed test exists.** The environment is
  Debian Linux with no .NET SDK and cannot build/run WPF (`KNOWN-ISSUES.md`
  BLOCKER-1). Source is **authored, not compiled/tested**. See `PROJECT-STATE.md`.

## [0.0.0] — 2026-10-19
- Repository initialized (empty `README`). No product code.

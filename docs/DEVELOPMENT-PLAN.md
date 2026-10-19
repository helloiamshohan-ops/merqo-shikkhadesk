# DEVELOPMENT PLAN — MERQO ShikkhaDesk

Phased plan derived from PART 31, adapted to the measured environment. Each phase
lists deliverables, dependencies, and its honest status.

---

## Environment reality (drives sequencing)

No Windows/.NET toolchain is available in the current environment (see
`KNOWN-ISSUES.md` BLOCKER-1). Phases are therefore split into:

- **Offline-doable now:** research, documentation, brand assets, source authoring,
  design review.
- **Blocked until Windows + .NET SDK:** all compile, test, LAN, installer and
  installation phases.

---

## Phase status

### Phase 0 — Repository & environment audit ✅
Deliverable: `PROJECT-STATE.md`, `KNOWN-ISSUES.md`. Done, evidence recorded.

### Phase 1 — Competitor research 🟡
Deliverables: `COMPETITOR-RESEARCH.md`, `COMPETITOR-GAP-MATRIX.md`,
`PRODUCT-POSITIONING.md`, `FEATURE-COMPARISON.md`, `UX-RESEARCH.md`.
Facts separated from analysis. In progress this session.

### Phase 2 — Scope & feature matrix 🟡
Deliverables: `REQUIREMENTS.md`, `FEATURE-MATRIX.md` (Core/Advanced/Optional/
Future/Not-required per feature).

### Phase 3 — Branding & Bangla terminology ✅/🟡
Deliverables: original SVG logo, light/dark PNG, **genuine multi-resolution
`.ico`**, `BRAND-GUIDELINES.md`, `BANGLA-TERMINOLOGY.md`. Assets generated &
verified with Pillow this session.

### Phase 4 — UI/UX specification 🟡
Deliverable: `UI-DESIGN-SPEC.md` with the full 48-screen inventory mapped to
real workflows (no placeholder screens).

### Phase 5 — Architecture & database design 🟡
Deliverables: `ARCHITECTURE.md`, `DATA-MODEL.md`, `ACCOUNTING-DESIGN.md`,
`SECURITY-PLAN.md`, `LICENSE-DESIGN.md`.

### Phase 6 — Application foundation & first-run setup 🟡
Deliverables: solution + project scaffold, domain model, setup wizard design.
Blocked for build.

### Phase 7 — Authentication & permissions 🟡
Deliverables: users/roles/permissions domain + service design; password hashing;
audit log. Blocked for build/test.

### Phase 8 — Students & academic modules 🟡
Deliverables: students, admissions, sessions, courses, classes, batches,
attendance, exams, results (domain + services). Blocked for build/test.

### Phase 9 — Fees & payments 🟡
Deliverables: fee plans, assessment, charges, payments, allocations, advance
credit, receipts, refunds, reversals. Blocked for build/test.

### Phase 10 — Complete accounting 🟡
Deliverables: chart of accounts, journal engine (balanced, atomic), income/
expense, cash/bank, transfers, receivables/payables, payroll, financial
statements. Blocked for build/test.

### Phase 11 — Reports, exports, printing 🟡
Deliverables: report center, CSV/Excel import-export, printable registers and
receipts. Blocked for build/test.

### Phase 12 — LAN host/client ⛔
Deliverables: authenticated host service, client transport, idempotency,
reconnection, device registration. Needs Windows to build & test.

### Phase 13 — License verification 🟡
Deliverables: signed, institution-bound, offline activation (domain + design).
Test execution blocked.

### Phase 14 — Private License Manager 🟡
Deliverables: separate seller-side app + register; signing with test keys only.
Build blocked; no production key without owner authorization (DEC-3).

### Phase 15 — Backup & recovery 🟡
Deliverables: backup/restore design + service. Windows runtime verification
blocked.

### Phase 16 — UI refinement 🟡
Deliverables: WPF design system, Bangla-first copy. Runtime blocked.

### Phase 17 — Security & regression testing ⛔
Deliverables: full test suites executed. Blocked (needs .NET runtime).

### Phase 18 — Customer installer ⛔
Deliverables: `ShikkhaDesk-Setup-vX.Y.Z-x64.exe`. Blocked (needs Windows).

### Phase 19 — License Manager installer ⛔
Deliverables: `ShikkhaDesk-LicenseManager-Setup-vX.Y.Z-x64.exe`. Blocked.

### Phase 20 — Upgrade & recovery testing ⛔
Deliverables: install/upgrade/failed-upgrade/backup-restore/reinstall matrix.
Blocked.

### Phase 21 — Documentation & release prep 🟡
Deliverables: full `docs/` set, `CHANGELOG.md`, screenshots. Near complete;
screenshots blocked (need running app).

### Phase 22 — Final verified release ⛔
Deliverables: verified artifacts + all gates PASS. Blocked until Windows/.NET.

---

## Definition of "ready to move a phase forward"

A phase advances only when its deliverables exist **and** (for anything that must
run) its evidence exists. Authored-but-unbuilt artifacts advance a phase only to
"🟡", never to "✅".

# RELEASE CHECKLIST — MERQO ShikkhaDesk (PART 32 gates)

**Status of the whole checklist: NOT READY FOR COMMERCIAL RELEASE.**

Each gate is listed with its current, honest state. A gate is only marked PASS
when the evidence exists (build log, test run id, installer checksum, screenshot).
No gate is marked PASS in this environment.

Legend: ⛔ blocked · ⬜ not started · 🟡 in progress / authored · ✅ passed

| # | Gate (PART 32) | State | Evidence / what's needed |
|---|----------------|-------|--------------------------|
| 1 | Required features implemented | ⬜ | Core not compiled; see FEATURE-MATRIX |
| 2 | Interface uses simple Bengali consistently | 🟡 | Glossary + UI spec authored; runtime check pending |
| 3 | MERQO branding correct | 🟡 | Assets + guidelines authored; app-wide wiring pending |
| 4 | About page includes merqoonline@gmail.com | 🟡 | Spec'd; WPF About not built |
| 5 | Financial transactions reconcile | ⬜ | Designed; **no test executed** |
| 6 | Journal entries balance (debits=credits) | ⬜ | Designed; **no test executed** |
| 7 | LAN operation tested | ⛔ | Needs Windows + 2+ machines |
| 8 | Device limits enforced | ⬜ | Designed; **no test executed** |
| 9 | License Manager is separate | 🟡 | Separate project scaffolded; not built |
| 10 | Production private key not embedded in customer app | ✅ (by design) | No production key exists; only test keys; enforced in LICENSE-DESIGN |
| 11 | Institution-bound license validation works | ⬜ | Designed; **no test executed** |
| 12 | Offline activation works | ⬜ | Designed; **no test executed** |
| 13 | Backup and restore verified | ⛔ | Needs Windows + SQLite runtime |
| 14 | Actual Windows installer exists | ⛔ | Needs Windows + installer toolchain |
| 15 | Installation and upgrade tested | ⛔ | Needs clean Windows environment |
| 16 | Customer data preserved (install/upgrade/uninstall) | ⛔ | Needs Windows |
| 17 | Documentation complete | 🟡 | `docs/` near-complete; keep current |
| 18 | Known issues disclosed | ✅ | `KNOWN-ISSUES.md` maintained |
| 19 | Release artifacts verified (hashes, real builds) | ⛔ | No artifacts built yet |

**Hard rule:** The product must not be marketed, sold, or described as
commercially ready until every gate above is ✅ with recorded evidence.

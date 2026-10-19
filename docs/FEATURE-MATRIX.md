# FEATURE MATRIX — MERQO ShikkhaDesk

ShikkhaDesk's own feature classification (PART 3.2 / PART 4 tiers). Every feature
is labeled **Core / Advanced / Optional / Future-extension / Not-required**.
This governs scope and what may be advertised as available.

Legend: **Core** = v1.0 Tier A · **Advanced** = v1.0 Tier B (full accounting) ·
**Optional** = possible add-on, not depended on · **Future** = Tier C, staged ·
**N/R** = not required.

| Area | Feature | Class | v1.0? |
|------|---------|-------|-------|
| Setup | Institution setup wizard | Core | ✅ |
| Setup | Locale/digits/currency/receipt/backup config | Core | ✅ |
| Setup | Host/client + LAN config | Core | ✅ |
| Setup | License activation | Core | ✅ |
| Users | Users, roles, granular permissions | Core | ✅ |
| Users | Password hashing, sessions, failed-login | Core | ✅ |
| Users | Audit history | Core | ✅ |
| Students | Registration, stable ID, profile | Core | ✅ |
| Students | Search/filter/sort/paginate, dup detection | Core | ✅ |
| Students | Archive/reactivate (history preserved) | Core | ✅ |
| Students | Export | Advanced | ✅ |
| Academic | Sessions, classes, courses, subjects | Core | ✅ |
| Academic | Batches, weekly schedules, teacher assignment | Core | ✅ |
| Academic | Admission, enrollment, roll numbers | Core | ✅ |
| Academic | Batch/course transfer, withdrawal, reactivation | Core | ✅ |
| Academic | Batch registers | Core | ✅ |
| Academic | Capacity limits | Optional | ✅ |
| Attendance | Daily/batch/session, bulk entry | Core | ✅ |
| Attendance | Present/Absent/Late/Leave, makeup, correction | Core | ✅ |
| Attendance | Teacher attendance, %, monthly reports, registers | Core | ✅ |
| Exams | Setup, schedule, subject allocation, max/pass marks | Core | ✅ |
| Exams | Marks entry, absent, grading, GPA, merit list | Core | ✅ |
| Exams | Publish/unpublish, correction, report cards, sheets | Core | ✅ |
| Exams | Question bank / MCQ / paper builder | Future | ❌ |
| Exams | Admit card | Future | ❌ |
| Fees | Fee types, plans, effective dates | Core | ✅ |
| Fees | Assessment, bulk generation, preview (no dup) | Core | ✅ |
| Fees | Outstanding dues, overdue reports | Core | ✅ |
| Fees | Discounts/scholarships/adjustments/late fees | Core | ✅ |
| Fees | Student/batch-specific fees | Advanced | ✅ |
| Payments | Full/partial, manual/auto allocation | Core | ✅ |
| Payments | Cash/bank; recorded bKash/Nagad (manual) | Core | ✅ |
| Payments | Receipts, refund, reversal | Core | ✅ |
| Payments | Advance credit ledger | Core | ✅ |
| Payments | Idempotency, no double-allocate/refund | Core | ✅ |
| Payments | Live bKash/Nagad/gateway API | Optional | ❌ |
| Accounting | Chart of accounts | Core | ✅ |
| Accounting | Journal engine (balanced, atomic) | Advanced | ✅ |
| Accounting | Income & expense + ledger integration | Core | ✅ |
| Accounting | Cashbook, bankbook, multi-account, transfers | Core/Advanced | ✅ |
| Accounting | Reconciliation, bank charges | Advanced | ✅ |
| Accounting | Receivables, payables, suppliers, aging | Advanced | ✅ |
| Accounting | Payroll (structure, payslip, posting) | Advanced | ✅ |
| Accounting | Trial balance, P&L, balance sheet, cash-flow | Advanced | ✅ |
| Accounting | GL, account ledger | Advanced | ✅ |
| Accounting | VAT/statutory compliance | N/R | ❌ |
| Reports | Report center, drill-down | Core | ✅ |
| Reports | CSV import/export (Excel where practical) | Advanced | ✅ |
| Reports | Printable receipts/registers/statements | Core | ✅ |
| Dashboard | Metrics from real records, permissioned | Core | ✅ |
| LAN | Single-computer mode | Core | ✅ |
| LAN | Host service/API + client transport | Core | ✅ |
| LAN | Concurrency, idempotency, reconnection, device reg | Core | ✅ |
| License | Signed, institution-bound, offline activation | Core | ✅ |
| License | Device limit (default 5), replacement, recovery | Core | ✅ |
| License | Separate private License Manager | Core | ✅ |
| Backup | Auto+manual backup, retention, integrity, restore | Core | ✅ |
| Backup | Pre-restore backup, recovery guide | Core | ✅ |
| Import/Export | Template, mapping, preview, validation, summary | Advanced | ✅ |
| Extras | Notices/reminders (local) | Optional | ✅ |
| Extras | Academic calendar | Optional | ✅ |
| Extras | Library | Future | ❌ |
| Extras | Inventory & assets | Future | ❌ |
| Extras | Certificates & ID cards | Future | ❌ |
| Extras | Multi-branch | Future | ❌ |
| Extras | Parent/student portal | Future | ❌ |
| Extras | SMS / WhatsApp | Optional | ❌ |
| Extras | Biometric devices | Optional | ❌ |
| Extras | Website / online courses / LMS | N/R | ❌ |
| Extras | Transport | N/R | ❌ |

## v1.0 scope statement

**v1.0 = all Core + all Advanced = Tier A (core coaching operations) + full Tier B
(complete double-entry accounting).** The omission of Tier C and Optional
integrations is intentional and documented here for owner approval (DEC-5). No
unimplemented module is advertised as available.

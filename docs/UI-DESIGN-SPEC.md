# UI DESIGN SPEC — MERQO ShikkhaDesk

PART 20. A polished, consistent Windows business-application design in simple
Bengali. **No placeholder screens** — every screen maps to a real workflow.

**Status: design spec. The WPF implementation (`ShikkhaDesk.Presentation`) is
scaffolded but not built (needs Windows). Screenshots are blocked until the app
runs.**

---

## 1. Shell (main layout)
- **Left sidebar:** primary navigation (Bengali labels + icons), collapsible.
- **Top bar:** page title, global search, connection status, license status,
  backup status, digit-preference indicator, user menu, version.
- **Content area:** page-specific toolbars, filters, tables, forms.
- **Notifications:** non-blocking toasts for success/error; modal confirmations for
  destructive/financial actions.
- **Footer/status:** schema version, LAN/host status, last backup time.

## 2. Design rules (PART 20)
Simple Bengali · consistent typography (Noto Sans Bengali) · comfortable spacing ·
clear primary/secondary buttons · readable financial values (tabular digits, no
tiny text on money screens) · consistent icon set · clear inline validation ·
keyboard navigation + visible focus · proper DPI scaling · no needless animation ·
no decorative clutter. Palette per `BRAND-GUIDELINES.md`.

## 3. Navigation map (Bengali)
প্রধান পাতা · শিক্ষার্থী · ভর্তি · একাডেমিক · উপস্থিতি · পরীক্ষা ও ফলাফল · ফি ·
হিসাব · বেতন · সরবরাহকারী · রিপোর্ট · ব্যবস্থাপনা · সম্পর্কে

## 4. Screen inventory (48 screens — all real workflows)

| # | Screen | Purpose / key actions |
|---|--------|-----------------------|
| 1 | Welcome/Setup (first run) | 10-step Bangla wizard |
| 2 | Login | username/password, failed-login handling |
| 3 | Dashboard (প্রধান পাতা) | permissioned metrics, drill-down |
| 4 | Student list (শিক্ষার্থী) | search/filter/sort/paginate, bulk actions |
| 5 | Student registration | validated form, duplicate detection |
| 6 | Student profile | tabs: academic, attendance, exams, payments, receipts, statement, balance |
| 7 | Admission (ভর্তি) | admission + fee-plan assignment + admission fee |
| 8 | Class/course management | classes, courses, subjects |
| 9 | Batch management (ব্যাচ) | batches, schedules, capacity, teacher assignment |
| 10 | Staff management (কর্মী) | profiles, designations, assignments, salary (restricted) |
| 11 | Attendance (উপস্থিতি) | fast batch entry, bulk, correction |
| 12 | Exam setup (পরীক্ষা) | exams, subjects, max/pass marks, schedule |
| 13 | Marks entry (নম্বর) | grid entry, absent, correction |
| 14 | Results (ফলাফল) | grading, GPA, merit, publish/unpublish, report cards |
| 15 | Fee-plan setup (ফি পরিকল্পনা) | fee types, plans, effective dates |
| 16 | Fee assessment (ফি নির্ধারণ) | single + bulk, preview, no-dup guard |
| 17 | Outstanding dues (বকেয়া) | overdue lists, statements |
| 18 | Payment collection (ফি আদায়) | full/partial, allocation, methods, idempotency |
| 19 | Receipt preview (রসিদ) | print layout, reprint |
| 20 | Advance-credit ledger (অগ্রিম জমা) | received/allocated/refunded/available |
| 21 | Refund/reversal (ফেরত) | bounded refunds, reversal with source |
| 22 | Chart of accounts (হিসাবের তালিকা) | accounts, categories, active |
| 23 | Income (আয়) | entry, voucher, register |
| 24 | Expenses (খরচ) | entry, voucher, approval |
| 25 | Journal entries (জার্নাল এন্ট্রি) | create/post/reverse; balance enforced |
| 26 | Cashbook (নগদ হিসাব) | running balance, register |
| 27 | Bankbook (ব্যাংক হিসাব) | multi-account, reconciliation |
| 28 | Account ledger (খতিয়ান) | per-account posted lines |
| 29 | Trial balance (রেওয়ামিল) | debits=credits |
| 30 | Profit & loss (লাভ-ক্ষতি) | period report |
| 31 | Balance sheet (ব্যালেন্স শিট) | assets = liabilities + equity |
| 32 | Cash flow (নগদ প্রবাহ) | derived from ledger |
| 33 | Supplier/payables (সরবরাহকারী) | suppliers, bills, settlement, aging |
| 34 | Payroll (বেতন হিসাব) | structure, run, payslip, posting |
| 35 | Report center (রিপোর্ট) | all reports, filters, export, print |
| 36 | User management (ব্যবহারকারী) | users, roles, activation |
| 37 | Permission editor (অনুমতি) | granular per-role permissions |
| 38 | Audit log (পরিবর্তন ইতিহাস) | append-only history, filters |
| 39 | Backup (নিরাপদ কপি) | auto/manual, history, integrity |
| 40 | Restore (পুনরুদ্ধার) | choose, confirm, pre-restore backup |
| 41 | Host configuration (হোস্ট) | host mode, port, firewall guidance |
| 42 | LAN status (নেটওয়ার্ক) | connection, devices, status |
| 43 | License activation (লাইসেন্স) | select file/code, verify, activate |
| 44 | Registered devices (নিবন্ধিত কম্পিউটার) | list, replace, release, limit |
| 45 | Institution profile (প্রতিষ্ঠান) | details, logo, immutable uid shown |
| 46 | Receipt settings (রসিদ নির্ধারণ) | format, numbering, prefix |
| 47 | General settings (সেটিংস) | locale, digits, currency, session, backup dir |
| 48 | About (সম্পর্কে) | brand identity (below) |

## 5. About screen (PART 1.2) — required content
- **ShikkhaDesk** · **A MERQO Product**
- "কোচিং সেন্টারের ব্যবস্থাপনা, শিক্ষার্থী, ফি ও হিসাবের সমন্বিত সফটওয়্যার।"
- "প্রস্তুতকারক: MERQO" · "যোগাযোগ: merqoonline@gmail.com"
- App version · build/release id · DB schema version · license status ·
  copyright · support instructions.
- **No** invented address/phone/website/registration/social. No unverified legal
  status. Same identity in the License Manager About and docs.

## 6. Common components
- Data table (sort, filter, paginate, column choose, export).
- Form layout with inline Bangla validation.
- Money cell (tabular digits, digit preference, ৳).
- Date cell (Gregorian, digit preference).
- Confirm dialog (names record + amount).
- Status badges (license/LAN/backup).
- Print preview (receipts, registers, statements).

## 7. Accessibility & scaling
Keyboard-reachable controls, visible focus, ≥ readable font sizes (financial
screens larger), DPI-aware layout, sufficient color contrast, status never conveyed
by color alone.

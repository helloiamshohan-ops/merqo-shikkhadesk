# BANGLA TERMINOLOGY — MERQO ShikkhaDesk

Single source of truth for customer-facing Bengali wording (PART 7). **One
concept = one Bengali term**, used identically in every module, screen, report,
receipt and message. Terms are to be **validated with real coaching-center staff**
before release (per directive); changes are logged at the bottom.

## Core glossary

| English concept | Bengali term (use this) | Notes |
|-----------------|-------------------------|-------|
| Dashboard | প্রধান পাতা | home/landing |
| Students | শিক্ষার্থী | |
| Student | শিক্ষার্থী | |
| Admissions | ভর্তি | |
| Admission | ভর্তি | |
| Enrollment / Enroll | ভর্তি | enrollment record = **ভর্তি রেকর্ড**; use **ভর্তি** consistently |
| Batch | ব্যাচ | |
| Class | শ্রেণি | |
| Course | কোর্স | |
| Subject | বিষয় | |
| Attendance | উপস্থিতি | |
| Present | উপস্থিত | |
| Absent | অনুপস্থিত | |
| Late | দেরিতে | |
| Exam | পরীক্ষা | |
| Marks | নম্বর | |
| Result | ফলাফল | |
| Fee | ফি | |
| Fee Assessment | ফি নির্ধারণ | |
| Fee Collection | ফি আদায় | |
| Outstanding (due) | বকেয়া | |
| Advance Credit | অগ্রিম জমা | |
| Receipt | রসিদ | |
| Payment | পেমেন্ট / পরিশোধ | prefer **পরিশোধ** in prose, **পেমেন্ট** in tables |
| Income | আয় | |
| Expense | খরচ | |
| Cashbook | নগদ হিসাব | |
| Bankbook | ব্যাংক হিসাব | |
| Payroll | বেতন হিসাব | |
| General Ledger | সাধারণ খতিয়ান | |
| Trial Balance | রেওয়ামিল | |
| Profit and Loss | লাভ-ক্ষতি | |
| Balance Sheet | ব্যালেন্স শিট | use **ব্যালেন্স শিট** in UI |
| Chart of Accounts | হিসাবের তালিকা (চার্ট অফ অ্যাকাউন্টস) | show Bengali first |
| Journal Entry | জার্নাল এন্ট্রি / দিনলিপি | use **জার্নাল এন্ট্রি** in UI |
| Backup | তথ্যের নিরাপদ কপি | |
| Restore | তথ্য পুনরুদ্ধার | |
| License | ব্যবহারের অনুমতিপত্র | |
| Settings | সেটিংস / নির্ধারণ | use **সেটিংস** in UI |
| Users | ব্যবহারকারী | |
| Role | ভূমিকা | |
| Permission | অনুমতি | |
| Teacher | শিক্ষক | |
| Staff | স্টাফ / কর্মী | use **কর্মী** in prose |
| Guardian | অভিভাবক | |
| Supplier | সরবরাহকারী | |
| Payable | দেয়ানি | |
| Receivable | পেতে হবে / পাওনা | use **পাওনা** |
| Report | রিপোর্ট | |
| Print | প্রিন্ট | |
| Export | এক্সপোর্ট / সংরক্ষণ (CSV) | use **এক্সপোর্ট** |
| Save | সংরক্ষণ | |
| Cancel | বাতিল | |
| Delete | মুছুন | (soft delete → **আর্কাইভ** for archive) |
| Edit | সম্পাদনা | |
| Add / New | নতুন / যোগ করুন | |
| Search | খুঁজুন | |
| Date | তারিখ | |
| Amount | পরিমাণ | |
| Total | মোট | |
| Balance | ব্যালেন্স / স্থিতি | use **ব্যালেন্স** |
| Discount | ছাড় | |
| Scholarship | বৃত্তি | |
| Refund | ফেরত | |
| Reversal | উল্টানো / বাতিল এন্ট্রি | use **বাতিল এন্ট্রি** |
| Institution | প্রতিষ্ঠান | |
| Academic session | শিক্ষাবর্ষ | |
| Device | ডিভাইস / কম্পিউটার | use **কম্পিউটার** for end users |
| LAN | স্থানীয় নেটওয়ার্ক | |
| Host | হোস্ট কম্পিউটার | |
| Client | ক্লায়েন্ট কম্পিউটার | |
| Activation | সক্রিয়করণ | |
| Verify | যাচাই করুন | |
| Install | ইনস্টল / স্থাপন | use **ইনস্টল** |
| Upgrade | আপগ্রেড | |
| About | সম্পর্কে | |
| Help | সহায়িকা | |

## Button/action wording (standard)
- **সংরক্ষণ করুন** (Save) · **বাতিল** (Cancel) · **যোগ করুন** (Add) ·
  **সম্পাদনা** (Edit) · **মুছুন** (Delete) · **প্রিন্ট** (Print) ·
  **এক্সপোর্ট** (Export) · **খুঁজুন** (Search) · **যাচাই করুন** (Verify) ·
  **সক্রিয় করুন** (Activate) · **পুনরুদ্ধার করুন** (Restore)

## Confirmation & error style
- Confirm destructive/financial actions explicitly, naming the record and amount
  in Bengali digits per the user's digit preference.
- Errors state what happened and the next step in plain Bengali. Never show raw
  stack traces or English exception text to ordinary users.

## Digit preference (PART 7.1)
Global setting: **Bengali digits ০১২৩৪৫৬৭৮৯** or **English digits 0123456789**.
Applied to IDs, amounts, dates, tables, receipts, reports, counts and dashboard
figures. Storage and math always use English-digit canonical values; only display
is localized. (Implementation: `docs/ARCHITECTURE.md` → Formatting layer.)

## Change log
- 2026-10-19 — Initial glossary created from master prompt + research. Pending
  validation with coaching-center staff (owner/user-testing task).

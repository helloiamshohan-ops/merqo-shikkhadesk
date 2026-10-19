# UX RESEARCH — MERQO ShikkhaDesk

**Research date:** 2026-10-19
Method: observation of **public** marketing/product pages and public demos only.
Patterns below are **[OBSERVED]** from public material; recommendations are
**[ANALYSIS]** for ShikkhaDesk. No proprietary UI was copied.

---

## Observed market UI patterns [OBSERVED]

1. **Web dashboards dominate.** Nearly all competitors present a browser
   dashboard (CampusQ, OurCampusBD, ShikkhaPlus, BIDYATek, Edufy, Digicampus,
   Eduman, Sabaaq). Left-nav + top-bar + cards is the common skeleton.
2. **Bengali-first marketing, mixed product language.** Bangla is used heavily in
   marketing (Sabaaq, OurCampusBD, Eduman); in-product language varies and often
   mixes English terms.
3. **Card-based dashboards.** Headline counts (students, collection, dues) shown
   as tiles.
4. **Table + filter + search** is the universal list pattern for students, fees,
   attendance.
5. **Receipt / marksheet / ID-card print previews** are shown as selling points
   (Sabaaq, GreenWeb, Digicampus).
6. **Role-based portals** (admin / teacher / parent) are common selling points
   (ShikkhaPlus, BIDYATek, Edufy).
7. **Onboarding is "2-minute setup / subdomain"** for cloud products (EduFlow,
   Sabaaq) — a contrast to a guided desktop first-run wizard.

## Observed usability themes in the market [OBSERVED]
- Vendors advertise **low training overhead** for "non-tech" staff (GreenWeb,
  Sabaaq, Zaman IT).
- **SMS/WhatsApp absence alerts** are marketed as a core daily-value feature.
- **Fee "chase" / defaulter lists** are a highlighted workflow (CoachDesk's
  "four-stage chase list"; Sabaaq's overdue lists).

---

## ShikkhaDesk UX principles [ANALYSIS]

Derived from the market observations and the product principles (simple Bangla,
low computer-literacy users, offline-first desktop).

1. **Desktop-native, not a website in a window.** Left sidebar + top bar + page
   title + content, per `UI-DESIGN-SPEC.md`. Feels like a real Windows business
   app, not a browser page.
2. **Simple Bangla everywhere.** Every menu, button, form label, validation,
   error, receipt and report uses natural Bengali from
   `BANGLA-TERMINOLOGY.md`. No stray technical English.
3. **Big, readable financial numbers.** Money is never tiny text; Bengali/English
   digit preference is global and consistent.
4. **Fast daily tasks first.** The screens a teacher/accountant touch daily
   (attendance entry, fee collection, marks entry) are optimized for keyboard and
   minimal clicks. **No speed target is claimed until measured** (per directive).
5. **Guided first run.** A 10-step Bangla setup wizard replaces the market's
   "2-minute cloud signup" — appropriate for an installed, offline product.
6. **Honest status surfaces.** Connection status, license status, backup status
   are always visible — because offline/LAN operation makes them operationally
   important (unlike cloud products that hide connectivity).
7. **Print is a first-class citizen.** Receipts, registers, marksheets and
   statements are designed for real thermal/inkjet printing in Bangladesh.
8. **No decorative clutter, no needless animation.** Accessible keyboard
   navigation and visible focus indicators throughout.

## Information architecture (top level) [ANALYSIS]
- প্রধান পাতা (Dashboard)
- শিক্ষার্থী (Students) · ভর্তি (Admissions)
- একাডেমিক (Academic: classes, courses, batches, subjects, sessions)
- উপস্থিতি (Attendance)
- পরীক্ষা ও ফলাফল (Exams & Results)
- ফি (Fees: plans, assessment, collection, dues, advance)
- হিসাব (Accounts: CoA, income, expense, journal, cash/bank, ledger, statements)
- বেতন (Payroll) · সরবরাহকারী (Suppliers/Payables)
- রিপোর্ট (Report center)
- ব্যবস্থাপনা (Administration: users, permissions, audit, backup, license, devices, settings)
- সম্পর্কে (About)

## Open UX questions for owner/user testing [ANALYSIS]
- Confirm Bengali terminology with real coaching-center staff (per directive:
  "Validate terminology with the intended audience").
- Confirm default digit preference (Bengali vs English) per institution size.
- Confirm receipt layout expectations (logo, fields, footer text).

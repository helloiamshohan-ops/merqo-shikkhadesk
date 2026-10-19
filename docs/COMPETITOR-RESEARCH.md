# COMPETITOR RESEARCH — MERQO ShikkhaDesk

**Research date:** 2026-10-19
**Method:** Public web search and public marketing/product pages only. No private
systems were accessed, no logins bypassed, no prohibited content scraped, and no
proprietary code, branding or confidential material was copied.
**Separation rule:** Sections marked **[OBSERVED]** are facts visible on public
pages (with source URL). Sections marked **[ANALYSIS]** are ShikkhaDesk's own
interpretation and are not vendor claims. Advertised features are **not**
independently verified unless stated.

---

## Market context [OBSERVED / ANALYSIS]

[OBSERVED] The Bangladesh coaching/education-management market is large and
growing. Bonik Barta (citing the Bangladesh Bureau of Statistics "Private
Educational Institutions Survey 2024") reports **6,587 registered coaching
centers** (6,312 academic, 275 university-admission/job-prep), with **917,000
students** and **66,754 staff**, and notes unregistered centers are "several
times higher." The Daily Star cites the coaching sector as a **BDT 50,000 crore**
business.
Sources: https://en.bonikbarta.com/bangladesh/ISo0VBvmo6FW7M0v ,
https://www.thedailystar.net/campus/news/the-experience-starting-your-own-coaching-classes-university-student-3500051

[ANALYSIS] A large, fragmented, mostly-underserved market with recurring revenue
potential — but dominated by low-digital-literacy operators, which validates a
simple-Bengali, offline-first, low-training-overhead product.

---

## Competitor profiles

### 1. CampusQ
- **Public URL:** https://campusqbd.com (signin page)
- **Research date:** 2026-10-19
- **[OBSERVED]** "Multi-tenant SaaS coaching center management system for
  Bangladesh. Manage students, attendance, exams, and payments in one place.
  Supports bKash & Nagad." Located in Dhaka. Has Features / Pricing / Blog pages.
- **Deployment:** Cloud multi-tenant SaaS.
- **Pricing:** Pricing page exists; exact figures not captured on the signin page.
- **Strengths (evidenced):** Coaching-specific positioning; local payment methods.
- **Unknowns:** Accounting depth; offline capability; data-ownership terms.
- **[ANALYSIS] Opportunity:** No evidence of full double-entry accounting or
  offline-first operation.

### 2. OurCampusBD
- **Public URL:** https://www.ourcampusbd.com/
- **Research date:** 2026-10-19
- **[OBSERVED]** Bengali-first management software for schools, madrasas,
  academies and coaching centers: admission, student info, attendance, salary &
  fee collection via bKash/Nagad, exams & results, guardian SMS — "one dashboard.
  From ৳500/month."
- **Deployment:** Cloud (implied monthly subscription).
- **Pricing:** From ৳500/month [OBSERVED].
- **Strengths:** Full Bengali; broad module coverage; local payments.
- **Unknowns:** Offline mode; double-entry accounting; data ownership.
- **[ANALYSIS] Opportunity:** Subscription + cloud; not positioned around data
  ownership or offline use.

### 3. CoachDesk / CoachDesk BD (Shikhon ecosystem)
- **Public URL:** https://sikhon.systems/en (product "CoachDesk কোচডেস্ক");
  open-source reference: https://github.com/auntor69/coachdesk-bd
- **Research date:** 2026-10-19
- **[OBSERVED]** "A Bangla-first workspace for coaching centres in Bangladesh to
  collect fees, track who owes what, take attendance and message guardians by
  SMS. It is in beta." The GitHub project describes: admissions & roster with bulk
  import, batches, attendance, exams & marks, a fee board with invoices,
  installments, scholarships and a "four-stage chase list", payroll, expenses,
  notices, weekly progress reports, multi-branch HQ, full audit log — Bengali and
  English, **offline-first**, **"money is integer poisha, and no float touches a
  balance"**, prepaid per-day balance model.
- **Deployment:** Offline-first (mobile-first); prepaid balance gating.
- **Pricing:** Beta; prepaid per-open-day model described in the repo.
- **Strengths (evidenced):** Closest to ShikkhaDesk's philosophy — Bangla-first,
  offline-first, integer money, dual-ledger fees, audit log.
- **Unknowns:** Maturity (beta); double-entry accounting depth; Windows desktop
  UX; license model.
- **[ANALYSIS] Opportunity:** The single closest competitor on *philosophy*.
  ShikkhaDesk differentiates with a **Windows desktop** product, **full
  double-entry accounting**, and a **perpetual-license** (not prepaid-balance)
  model. The shared "…Desk" naming is also a confusion risk (see DEC-1).

### 4. ShikkhaPlus (Ambala IT)
- **Public URL:** https://www.shikkhaplus.com/ , https://ambalait.com/blogs/shikkhaplus-...
- **Research date:** 2026-10-19
- **[OBSERVED]** "Complete Educational Management Software (EMS)… for schools,
  colleges, universities, and madrasas… also coaching and training centers."
  Cloud-based. Feature list (schema.org): online admission, student info,
  attendance (biometric support), fees, exams, results, student/parent/teacher
  portals, routine, SMS, mobile app, reports, role-based portals, cloud access.
- **Deployment:** Cloud SaaS + mobile apps.
- **Pricing:** Not captured.
- **Strengths:** Broad, mature feature set; portals; biometrics.
- **Unknowns:** Accounting depth; offline mode; data ownership.
- **[ANALYSIS] Opportunity:** Name collision with "ShikkhaDesk" is a direct
  market-confusion risk (DEC-1). Cloud/subscription model; no evidence of
  offline-first or true double-entry.

### 5. Shikkha (AcquaintBD)
- **Public URL:** https://acquaintbd.com/best-school-management-software-in-bangladesh/
- **Research date:** 2026-10-19
- **[OBSERVED]** "'Shikkha' — A complete Education Management system… providing
  school management software… since 2018… for schools of all sizes."
- **Deployment / Pricing:** Not captured.
- **[ANALYSIS] Opportunity:** Another "Shikkha"-named incumbent; reinforces the
  naming-confusion finding (DEC-1).

### 6. BIDYATek (Pepoltek Limited)
- **Public URL:** https://bidyatek.com/ , https://www.pepoltek.com/solutions/bidyatek/
- **Research date:** 2026-10-19
- **[OBSERVED]** "Comprehensive School Management System… SaaS Product… Cloud
  Native." Unifies students, teachers, fees, exams, parents. Modules: Academics
  (class/subject allocation, exam scheduling, automated grading), Finance (fee
  collection, **expense tracking, payroll processing**, "comprehensive
  accounting"), Communication (SMS/email/push). "Trusted by 100+ institutions."
- **Deployment:** Cloud SaaS.
- **Pricing:** Not captured.
- **Strengths:** Finance module incl. payroll; portals; scale claims.
- **Unknowns:** Whether "comprehensive accounting" is true double-entry; offline
  mode; data ownership.
- **[ANALYSIS] Opportunity:** Closest on *breadth* of finance claims; still
  cloud/subscription. Double-entry rigor unverified.

### 7. Edufy (SoftifyBD Limited)
- **Public URL:** https://edufy.com.bd/ (pricing, about, features)
- **Research date:** 2026-10-19
- **[OBSERVED]** Education management for schools/colleges/madrasas/kindergartens.
  "Over 600 institutions." Has an **Accounting Management** module. Pricing
  "starting at 7 TK/student/month," monthly, no upfront fees. Free school website
  on Premium. Bilingual.
- **Deployment:** Cloud SaaS.
- **Pricing:** From ৳7/student/month [OBSERVED].
- **Strengths:** Scale (600+), accounting module, affordability.
- **Unknowns:** Double-entry depth; offline; data ownership.
- **[ANALYSIS] Opportunity:** Per-student recurring pricing; cloud. Data lives
  with vendor.

### 8. Sabaaq
- **Public URL:** https://sabaaq.com/school-management-software-bangladesh/
- **Research date:** 2026-10-19
- **[OBSERVED]** Full Bengali school & coaching management: admission, attendance
  (manual or ZKTeco fingerprint/face machine; SMS on absence), salary & fees,
  exams & results (marksheet, GPA, merit list), income-expense by head, guardian
  & teacher app, ID cards/certificates/receipts, roles & shifts, multi-branch.
  Pricing: **monthly ৳500 / yearly ৳5000 / one-time ৳20,000**; installation fee
  ৳2,000 (<200 students) or ৳5,000. SMS and biometric are optional add-ons.
- **Deployment:** Cloud (subdomain per institution); optional hardware.
- **Pricing:** ৳500/mo, ৳5000/yr, or ৳20,000 one-time [OBSERVED].
- **Strengths:** Broad coverage; explicit one-time option; optional hardware.
- **Unknowns:** Offline mode; double-entry; data ownership.
- **[ANALYSIS] Opportunity:** A one-time ৳20,000 option exists but the product is
  still cloud/subscription-centric.

### 9. Digicampus BD
- **Public URL:** https://digicampusbd.com/
- **Research date:** 2026-10-19
- **[OBSERVED]** Per-student monthly plans (Advanced ৳10/mo, others ৳12/৳14/mo):
  students, teachers/staff, attendance (biometric device integrated), fees,
  **accounts management**, **online fees collection (bKash, Rocket, Nagad,
  Cellfin SSL)**, results, routine, SMS, holiday, user management.
- **Deployment:** Cloud + biometric hardware + payment gateway.
- **Pricing:** Per-student monthly [OBSERVED].
- **[ANALYSIS] Opportunity:** Recurring per-student; cloud; hardware/gateway
  dependencies.

### 10. Eduman (EdumanBD)
- **Public URL:** https://edumanbd.com/features/
- **Research date:** 2026-10-19
- **[OBSERVED]** "5,000+ schools, colleges and madrasas," 10+ years. Digital
  attendance, fee collection, SMS notices, result processing & publishing,
  dynamic routine, student/teacher profiles, "সহজ একাউন্টিং" (simple accounting),
  tutorials, community support.
- **Deployment:** Not captured (likely cloud/SaaS).
- **[ANALYSIS] Opportunity:** Large installed base; "simple accounting" (not
  double-entry). Offline/data-ownership not evidenced.

### 11. Green Software & Technology (coaching CMS)
- **Public URL:** https://greensoftwarebd.com/product-detail/12
- **Research date:** 2026-10-19
- **[OBSERVED]** Coaching-center CMS: dashboard, class/schedule/batch/subject/
  exam-type/grade setup, student & teacher management with ID/admit cards,
  teacher-subject assignment, results & progress reports, SMS, fees collection
  with fee heads and class-wise fee setting, collection/update reports, expense
  entry with heads, **balance sheet**. Pricing: **development ৳15,000 +
  ৳1,250/month service.**
- **Deployment:** Custom-developed (per-customer build).
- **[ANALYSIS] Opportunity:** "Balance sheet" is single, not double-entry;
  per-customer custom dev + monthly service.

### 12. Zaman IT (school management packages)
- **Public URL:** https://zaman-it.com/school-management-system/
- **Research date:** 2026-10-19
- **[OBSERVED]** Tiered packages: Basic ৳50,000 / Professional ৳100,000 /
  Advanced ৳150,000. Each tier lists the same feature categories (student,
  academic, staff/HR, finance & fees, library, operations, communication,
  analytics).
- **Deployment:** One-time license, tiers.
- **[ANALYSIS] Opportunity:** One-time pricing precedent in-market (৳50k–৳150k),
  useful for ShikkhaDesk pricing context. Feature parity across tiers is claimed
  (unverified).

### 13. schoolsoftware-bd.com
- **Public URL:** https://schoolsoftware-bd.com/
- **Research date:** 2026-10-19
- **[OBSERVED]** "Bangladesh's #1 School Management Software," since 2014,
  "200+ institutes, 125+ schools, 45+ coaching centers, 12+ colleges," mobile app
  included, all-in-one ERP.
- **[ANALYSIS] Opportunity:** Established brand; ERP breadth; mobile-first.

### 14. Other named leads (checked)
- **iSkool / iSkooll** — https://www.iskool.in/ — school management (India-facing);
  not a Bangladesh coaching-specific desktop product. [OBSERVED]
- **Rofasware** — https://www.rofasware.com/ — **a software design/development
  studio in Dhaka (House 17, Road 4, Dhanmondi), NOT an education-management
  product.** Listed as a research lead; recorded here to correct that assumption.
  [OBSERVED]
- **Edumi** — no distinct, verifiable Bangladesh coaching product page found in
  public search on the research date. [ANALYSIS: treat as unconfirmed; do not
  cite as a verified competitor.]
- **Effy Edu** — no distinct, verifiable product page found on the research date.
  [ANALYSIS: unconfirmed.]
- **RAJDESK** — no distinct education product found; "RajTech" is a web-design
  company. [ANALYSIS: unconfirmed / likely not a coaching product.]
- **Tutorey** — no verifiable product page found on the research date.
  [ANALYSIS: unconfirmed.]

### 15. Additional in-market players [OBSERVED]
ShikshaSuite (shikshasuite.com), Sikhsha (sikhsha.in), ShikshaCoach
(shikshacoach.com), Shikkha IT Limited (shikkha.dev), Bidyaan (bidyaan.com),
Biddaloy (biddaloy.app), Biddaniketon (biddaniketon.com), Witty EMS
(wittyems.com), School360 (school360.com.bd), Pixelvate (edu.pixelvate.com),
eEdu.bd (eedu.bd), EduBari (edubari.bd), EduFlow (eduflowbd.com), Digital Crop,
GreenWeb, Biz IT BD, Dianahost. These reinforce a **crowded, mostly cloud/SaaS**
  market with many "Shiksha/Shikkha"-derived names.

---

## Naming-conflict finding (feeds DEC-1) [ANALYSIS on OBSERVED names]

Public pages evidence several similarly-named products: **ShikkhaPlus**,
**Shikkha** (AcquaintBD), **ShikshaSuite**, **Sikhsha**, **ShikshaCoach**,
**Shikkha IT Limited**, and **CoachDesk** (Bangla-first coaching). The proposed
name **"ShikkhaDesk"** therefore sits in a crowded "Shikkha/Shiksha" + "…Desk"
space. This is a **material market-confusion risk**, exactly as the master prompt
anticipated. **Decision required from the owner (DEC-1); the product is NOT
silently renamed.** Recommended mitigation: always present the full brand
**"MERQO ShikkhaDesk"** and commission a formal trademark search before paid
distribution.

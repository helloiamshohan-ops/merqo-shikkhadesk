# INSTALLATION GUIDE — MERQO ShikkhaDesk

PART 28. **Honest status: the Windows installer has NOT been built yet** (it
requires Windows + the installer toolchain, which this Linux environment cannot
provide — see `KNOWN-ISSUES.md` BLOCKER-1). The expected artifacts are named
below; **they do not exist until actually built and verified.** This guide
describes the intended, tested-when-available installation experience.

---

## 1. Expected artifacts (naming conventions — must be actually built)
- `ShikkhaDesk-Setup-v1.0.0-x64.exe` — customer installer
- `ShikkhaDesk-Release-v1.0.0-x64.zip` — portable release (optional)
- `ShikkhaDesk-LicenseManager-Setup-v1.0.0-x64.exe` — **seller-only** License
  Manager installer (never shipped to customers)

## 2. System requirements (intended)
- Windows 10 or Windows 11, 64-bit (x64).
- ~500 MB free disk (app + .NET runtime if not self-contained).
- For LAN: computers on the same local network.
- .NET Desktop Runtime: either bundled (self-contained) or installed as a
  prerequisite (to be finalized at build time and documented).

## 3. What the installer does
- Shows the **MERQO** brand and **ShikkhaDesk** product name with the genuine
  icon and version info.
- Installs application files; creates Start Menu and Desktop shortcuts.
- Installs the bundled Bengali font (Noto Sans Bengali) if not present.
- Supports **upgrade** over a prior version and **safe uninstall**.
- **Preserves customer data** (database and backups are not removed on uninstall).

## 4. First launch (guided)
1. Launch ShikkhaDesk → **first-run wizard** (Bangla): Welcome → institution →
   academic → locale/digits → administrator → receipts → backup → host/client →
   license activation → review/finish.
2. Set a strong administrator password (**no default password**).
3. **Activate the license** using the file from MERQO (offline verification).
4. Begin daily use.

## 5. License activation
- Obtain the license file from MERQO (merqoonline@gmail.com).
- Activation screen: select file / enter code → **যাচাই করুন** (Verify) →
   **সফটওয়্যার চালু করুন** (Start). Works offline. See `LICENSE-DESIGN.md`.

## 6. Upgrade
- Run the newer `ShikkhaDesk-Setup-vX.Y.Z-x64.exe`; it upgrades in place, runs
   schema migrations after an automatic pre-upgrade backup, and preserves data.
- Failed-upgrade recovery: the pre-upgrade backup + previous version allow
   rollback (to be verified on Windows).

## 7. Uninstall / reinstall
- Uninstall removes program files but **keeps** the database and backups.
- Reinstall + restore returns the institution to its data.

## 8. Code signing & SmartScreen (honest)
- **The app is not claimed to be digitally signed** unless a real valid
  Authenticode signature exists. Until then, Windows SmartScreen may show a
  warning on first run; this is disclosed, not hidden. See `RELEASE-CHECKLIST.md`.

## 9. Verification still required before release (PART 27/28)
Clean install · first launch · license activation · upgrade · failed-upgrade
recovery · backup & restore · uninstall · reinstall · customer-data preservation.
All are **OPEN** until run on a clean supported Windows environment.

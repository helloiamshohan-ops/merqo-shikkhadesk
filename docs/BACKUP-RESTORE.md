# BACKUP & RESTORE — MERQO ShikkhaDesk

PART 24. Reliable local backup and recovery. The institution owns its data; the
app never transmits customer data to MERQO and never deletes/encrypts data because
a license is invalid.

**Status: design + scaffold (`ShikkhaDesk.Backup`). Not built or tested on Windows
(restore verification is OPEN in `RELEASE-CHECKLIST.md`).**

---

## 1. Principles
- The authoritative store is a single SQLite database file on the host.
- Backups are **consistent snapshots** (SQLite online backup / VACUUM INTO, or a
  safe file copy after a checkpoint) so a backup is never a torn/partial DB.
- **Never overwrite the only valid backup.** Each backup is a new timestamped file;
- retention keeps the most recent N and prunes older ones only after a newer good
  backup exists.
- Every backup records a **checksum** and an **integrity check** result.

## 2. Backup types
- **Automatic:** on a schedule (e.g. daily at close) and before risky operations
  (restore, migration, upgrade). Configurable directory.
- **Manual:** on demand from the Backup screen.
- **Pre-restore:** automatically taken immediately before any restore.
- **Pre-migration/upgrade:** taken before schema migrations.

## 3. Backup settings
- Directory (default: a `ShikkhaDesk-Backups` folder; configurable, ideally on a
  different physical drive or external media).
- Retention count and/or age.
- Automatic schedule and whether to alert on failure.

## 4. Integrity & alerts
- After each backup: verify the snapshot opens and `PRAGMA integrity_check` passes;
  record checksum. On failure, raise a **failure alert** and keep the previous good
  backup.

## 5. Restore
1. Choose a backup file (list shows date, type, size, integrity).
2. **Confirm** — restoring replaces current data.
3. The app takes a **pre-restore backup** of the current DB first.
4. Restore the snapshot atomically; verify integrity; restart into the restored
   state.
5. If restore fails, the pre-restore backup allows returning to the prior state.

## 6. Recovery guide (for the owner)
- **Routine recovery:** Backup screen → Restore → pick latest good backup.
- **New computer / host migration:** install ShikkhaDesk, activate license, then
   restore the latest backup; set host/client mode; point clients to the new host.
- **Disk failure:** restore from the backup directory on external media.
- **Corrupted DB suspected:** do not keep writing; restore the last integrity-OK
   backup.
- Keep backups **off the same disk** where possible; test a restore periodically.

## 7. Data ownership & uninstall
- Customer data belongs to the institution.
- **Uninstall preserves customer data by default** (the DB and backups are not
   removed). An explicit, documented, authorized **Remove data** action exists for
   when the owner intends deletion.
- No silent transmission of customer data to MERQO at any time.

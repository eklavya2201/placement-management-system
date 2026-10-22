# College Placement Management System

A web app for a college placement cell to manage **students**, **recruiting companies**, **job drives** and **applications**.
Built with **ASP.NET Core MVC (.NET 8)** and **MySQL**.

- Add students (branch, batch, CGPA, email) and companies
- Post job drives with role, package (LPA), minimum CGPA and date
- Apply a student to a drive. **The app checks CGPA eligibility automatically** and blocks duplicate applications
- Track each application: Applied → Shortlisted → Selected / Rejected
- Dashboard with live counts, plus a read-only **Database** page that shows the raw MySQL tables

---

## 1. Just want to see it?

Open the live website in any browser. Nothing to install:

**Live link:** _(ask Eklavya for the Render link)_

> The free server sleeps when idle. The first load can take about a minute, so just wait and refresh.

---

## 2. Run it on your own PC

### What you need
| Tool | Check / Get it |
|---|---|
| .NET 8 SDK | `dotnet --version` should show `8.x`. Get it at https://dotnet.microsoft.com/download |
| Visual Studio 2022 with the **"ASP.NET and web development"** workload | Visual Studio Installer → Modify. (VS Code + `dotnet run` also works.) |
| A MySQL database | **Option A:** the team's shared cloud database (nothing to install)<br>**Option B:** your own MySQL Server + MySQL Workbench |

### Step 1: Unzip
Unzip to a **short path**, e.g. `C:\Projects\PlacementManagementSystem`. Very long folder paths can break the NuGet restore.

### Step 2: Choose a database

**Option A: shared cloud database (easiest, same data as the live site)**

Ask Eklavya for the 4 connection values: **host, port, user, password**. They are not in this zip on purpose.
Then open `Core/Data/DbConfig.cs` and replace the fallback values:

```csharp
public static string Server   { get; set; } = Env("DB_HOST", "PASTE-HOST-HERE");
public static string Database { get; set; } = Env("DB_NAME", "placement_db");
public static string User     { get; set; } = Env("DB_USER", "PASTE-USER-HERE");
public static string Password { get; set; } = Env("DB_PASSWORD", "PASTE-PASSWORD-HERE");
public static uint   Port     { get; set; } = uint.Parse(Env("DB_PORT", "PASTE-PORT-HERE"));
```

> Any change you make in the app (adding a student, etc.) goes into the **shared** database that everyone sees.

**Option B: your own local MySQL**

1. Open MySQL Workbench and connect to your local server.
2. File → Open SQL Script → `schema.sql` → run it (⚡). This creates the `placement_db` database and its 4 tables.
3. *(Optional demo data)* Open and run `seed.sql`. It adds 12 students, 6 companies, 6 drives and 13 applications.
   **Warning:** it deletes whatever is already in those tables first.
4. In `Core/Data/DbConfig.cs`, change the password fallback `"your_password"` to your MySQL root password.
   Change `"root"` too if you use a different user.

### Step 3: Run
- **Visual Studio:** double-click `PlacementManagementSystem.sln`, wait for the package restore (needs internet the first time), then press **F5**.
  The browser opens at `http://localhost:5145`.
- **Terminal:** from the project folder run `dotnet run --project Web`, then open `http://localhost:5145`.

If F5 is greyed out: right-click **PlacementManagementSystem.Web** → *Set as Startup Project*.

---

## 3. Seeing the SQL database

**In the browser:** click **Database** in the sidebar. It shows the MySQL version and every row of
`students`, `companies`, `job_drives` and `applications` exactly as stored. It is read-only.

**In MySQL Workbench:**
- *Local (Option B):* connect to `localhost` and look at the `placement_db` schema.
- *Shared cloud (Option A):* click **+** next to *MySQL Connections* and enter:
  - Hostname / Port / Username: the values from Eklavya
  - Password → *Store in Vault…* → paste the password
  - **SSL tab → Use SSL: `Require`**
  
  Connect, then open `placement_db` → Tables. Right-click a table → *Select Rows* to see its data.

Example queries:
```sql
USE placement_db;

-- Who got selected, where, and for how much
SELECT s.name, c.name AS company, j.role, j.package_lpa
FROM applications a
JOIN students s   ON s.id = a.student_id
JOIN job_drives j ON j.id = a.job_drive_id
JOIN companies c  ON c.id = j.company_id
WHERE a.status = 'Selected';

-- Students eligible for each drive
SELECT j.role, c.name AS company, s.name, s.cgpa
FROM job_drives j
JOIN companies c ON c.id = j.company_id
JOIN students s  ON s.cgpa >= j.min_cgpa
ORDER BY j.id, s.cgpa DESC;
```

---

## 4. Database design

```
students (id PK, name, branch, graduation_year, cgpa, email)
companies (id PK, name, sector, contact_email)
job_drives (id PK, company_id FK → companies, role, package_lpa, min_cgpa, drive_date)
applications (id PK, student_id FK → students, job_drive_id FK → job_drives, status)
             UNIQUE (student_id, job_drive_id)   -- a student can apply to a drive only once
```
All foreign keys are `ON DELETE CASCADE`. `status` is one of `Applied`, `Shortlisted`, `Selected`, `Rejected`.

---

## 5. Project structure

```
PlacementManagementSystem.sln   ← open this in Visual Studio
schema.sql                      ← creates the database + tables
seed.sql                        ← optional demo data
Core/                           ← class library
  Models/                       ← Student, Company, JobDrive, Application
  Data/                         ← DbConfig (connection) + one repository per table (plain SQL via MySqlConnector)
Web/                            ← the website (startup project)
  Controllers/                  ← Home, Students, Companies, JobDrives, Applications, Database
  Views/                        ← Razor pages
  wwwroot/css/site.css          ← all styling
Dockerfile, render.yaml         ← used to deploy the site on Render
```

The eligibility rule lives in `Web/Controllers/ApplicationsController.cs` → `Apply(...)`.
All SQL is parameterised, so it is safe against SQL injection.

---

## 6. Troubleshooting

| Problem | Fix |
|---|---|
| `Access denied for user ...` | Wrong user or password in `Core/Data/DbConfig.cs`. |
| `Unable to connect to any of the specified MySQL hosts` | MySQL isn't running (local), or the host/port is wrong (cloud). On a college network, port 3306 or the cloud port may be blocked; try a phone hotspot. |
| `Unknown database 'placement_db'` | Run `schema.sql` first. |
| Project won't build / "unavailable" in VS | Install the .NET 8 SDK and the *ASP.NET and web development* workload. |
| Package restore fails | Needs internet the first time (it downloads `MySqlConnector`). |
| `address already in use` | Change the port `5145` in `Web/Properties/launchSettings.json`. |
| HTTPS certificate warning | Pick the **http** profile in VS's Run dropdown, or run `dotnet dev-certs https --trust`. |

---

## Deploying (for reference)
The live site runs on **Render** (Docker, free plan), and the database is a free **Aiven MySQL**.
The app reads `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER` and `DB_PASSWORD` from environment variables, which are set in Render.
If they are not set, it uses the fallback values in `DbConfig.cs`.

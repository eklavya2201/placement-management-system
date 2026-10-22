College Placement Management System (ASP.NET Core MVC + MySQL)
====================================================================

WHAT THIS IS
A browser-based (localhost) web app for managing students, companies,
job drives, and applications, backed by a MySQL database. Open in
Visual Studio, press Run, and it opens in your browser automatically.

PROJECT LAYOUT
  PlacementManagementSystem.sln   - open THIS file in Visual Studio
  Core/                           - shared library: Models + MySQL data access
  Web/                             - the ASP.NET Core MVC website (the startup project)
  schema.sql                       - run once in MySQL Workbench to create the DB
  Core/Data/DbConfig.cs            - MySQL connection settings (EDIT THIS on each new PC)


REQUIREMENTS ON THE COLLEGE PC
  - .NET 8 SDK (check with: dotnet --version -- must show 8.x or higher)
  - Visual Studio 2022 Community (or newer) with the
    "ASP.NET and web development" workload installed
  - MySQL Server + MySQL Workbench


SETUP STEPS (do this every time you move to a new/different PC)

1. Unzip this whole folder somewhere with a SHORT path, e.g.
       C:\Projects\PlacementManagementSystem
   Avoid deeply nested folders (Desktop\College\Sem6\Project\Final\...) --
   Windows has a path length limit and NuGet restore can fail on very
   long paths.

2. Open MySQL Workbench, connect to the local MySQL server, open
   schema.sql as a new SQL script, and run it (the lightning bolt /
   Execute button). This creates the `placement_db` database and all
   4 tables (students, companies, job_drives, applications).

3. Open Core\Data\DbConfig.cs (in Visual Studio's Solution Explorer,
   under the Core project) and edit these 4 lines to match whatever
   MySQL login the college PC actually has:
       Server   = "localhost"
       Database = "placement_db"
       User     = "root"          <-- change if needed
       Password = "your_password" <-- change to the real password
       Port     = 3306

4. Double-click PlacementManagementSystem.sln to open the whole
   solution in Visual Studio.

5. Wait for NuGet to finish restoring packages (bottom status bar).
   This needs internet the first time on a new PC, to download the
   MySqlConnector package used to talk to MySQL.

6. Press the green Run button / F5 (top toolbar should show
   "PlacementManagementSystem.Web" or "http" as the start item).
   Visual Studio will build the app, start it, and open your browser
   to something like http://localhost:5145 automatically.


USING THE APP
Navigate using the top menu:
  Students     - add/view students (name, branch, year, CGPA, email)
  Companies    - add/view recruiting companies
  Job Drives   - post a company's hiring drive (role, package, min CGPA)
  Applications - apply a student to a drive (CGPA eligibility is
                 checked automatically) and update status
                 (Applied / Shortlisted / Selected / Rejected)

All data lives in the MySQL `placement_db` database, so you can also
browse/query it directly in MySQL Workbench at any time.


POSSIBLE ISSUES ON A DIFFERENT (COLLEGE) PC AND HOW TO FIX THEM
====================================================================

1. "Startup project" is wrong / Run button greyed out
   Visual Studio should auto-pick the Web project as startup since
   Core is a library and can't run on its own. If it doesn't:
   right-click the "PlacementManagementSystem.Web" project in Solution
   Explorer -> "Set as Startup Project".

2. .NET SDK missing or too old
   This targets .NET 8. If the college PC only has .NET 6/7 installed,
   the project won't build. Check with `dotnet --version` in a terminal,
   or install the .NET 8 SDK from https://dotnet.microsoft.com/download
   (needs admin rights / internet -- ask a lab admin if you can't install).

3. Visual Studio missing the web workload
   If VS says it can't load the Web project or shows it as
   "unavailable", the "ASP.NET and web development" workload isn't
   installed. Open Visual Studio Installer -> Modify -> check that
   workload -> Modify/Install. Requires internet and possibly admin
   rights on a lab PC.

4. MySQL Server not installed, not running, or on a different port
   The app assumes MySQL is on localhost:3306. If MySQL Workbench
   can't connect either, MySQL Server may not be installed/running on
   that PC at all -- you (or the lab) would need to install it, or use
   a MySQL server elsewhere on the network and change the Server/Port
   in DbConfig.cs accordingly.

5. MySQL login has no permission to CREATE DATABASE
   College lab MySQL accounts are sometimes restricted student logins
   without admin rights, so running schema.sql (which does
   CREATE DATABASE placement_db) may fail with an "Access denied" error.
   If so: ask whoever manages the lab machines to either run schema.sql
   as root once, or grant your account CREATE privileges. As a
   workaround, if `placement_db` already exists on that server, you can
   delete the "CREATE DATABASE" / "USE placement_db;" lines from
   schema.sql and just run the CREATE TABLE statements inside whatever
   database you've been given access to (then update
   DbConfig.Database to match).

6. Wrong username/password in DbConfig.cs
   This is the #1 likely failure. Every PC's MySQL root password is
   different (or the lab may give you a specific username instead of
   root) -- the app will show "Could not connect to MySQL... Access
   denied" if these don't match. Always re-check step 3 above on a new
   machine.

7. Lab PCs that reset on reboot / logout
   Some college labs restore the machine to a clean disk image after
   every reboot or logout (common in shared computer labs). If that's
   the case here, anything you add to the MySQL database during one
   session (students, companies, applications) may be WIPED the next
   time you log in, because MySQL's data files get reset along with
   everything else. If your demo needs to persist across sessions,
   check with the lab about this beforehand, or re-run schema.sql and
   re-enter demo data right before you present.

8. Port 5145 (or whichever port Visual Studio picks) already in use
   Rare, but if something else on the lab PC is already using that
   port, the app will fail to start with an "address already in use"
   error. Fix: open Web\Properties\launchSettings.json and change the
   port number in "applicationUrl" to something else, e.g. 5200.

9. HTTPS certificate warning on first run
   If Visual Studio launches the "https" profile, the browser may show
   an "untrusted certificate" / "not private" warning the first time,
   because the ASP.NET Core dev HTTPS certificate isn't trusted on
   that machine yet. Either click through the browser's "proceed
   anyway" link, or run `dotnet dev-certs https --trust` once from a
   terminal in the Web project folder (may need admin rights), or
   simply select the "http" launch profile instead of "https" in
   Visual Studio's Run dropdown to avoid HTTPS entirely.

10. NuGet restore needs internet on first open
    The first time this solution is opened on a new PC, Visual Studio
    downloads the MySqlConnector package from nuget.org. If the lab
    network blocks external sites (some do, for security), restore
    will fail/hang. Test opening the solution with internet access
    at least once before you need to demo it; after that first
    successful restore the packages are cached locally and it won't
    need internet again on that same PC.

11. Antivirus / lab security software blocking the app
    Some locked-down lab PCs block unsigned .exe files from binding to
    network ports. If Run fails with a permissions-type error, try
    running Visual Studio "as Administrator", or ask lab staff if
    there's a security policy blocking it.


DEMO DATA
====================================================================
seed.sql fills the database with realistic demo data (12 students,
6 companies, 6 drives, 13 applications). Run it AFTER schema.sql.
WARNING: it deletes everything already in the 4 tables first.


ONLINE VERSION (Render + cloud MySQL)
====================================================================
The app reads its database login from environment variables, falling
back to the values in Core/Data/DbConfig.cs when they are not set:
    DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_PASSWORD

- Database: a free MySQL on Aiven (aiven.io). Run schema.sql and
  seed.sql on it once (Workbench can connect to it like any MySQL).
- Website: Render builds the Dockerfile in this folder. Set the five
  DB_* variables in the Render service's Environment tab.
- Render's free plan sleeps after ~15 minutes idle; the first visit
  after that takes about a minute. Open the link a few minutes before
  presenting.

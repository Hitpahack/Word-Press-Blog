<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>README - WordPress Clone in .NET Core</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
        h1, h2, h3 { color: #2c3e50; }
        .container { width: 80%; margin: auto; }
        .tech-stack img { width: 50px; margin-right: 10px; }
        .emoji { font-size: 1.2em; }
        .code { background: #f4f4f4; padding: 10px; border-radius: 5px; }
    </style>
</head>
<body>
    <div class="container">
        <h1 align="center">🚀 WordPress Clone - .NET Core Web API</h1>
        <p align="center">A powerful and secure blogging system inspired by WordPress, built with .NET Core.</p>

        <h2>📌 Features</h2>
        <ul>
            <li>🔑 <strong>User Authentication</strong> (Signup, Login, JWT Authentication, Password Reset)</li>
            <li>📝 <strong>Post Management</strong> (CRUD Operations)</li>
            <li>💬 <strong>Comments Module</strong> (CRUD, Reply to Comments, Approval System)</li>
            <li>🛡️ <strong>Security Features</strong>:
                <ul>
                    <li>🔒 Secure Password Hashing (Modified MD5 with Salting & Stretching)</li>
                    <li>🚫 IDOR Prevention (Insecure Direct Object References)</li>
                    <li>🛠️ JWT Authentication & Session Management</li>
                    <li>⏳ Login Attempt Limit with IP Tracking</li>
                </ul>
            </li>
        </ul>

        <h2>🛠️ Tech Stack</h2>
        <p class="tech-stack">
            <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="C#">
            <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dot-net/dot-net-original-wordmark.svg" alt=".NET Core">
            <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="SQL Server">
            <img src="https://www.vectorlogo.zone/logos/mongodb/mongodb-icon.svg" alt="MongoDB">
            <img src="https://www.vectorlogo.zone/logos/getpostman/getpostman-icon.svg" alt="Postman">
            <img src="https://www.vectorlogo.zone/logos/git-scm/git-scm-icon.svg" alt="Git">
        </p>

        <h2>📥 Installation</h2>
        <h3>🔧 Prerequisites</h3>
        <ul>
            <li>.NET SDK (Latest Version)</li>
            <li>MongoDB or MS SQL Server</li>
            <li>Visual Studio (or VS Code with C# Extension)</li>
        </ul>
        <h3>📌 Steps</h3>
        <pre class="code">
1. Clone the repository:
   git clone https://github.com/your-repo/wordpress-clone.git

2. Navigate to the project folder:
   cd wordpress-clone

3. Install dependencies:
   dotnet restore

4. Set up the database (MongoDB or SQL Server)

5. Run the API:
   dotnet run

6. API should be running at: http://localhost:5000/
        </pre>

        <h2>🔐 Security & Authentication</h2>
        <ul>
            <li>🔑 <strong>JWT Tokens</strong>: Used for user authentication and session management.</li>
            <li>🔒 <strong>Secure Password Hashing</strong>: Implemented using Modified MD5 with Salting & Stretching.</li>
            <li>⏳ <strong>Rate Limiting</strong>: Max 3 login attempts before temporarily blocking IP.</li>
        </ul>

        <h2>🤝 Contributing</h2>
        <p>Want to contribute? Follow these steps:</p>
        <ol>
            <li>🔄 Fork the repository.</li>
            <li>🌿 Create a new feature branch.</li>
            <li>💾 Commit your changes.</li>
            <li>🚀 Push to your fork and submit a PR.</li>
        </ol>

        <h2>📧 Contact</h2>
        <p>Email: your.email@example.com</p>
        <p>LinkedIn: <a href="https://linkedin.com/in/yourprofile">Your LinkedIn</a></p>
    </div>
</body>
</html>

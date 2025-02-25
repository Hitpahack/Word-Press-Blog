<div align="center">
  <h1 style="display: inline;">WordPress Clone – Blog Management System</h1>
  <h3>A powerful and secure WordPress-like blog system built using .NET Core Web API</h3>
</div>


<p align="left"> <img src="https://komarev.com/ghpvc/?username=yourusername&label=Profile%20views&color=0e75b6&style=flat" alt="yourusername" /> </p>

## 🚀 Project Overview  
<img align="right" width="35%" alt="WordPress Clone API" width="300" src="https://gteches.com/wp-content/uploads/2024/01/wordpress.gif" alt="yourusernamef"/> 

This project is a **.NET Core Web API** designed to manage a **WordPress-like blog system**. It includes essential modules for **user authentication, posts, and comments**, along with robust security implementations to safeguard user data and permissions.

### **Key Features**  
✅ **User Authentication** – Signup, Login, JWT Authentication, and Password Reset  
✅ **Posts Management** – CRUD operations for blog posts  
✅ **Comments Module** – CRUD operations, reply to comments, and approval system  
✅ **Secure Authentication** – Modified **MD5 with Salting & Stretching**  
✅ **Security Enhancements** – IDOR Prevention, Rate Limiting, and JWT-based session management  
✅ **Logging & Monitoring** – Integrated **Serilog** for efficient tracking  

## 🛠️ Tech Stack  

<p align="left">
  <a href="https://dotnet.microsoft.com/" target="_blank"> 
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dot-net/dot-net-original-wordmark.svg" alt=".NET Core" width="50" height="50"/> 
  </a>
  <a href="https://learn.microsoft.com/en-us/dotnet/csharp/" target="_blank"> 
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="C#" width="50" height="50"/> 
  </a>
  <a href="https://www.microsoft.com/en-us/sql-server" target="_blank"> 
    <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="SQL Server" width="50" height="50"/> 
  </a>
  <a href="https://www.mongodb.com/" target="_blank"> 
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/mongodb/mongodb-original-wordmark.svg" alt="MongoDB" width="50" height="50"/> 
  </a>

  <a href="https://www.postman.com/" target="_blank"> 
    <img src="https://www.vectorlogo.zone/logos/getpostman/getpostman-icon.svg" alt="Postman" width="50" height="50"/> 
  </a>
  <a href="https://git-scm.com/" target="_blank"> 
    <img src="https://www.vectorlogo.zone/logos/git-scm/git-scm-icon.svg" alt="Git" width="50" height="50"/> 
  </a>
</p>

---

## 📌 Features  

- 🔑 **User Authentication** – Signup, Login, JWT-based authentication, and Password Reset  
- 📝 **Posts Management** – Create, Read, Update, and Delete blog posts  
- 💬 **Comments Module** – Nested comments, replies, and moderation  
- 🔒 **Security Implementations**  
  - **Secure Password Hashing** – Modified **MD5 with Salting & Stretching**  
  - **IDOR Prevention** – Restricts unauthorized access to resources  
  - **JWT Authentication** – Token-based session management  
  - **Login Attempt Limit** – Blocks IP temporarily after **3 failed attempts**  
- 📊 **Logging & Monitoring** – Integrated **Serilog** for tracking and debugging  

## 🚀 Getting Started  

### **Prerequisites**  
- [🛠 .NET SDK (Latest Version)](https://dotnet.microsoft.com/)  
- [🗄️ MongoDB or MS SQL Server](https://www.mongodb.com/)  
- [🖥️ Visual Studio (or VS Code with C# Extension)](https://visualstudio.microsoft.com/)  

### **Installation**  

1. **Clone the repository:**  
   ```sh
   git clone https://github.com/yourusername/wordpress-clone.git
   cd wordpress-clone
2. **Install dependencies:**
   ```sh
   dotnet restore
3. Set up the database:
    Configure MongoDB/MS SQL Server connection string in appsettings.json.
 
4. **Run the API:**
   ```sh
   dotnet run

## 📡 API Endpoints

| Method | Endpoint                  | Description                   |
|--------|---------------------------|-------------------------------|
| POST   | `/api/auth/signup`        | User Registration            |
| POST   | `/api/auth/login`         | User Login                   |
| GET    | `/api/posts`              | Fetch all posts              |
| POST   | `/api/posts`              | Create a new post            |
| GET    | `/api/comments/{postId}`  | Get comments for a post      |
| POST   | `/api/comments`           | Add a comment                |
| DELETE | `/api/comments/{commentId}` | Delete a comment          |

💡 **Use Postman to test these APIs.**


## 🔐 Security & Authentication

- 🔑 **JWT Tokens** – Used for user authentication and session management  
- 🛡️ **Secure Password Hashing** – Implemented using Modified MD5 with Salting & Stretching  
- 🚫 **Rate Limiting** – Blocks IP after 3 failed login attempts  

---

## 🎯 Future Enhancements

- 🌍 **Multi-Language Support**  
- 📱 **Mobile App Integration**  
- 📊 **AI-based Content Recommendations**  
- 📢 **Social Media Sharing**  

---

## 🛠 Contributing  

Contributions are welcome!


📞 Contact
📫 Email: hiteshpatodia1@gmail.com
🔗 LinkedIn: https://www.linkedin.com/in/hitesh-patodia-a1496b14b/

⭐ If you found this useful, don’t forget to star the repository! 🚀



### **What’s Improved?**  
✅ **Proper Markdown Formatting**  
✅ **Consistent Line Breaks & Sections**  
✅ **Emoji Usage for Readability**  
✅ **Improved Structure for Contributing Section**  





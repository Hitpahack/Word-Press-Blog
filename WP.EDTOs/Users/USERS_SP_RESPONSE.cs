using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs.Users
{
    public class USERS_SP_RESPONSE : SP_BASE_RESPONSE
    {
        public ulong Id { get; set; }              // u.Id
        public string User_Login { get; set; }     // u.user_login
        public string Display_Name { get; set; }   // u.display_name
        public string User_Email { get; set; }     // u.user_email
        public string User_Nicename { get; set; }  // u.user_nicename
        public DateTime User_Registered { get; set; } // u.user_registered
        public int Total_Posts { get; set; }
        public int User_Status { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
        public string User_Url { get; set; } = null!;

    }
    
}

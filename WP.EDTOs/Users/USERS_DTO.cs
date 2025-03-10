using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WP.EDTOs.Users
{
    public class WP_USERS_BASE
    {
        [Required]
        public string UserLogin { get; set; } = null!;
        [Required]
        public string UserNicename { get; set; } = null!;
        [Required]
        public string UserEmail { get; set; } = null!;
        public string UserUrl { get; set; } = null!;
        [Required]
        public string DisplayName { get; set; } = null!;
    }
    public class USERS_DTO : WP_USERS_BASE
    {
        public ulong Id { get; set; }
        public string UserActivationKey { get; set; } = null!;
        public int UserStatus { get; set; }
        
    }
    public class WP_USERS_ADD_DTO : WP_USERS_BASE
    {
        public DateTime UserRegistered { get; set; }
        public string UserPass { get; set; } = null!;

    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BeMarketer.Models
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class ApplicationUser : IdentityUser
    {
        private UserRole role = UserRole.Admin;

        [Required(ErrorMessage = "Rola jest wymagana.")]
        public UserRole Role { get => role; set => role = value; }
    }
}
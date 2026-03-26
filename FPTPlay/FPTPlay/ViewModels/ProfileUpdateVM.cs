using System.ComponentModel.DataAnnotations;

namespace FPTPlay.ViewModels
{
    public class ProfileUpdateVM
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}

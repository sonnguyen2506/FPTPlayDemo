using System.ComponentModel.DataAnnotations;

namespace FPTPlay.ViewModels
{
    public class ChangePasswordVM
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}

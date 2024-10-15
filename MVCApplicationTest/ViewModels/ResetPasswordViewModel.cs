using System.ComponentModel.DataAnnotations;

namespace MVCApplicationTestPL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="New Password is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "New Password is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="New Password does not match !")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MVCApplicationTestPL.ViewModels
{
    public class RegisterViewModel
    {
        public string FName { get; set; }
        public string LName { get; set; }
        [Required(ErrorMessage ="Email Is Required!!")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password Is Required")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Confirm Password Is Required")]
        [Compare("Password", ErrorMessage ="The Password Doesn't Match Password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}

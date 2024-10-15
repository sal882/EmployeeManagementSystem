using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCApplicationTest.BLL.Repositories;
using MVCApplicationTest.DAL.Models;
using MVCApplicationTestPL.Helpers;
using MVCApplicationTestPL.ViewModels;
using System.Threading.Tasks;

namespace MVCApplicationTestPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerVM.Email,
                    UserName = registerVM.Email.Split("@")[0],
                    FName = registerVM.FName,
                    LName = registerVM.LName,
                    IsAgree = registerVM.IsAgree,
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    ViewData["user"] = user.UserName;
                    return RedirectToAction(nameof(Login));
                }

                    foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(registerVM);
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);

                if (user is not null)
                {
                    bool flag = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                    if(flag)
                    {
                        await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                        ViewData["user"] = loginVM.Email.Split("@")[0];
                        return RedirectToAction("Index","Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Password");
                }
                ModelState.AddModelError(string.Empty, "Email does not Existes");
            }
            return View(loginVM);
        }
        #endregion

        #region Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager?.SignOutAsync();
            ViewData["user"] = "Login";
            return RedirectToAction(nameof(Login));
        }
        #endregion
        #region Reset Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel forgetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
                if(user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword", "Account", new {email = user.Email, token = token} , Request.Scheme);

                    Email email = new Email()
                    {
                        Subject = "Reset Password",
                        Body = resetPasswordLink,
                        To= user.Email
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Email does not Existes");
            }
            return View(forgetPasswordVM);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion


        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                string email =TempData["email"] as string;
                string token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);

                await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.NewPassword);

                return RedirectToAction(nameof(Login));
            }
            return View(resetPasswordVM);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChatApp.Models;

namespace MyChatApp.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            //todo login
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    Console.WriteLine("Successfull login");
                    return RedirectToAction("Index", "Home");
                   
                }
            }
            Console.WriteLine("login failed");
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new User { 
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                Console.WriteLine("successful register");
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("register failed");
            return RedirectToAction("Register", "Account");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}

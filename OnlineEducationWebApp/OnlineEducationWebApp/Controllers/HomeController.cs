using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineEducationWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/sign-in")]
        public IActionResult SignIn([FromQuery] string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("/sign-in")]
        public IActionResult SignIn(SignInRequest request, string returnUrl = null)
        {
            var student = _context.Students.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);
            if (student != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, UserRoles.Student),
                    new Claim(ClaimTypes.Name, $"{student.Name} {student.Surname}"),
                    new Claim(JwtRegisteredClaimNames.Email, student.Email),
                    new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Now).ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, student.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", "Home");

                return Redirect(returnUrl);
            }

            var teacher = _context.Teachers.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);
            if (teacher != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, UserRoles.Teacher),
                    new Claim(ClaimTypes.Name, $"{teacher.Name} {teacher.Surname}"),
                    new Claim(JwtRegisteredClaimNames.Email, teacher.Email),
                    new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Now).ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, teacher.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", "Home");

                return Redirect(returnUrl);
            }

            return RedirectToAction("SignIn");
        }

        [HttpGet("/sign-out")]
        [Authorize]
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/access-denied")]
        public IActionResult AccessDenied([FromQuery] string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
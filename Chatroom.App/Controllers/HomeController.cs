using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Chatroom.App.Controllers
{
    using Contracts;
    using Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBotService _botService;

        public HomeController(ILogger<HomeController> logger, IBotService botService)
        {
            _logger = logger;
            _botService = botService;
        }

        private void PopulateUserData()
        {
            ViewBag.IsUserLogged = HttpContext.User.IsAuthenticated();
            ViewBag.UserLogged = HttpContext.User.FindFirstValue(JwtClaimTypes.PreferredUserName);
        }

        public IActionResult Index()
        {
            PopulateUserData();

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction("Chat");
        }

        [Authorize]
        public async Task<IActionResult> Chat()
        {
            PopulateUserData();

            return View();
        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
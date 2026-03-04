using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Services;
using System.Security.Claims;
using static System.Net.WebRequestMethods;



namespace MVC_Buddies.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly IAuthApiService _auth;
        private readonly HttpClient _http;

        public AuthController(IAuthApiService auth, IHttpClientFactory factory)
        {
            _auth = auth;
            _http = factory.CreateClient("API_Buddies");
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login() => View(new LoginDto());

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var (data, error) = await _auth.LoginAsync(dto);
            if (data == null)
            {
                ViewBag.Error = error ?? "Login falhou.";
                return View(dto);
            }

            AppendAccessTokenCookie(data.AccessToken, data.ExpiresAtUtc);
            await SignInMvcAsync(data);

            // Após login → vai verificar o perfil
            return RedirectToAction("CompleteProfile", "Account");
        }

        [HttpGet("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            await LoadRegisterViewData();
            return View(new RegisterRequestDto());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadRegisterViewData();
                return View(dto);
            }

            var (data, error) = await _auth.RegisterAsync(dto);
            if (data == null)
            {
                ViewBag.Error = error ?? "Registo falhou.";
                await LoadRegisterViewData();
                return View(dto);
            }

            AppendAccessTokenCookie(data.AccessToken, data.ExpiresAtUtc);
            await SignInMvcAsync(data);

            // Após registo → vai verificar o perfil
            return RedirectToAction("CompleteProfile", "Account");
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("access_token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Helpers mantidos
        private void AppendAccessTokenCookie(string token, DateTime expiresAtUtc)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.SpecifyKind(expiresAtUtc, DateTimeKind.Utc),
                Secure = Request.IsHttps
            };
            Response.Cookies.Append("access_token", token, options);
        }

        private async Task SignInMvcAsync(AuthResponseDto data)
        {
            var displayName = !string.IsNullOrWhiteSpace(data.UserName) ? data.UserName! : data.Email;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
            new Claim(ClaimTypes.Name, displayName),
            new Claim(ClaimTypes.Email, data.Email),
        };

            foreach (var r in data.Roles ?? Enumerable.Empty<string>())
                claims.Add(new Claim(ClaimTypes.Role, r));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = new DateTimeOffset(DateTime.SpecifyKind(data.ExpiresAtUtc, DateTimeKind.Utc))
                });
        }

        private async Task LoadRegisterViewData()
        {
            ViewBag.Skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills");
            ViewBag.Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species");
            ViewBag.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations");
        }
    }
}


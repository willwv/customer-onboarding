using Domain.Interfaces.Services;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IAuthService _authService;
        public BaseController(IAuthService authService)
        {
            _authService = authService;
        }
        protected void SetJwtTokenInCookie(string email, string role, Guid userId) 
        {
            var token = _authService.GenerateJwtToken(email, role, userId);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.ToUniversalTime().AddMinutes(Useful.TOKEN_JWT_EXPIRES_IN_30_MIN),
                Secure = false,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append(Useful.JWT_COOKIE_INDEX, token, cookieOptions);
        }
    }
}

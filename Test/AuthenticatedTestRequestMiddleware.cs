using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

/*
    Ez egy egyszerű middleware, amivel minden lekérést úgy állít be, mintha egy bejelentkezett felhasználótól érkeztek volna
    Teszteléskor ezzel az osztállyal oldjuk meg az automatikus autentikációt
*/
namespace Test
{
    public class AuthenticatedTestRequestMiddleware
    {
        public const string TestingCookieAuthentication = "TestCookieAuthentication";

        private readonly RequestDelegate _next;

        public AuthenticatedTestRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testName"),
                new Claim(ClaimTypes.NameIdentifier, "testId"),
            }, TestingCookieAuthentication);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;

            await _next(context);
        }
    }
}
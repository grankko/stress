using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Stress.Server.Middleware
{
    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            SetSecurityHeaders(context);
            await _next(context);
        }

        public static void SetSecurityHeaders(HttpContext context)
        {
            context.Response.Headers.Add("referrer-policy", new StringValues("no-referrer"));
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));
            context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));
            context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                "block-all-mixed-content;" +
                "default-src 'self';"
                ));
            context.Response.Headers.Add("Permissions-Policy", new StringValues(
                "accelerometer=(), " +
                "camera=(), " +
                "geolocation=(), " +
                "gyroscope=(), " +
                "magnetometer=(), " +
                "microphone=(), " +
                "payment=(), " +
                "usb=()"));
        }
    }
}
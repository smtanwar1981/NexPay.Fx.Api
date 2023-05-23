using Microsoft.AspNetCore.Mvc.Filters;

namespace NexPay.Fx.Api.Core
{
    public sealed class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5278/");
                    var result = await client.GetAsync($"Security/validateToken/{token}");
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception("Unauthorized access.");
                    }
                }
            }
        }
    }
}

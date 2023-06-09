﻿using Newtonsoft.Json;
using System.Text;

namespace NexPay.Fx.Api.Service
{
    public class LoginApiProxyService : ILoginApiProxyService
    {
        private readonly IConfiguration _configuration;
        public LoginApiProxyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<bool> AuthenticateRequest(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetValue<string>("AuthenticationBaseUrl"));
                var result = await client.GetAsync($"{_configuration.GetValue<string>("AuthenticationBaseUri")}{token}");
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
        }
    }
}

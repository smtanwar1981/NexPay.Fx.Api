namespace NexPay.Fx.Api.Service
{
    public interface ILoginApiProxyService
    {
        /// <summary>
        /// A method to authenticate incoming request.
        /// </summary>
        /// <param name="token">Required Jwt token.</param>
        /// <returns>True or false depending on the authentication.</returns>
        public Task<bool> AuthenticateRequest(string token);
    }
}

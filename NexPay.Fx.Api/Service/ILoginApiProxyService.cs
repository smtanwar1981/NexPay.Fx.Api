namespace NexPay.Fx.Api.Service
{
    public interface ILoginApiProxyService
    {
        public Task<bool> AuthenticateRequest(string token);
    }
}

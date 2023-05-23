using NexPay.Fx.Api.Model;

namespace NexPay.Fx.Api.Service
{
    public interface IFxService
    {
        /// <summary>
        /// This method will fetch the conversion rate from the repository.
        /// </summary>
        /// <param name="request">Api request having To and From currency information.</param>
        /// <returns>A conversion rate in decimal format.</returns>
        public Task<decimal> GetCurrencyExchangeRate(GetExchangeRateRequest request);
    }
}

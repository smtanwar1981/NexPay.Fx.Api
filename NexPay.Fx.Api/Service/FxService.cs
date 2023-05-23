using Newtonsoft.Json;
using NexPay.Fx.Api.Common;
using NexPay.Fx.Api.Model;

namespace NexPay.Fx.Api.Service
{
    public class FxService : IFxService
    {
        private readonly ILogger<FxService> _logger;
        private readonly IConfiguration _configuration;
        public FxService(IConfiguration configuration, ILogger<FxService> logger)
        {
            _logger = logger;
            _configuration = configuration; 
        }

        /// <inheritdoc />
        public async Task<decimal> GetCurrencyExchangeRate(GetExchangeRateRequest request)
        {
            _logger.LogInformation("Begin Executing GetCurrencyExchangeRate() of FxService class");

            decimal conversionRate = 0;

            var exchange = await GetExchangeRates();
            if (exchange is not null)
            {
                var currencyWithRates = exchange.CurrencyExchange?.Where(x => string.Equals(x.CurrencyCode, request.CurrencyCodeFrom, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (currencyWithRates != null)
                {
                    var conversion = currencyWithRates.ConversionRates?.Where(x => string.Equals(x.ToCurrency, request.CurrencyCodeTo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (conversion is null)
                    {
                        throw new Exception("Invalid TO currency code.");
                    }
                    conversionRate = Convert.ToDecimal(conversion?.Rate);
                }
                else
                {
                    throw new Exception("Invalid FROM currency code.");
                }
            }
            else
            {
                throw new Exception("Exchange rates are not available, Please try after some time.");
            }
            _logger.LogInformation("End Executing GetCurrencyExchangeRate() of FxService class");

            return conversionRate;
        }

        private async Task<Exchange>? GetExchangeRates()
        {
            Exchange? exchange = null;
            var filePath = _configuration.GetValue<string>(Constants.ExchangeRateFilePath);
            if (File.Exists(filePath))
            {
                using (StreamReader content = new StreamReader(filePath))
                {
                    string json = await content.ReadToEndAsync();
                    exchange = JsonConvert.DeserializeObject<Exchange>(json);
                }
            }
            return exchange;
        }
    }
}

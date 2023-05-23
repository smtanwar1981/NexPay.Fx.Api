namespace NexPay.Fx.Api.Model
{
    public class GetExchangeRateRequest
    {
        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        public string? CurrencyCodeFrom { get; set; }

        /// <summary>
        /// Gets or sets CurrencyCodeTo.
        /// </summary>
        public string? CurrencyCodeTo { get; set; }
    }
}

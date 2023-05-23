namespace NexPay.Fx.Api.Model
{
    /// <summary>
    /// A class to instantiate the Existing Exchange.
    /// </summary>
    public class Exchange
    {
        /// <summary>
        /// Gets or sets CurrencyExchange.
        /// </summary>
        public List<CurrencyWithRates>? CurrencyExchange { get; set; }
    }

    public class CurrencyWithRates
    { 
        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets ConversionRates.
        /// </summary>
        public List<ConversionRates>? ConversionRates { get; set; }
    }

    public class ConversionRates
    {
        /// <summary>
        /// Gets or sets ToCurrency.
        /// </summary>
        public string? ToCurrency { get; set; }

        /// <summary>
        /// Gets or sets Rate.
        /// </summary>
        public string? Rate { get; set; }
    }
}

namespace API.Models.Other
{
    public class CurrencySettings
    {
        public string Default { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public List<SupportedCurrency> SupportedCurrencies { get; set; }
    }

    public class SupportedCurrency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
    }
}

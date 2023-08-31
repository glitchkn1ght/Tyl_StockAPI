using Newtonsoft.Json;

namespace Tyl_StockAPI.Models.Finhub
{
    public class Quote
    {
        [JsonProperty("c")]
        public decimal CurrentPrice{ get; set; }

        [JsonProperty("d")]
        public decimal Change { get; set; }

        [JsonProperty("dp")]
        public decimal PercentChange { get; set; }

        [JsonProperty("h")]
        public decimal DayHighPrice { get; set; }

        [JsonProperty("l")]
        public decimal DayLowPrice { get; set; }

        [JsonProperty("o")]
        public decimal DayOpenPrice { get; set; }

        [JsonProperty("pc")]
        public decimal PreviousClosePrice { get; set; }
    }
}

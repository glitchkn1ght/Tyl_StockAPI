using Stock_API.Validation;
using System.ComponentModel.DataAnnotations;

namespace Stock_API.Models
{
    public class Trade
    {
        [NotNullOrEmpty]
        public Guid TradeId { get; set; }

        [NotNullOrEmpty]
        public Guid BrokerId { get; set; }

        [Required]
        [TickerSymbol]
        public string TickerSymbol { get; set; } = string.Empty;

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PriceTotal { get; set; }

        [Required]
        public string TradeCurrency { get; set; } = string.Empty;

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal NumberOfShares { get; set; }
    }
}
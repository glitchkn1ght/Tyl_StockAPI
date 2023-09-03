using System.ComponentModel.DataAnnotations;

namespace CommonModels
{
    public class Trade
    {
        [Required]
        public Guid TradeId { get; set; }

        [Required]
        public Guid BrokerId { get; set; }

        [Required]
        [StringLength(5)]
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
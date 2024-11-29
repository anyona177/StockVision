using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockVision.Entities
{
    public class HistoricalData
    {
        [Key]
        public int HistoricalDataId { get; set; } // PK

        [ForeignKey("Asset")]
        public int AssetId { get; set; } // FK

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal ClosingPrice { get; set; }

        [Required]
        public decimal MaxPrice { get; set; }

        [Required]
        public decimal MinPrice { get; set; }

        [Required]
        public int Volume { get; set; }

        public Asset? Asset { get; set; } // Navigation Property
    }
}

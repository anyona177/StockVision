using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockVision.Entities
{
    public class Asset
    {
        [Key]
        public int AssetId { get; set; } // PK

        [Required]
        [MaxLength(50)]
        public string AssetType { get; set; } = string.Empty; // Тип актива

        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; } = string.Empty; // Символ актива

        [Required]
        public decimal CurrentPrice { get; set; } // Текущая цена

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty; // Валюта

        [Required]
        [MaxLength(50)]
        public string Exchange { get; set; } = string.Empty; // Биржа

        [Required]
        public DateTime LastUpdated { get; set; } // Дата обновления

        // Связь один-ко-многим с HistoricalData
        public ICollection<HistoricalData> HistoricalData { get; set; } = new List<HistoricalData>(); // Коллекция исторических данных

        // Связь один-ко-многим с Transaction
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Коллекция транзакций
        public required string Type { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

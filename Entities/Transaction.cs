using System;
using System.ComponentModel.DataAnnotations;

namespace StockVision.Entities
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; } // PK

        [Required]
        public int PortfolioId { get; set; } // FK к Portfolio

        [Required]
        public int AssetId { get; set; } // FK к Asset

        [Required]
        public DateTime TransactionDate { get; set; } // Дата транзакции

        [Required]
        public int Quantity { get; set; } // Количество акций/облигаций

        [Required]
        public decimal PricePerUnit { get; set; } // Цена за единицу

        [Required]
        [MaxLength(10)]
        public string TransactionType { get; set; } = string.Empty; // Тип транзакции (покупка, продажа)

        // Навигационные свойства
        public Portfolio Portfolio { get; set; } = null!; // Связь с Portfolio
        public Asset Asset { get; set; } = null!; // Связь с Asset
        public decimal UnitPrice { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockVision.Entities
{
    public class Portfolio
    {
        [Key]
        public int PortfolioId { get; set; } // PK

        [Required]
        [MaxLength(100)]
        public string PortfolioName { get; set; } = string.Empty; // Название портфеля

        [Required]
        public int UserId { get; set; } // FK к User

        [Required]
        public decimal TotalValue { get; set; } // Общая стоимость портфеля

        [Required]
        public DateTime LastUpdated { get; set; } // Дата последнего обновления

        // Связь с User
        public User? User { get; set; } // Навигационное свойство к User

        // Связь один-ко-многим с Transaction
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public required string Name { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

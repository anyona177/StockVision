using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StockVision.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // PK

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Обязательно поле

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty; // Обязательно поле

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty; // e.g., Admin, Analyst

        [Required]
        public DateTime RegistrationDate { get; set; }

        // Связь один-ко-многим с Portfolio
        [JsonIgnore]
        public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}

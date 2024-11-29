using StockVision.Entities;

namespace Entities
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public int UserId { get; set; } // FK на таблицу User
        public required string PasswordHash { get; set; } // Хэш пароля
        public DateTime LastLogin { get; set; } // Последняя авторизация
        public required User User { get; set; } // Связь с пользователем
    }


}


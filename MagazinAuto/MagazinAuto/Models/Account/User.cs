using System;

namespace MagazinAuto.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Nume { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}

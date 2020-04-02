using System;
using System.ComponentModel.DataAnnotations;

namespace MagazinAuto.Models
{
    public class RegisterModel
    {
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string Telefon { get; set; }
        [Required]
        public string Nume { get; set; }
    }
}

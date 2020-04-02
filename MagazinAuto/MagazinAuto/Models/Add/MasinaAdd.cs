using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace MagazinAuto.Models
{
    public class MasinaAdd
    {
        public Guid Id { get; set; }
        [Required]
        public Caroserie Caroserie { get; set; }
        [Required]
        public Cutie Cutie { get; set; }
        [Required]
        public Transmisie Transmisie { get; set; }
        [Required]
        public NormaPoluare NormaPoluare { get; set; }
        [Required]
        public Combustibil Combustibil { get; set; }
        [Required]
        public int? CP { get; set; }
        [Required]
        public int? CapacitateCilindrica { get; set; }
        [Required]
        public int? Km { get; set; }
        [Required]
        public int? AnFabricatie { get; set; }
        [Required]
        public int? Pret { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Descriere { get; set; }
        [Required]
        public IFormFile Poza { get; set; }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MagazinAuto.Models;
using System.IO;

namespace MagazinAuto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<MasinaView> list = new List<MasinaView>();
        private readonly User currentUser;

        public HomeController(ILogger<HomeController> logger, User currentUser)
        {
            _logger = logger;
            this.currentUser = currentUser;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ViewCars");
        }

        [HttpGet]
        public IActionResult ViewCars()
        {
            return View(list);
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            if (!currentUser.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new MasinaAdd();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddCar(MasinaAdd car)
        {
            if (!currentUser.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(car);
            }

            var newCar = new MasinaView
            {
                AnFabricatie = car.AnFabricatie.Value,
                CapacitateCilindrica = car.CapacitateCilindrica.Value,
                Caroserie = car.Caroserie,
                Combustibil = car.Combustibil,
                CP = car.CP.Value,
                Cutie = car.Cutie,
                Descriere = car.Descriere,
                Km = car.Km.Value,
                Marca = car.Marca,
                Model = car.Model,
                Pret = car.Pret.Value,
                NormaPoluare = car.NormaPoluare,
                Transmisie = car.Transmisie,
                Proprietar = new User
                {
                    Id = currentUser.Id,
                    Email = currentUser.Email,
                    Nume = currentUser.Nume,
                    Telefon = currentUser.Telefon
                }
            };

            if (car.Poza != null)
            {
                if (car.Poza.Length > 0)
                {

                    byte[] content = null;
                    using (var fs = car.Poza.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        content = ms.ToArray();
                    }

                    newCar.Poza = content;
                }
            }

            list.Add(newCar);

            return RedirectToAction("ViewCars");
        }
    }
}

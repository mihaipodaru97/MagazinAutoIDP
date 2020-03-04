using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MagazinAuto.Models;
using System.IO;
using System.Text;

namespace MagazinAuto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<MasinaView> list = new List<MasinaView>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewCars()
        {
            return View(list);
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            var model = new MasinaAdd();
            model.ProprietarId = Guid.NewGuid();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddCar(MasinaAdd car)
        {
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
                NormaPoluare = car.NormaPoluare,
                Transmisie = car.Transmisie,
                Proprietar = new User
                {
                    Id = car.ProprietarId,
                    Email = "podarumihai97@gmail.com",
                    Nume = "Mihai",
                    Telefon = "0724501858"
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

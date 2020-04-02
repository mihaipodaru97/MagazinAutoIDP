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
        private readonly User currentUser;
        private readonly Services services;

        public HomeController(ILogger<HomeController> logger, User currentUser, Services services)
        {
            this.services = services;
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
            return View(services.GetCars());
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            var model = new MasinaAdd();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddCar(MasinaAdd car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            byte[] content = null;

            if (car.Poza != null)
            {
                if (car.Poza.Length > 0)
                {

                    
                    using (var fs = car.Poza.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        content = ms.ToArray();
                    }
                }
            }

            services.AddCar(car, currentUser?.Id, content);

            return RedirectToAction("ViewCars");
        }
    }
}

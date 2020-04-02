using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MagazinAuto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MagazinAuto.Controllers
{
    public class AccountController : Controller
    {
        private readonly User currentUser;
        private readonly Services services;
        private const string schemeName = "Cookiesv2";

        public AccountController(User currentUser, Services services)
        {
            this.services = services;
            this.currentUser = currentUser;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (currentUser.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = services.CheckUser(model.Email, model.Password);

            if(user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email sau parola gresita");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };


            var identity = new ClaimsIdentity(claims, schemeName);
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(
                scheme: schemeName,
                principal: principal
                );

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(scheme: schemeName);

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (currentUser.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var model = new RegisterModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (currentUser.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = services.GetUser(model.Email);

            if(user != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email-ul exista deja");

                return View(model);
            }

            model.Id = Guid.NewGuid();
            services.AddUser(model);

            return RedirectToAction("Login", "Account");
        }
    }
}
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _kullaniciRepository;

        public AccountController(IUserRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User kullanici) //giriş yap butonuna basınca çalışacak kodlar
        {
            var KullaniciVarMi = _kullaniciRepository.GetMany(x => x.Email == kullanici.Email && x.Password == kullanici.Password && x.IsActive == true).SingleOrDefault();
            if(KullaniciVarMi != null)
            {
                if(KullaniciVarMi.Role.Name == "Admin")
                {
                    Session["KullaniciEmail"] = KullaniciVarMi.Email;
                    return RedirectToAction("Index","Home");
                }
                ViewBag.Mesaj = "Yetkisiz kullanıcı";
                return View();
            }
            ViewBag.Mesaj = "Kullanıcı bulunamadı";
            return View();
        }
    }
}
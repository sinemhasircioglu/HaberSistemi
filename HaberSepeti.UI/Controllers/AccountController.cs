using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IKullaniciRepository _kullaniciRepository;

        public AccountController(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanici kullanici) 
        {
            var KullaniciVarMi = _kullaniciRepository.GetMany(x => x.Email == kullanici.Email && x.Sifre == kullanici.Sifre && x.AktifMi == true).SingleOrDefault();
            if (KullaniciVarMi != null)
            {
                if (KullaniciVarMi.Rol.Ad == "Admin" || KullaniciVarMi.Rol.Ad == "Editör" || KullaniciVarMi.Rol.Ad == "Yazar")
                {
                    Session["KullaniciId"] = KullaniciVarMi.Id;
                    return RedirectToAction("Index", "Admin");
                }
                else if (KullaniciVarMi.Rol.Ad == "Üye")
                {
                    Session["KullaniciId"] = KullaniciVarMi.Id;
                    return RedirectToAction("Index", "Default");
                }
                ViewBag.Mesaj = "Yetkisiz kullanıcı";
                return View();
            }
            ViewBag.Mesaj = "Geçersiz Email/Şifre girdiniz!";
            return View();
        }
    }
}
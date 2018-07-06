using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
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
        public ActionResult Login(Kullanici kullanici) //giriş yap butonuna basınca çalışacak kodlar
        {
            var KullaniciVarMi = _kullaniciRepository.GetMany(x => x.Email == kullanici.Email && x.Sifre == kullanici.Sifre && x.AktifMi == true).SingleOrDefault();
            if(KullaniciVarMi != null)
            {
                if(KullaniciVarMi.Rol.Ad == "Admin")
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
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.UI.Controllers
{
    public class CaptchaResultt
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

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

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Kullanici kullanici)
        {
            var KullaniciVarMi = _kullaniciRepository.GetMany(x => x.Email == kullanici.Email).SingleOrDefault();
            if (KullaniciVarMi != null)
            {
                ViewBag.Mesaj = "Bu email kullanılmış !";
                return View();
            }
            Kullanici yeniKullanici = new Kullanici
            {
                AdSoyad = kullanici.AdSoyad,
                Email = kullanici.Email,
                Sifre = kullanici.Sifre,
                RolId = 3,
                KayitTarihi = DateTime.Now,
                AktifMi = true,
            };

            var recaptcha = Request.Form["g-recaptcha-response"];
            const string secretkey = "6LfO72kUAAAAANUJJDtClqUZ4n_ei8_bpK5DuHLs";
            var restUrl = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretkey, recaptcha);
            WebRequest request = WebRequest.Create(restUrl);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            JsonSerializer serializer = new JsonSerializer();
            CaptchaResultt result = null;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string resultObject = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<CaptchaResultt>(resultObject);
            }
            if (!result.Success)
            {
                ModelState.AddModelError("", "captcha mesajınız hatalı");
                if (result.ErrorCodes != null)
                {
                    ModelState.AddModelError("", result.ErrorCodes[0]);
                }
                return View();
            }
            else
            {
                try
                {
                    _kullaniciRepository.Insert(yeniKullanici);
                    _kullaniciRepository.Save();
                    return RedirectToAction("Index", "Default");
                }
                catch (Exception)
                {
                    return View();
                }
            }
        }
    }
}
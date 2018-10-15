using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IKullaniciRepository _kullaniciRepository;

        public KullaniciController(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 5;
            var kullaniciListesi = _kullaniciRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            return View(kullaniciListesi);
        }

        public ActionResult Sil(int id)
        {
            Kullanici kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            _kullaniciRepository.Delete(id);
            _kullaniciRepository.Save();
            TempData["Bilgi"] = "Kullanıcı başarıyla silindi";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeAdmin(int id)
        {
            Kullanici kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            kullanici.RolId = 1;
            _kullaniciRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık admin";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeEditor(int id)
        {
            Kullanici kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            kullanici.RolId = 2;
            _kullaniciRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık editör";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeAuthor(int id)
        {
            Kullanici kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            kullanici.RolId = 4;
            _kullaniciRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık yazar";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeMember(int id)
        {
            Kullanici kullanici = _kullaniciRepository.GetById(id);
            if (kullanici == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            kullanici.RolId = 3;
            _kullaniciRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık sadece üye";
            return RedirectToAction("Index", "Kullanici");
        }
    }
}
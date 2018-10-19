using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
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
        private readonly IUserRepository _userRepository;

        public KullaniciController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 5;
            var kullaniciListesi = _userRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            return View(kullaniciListesi);
        }

        public ActionResult Sil(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            _userRepository.Delete(id);
            _userRepository.Save();
            TempData["Bilgi"] = "Kullanıcı başarıyla silindi";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeAdmin(int id)
        {
            User kullanici = _userRepository.GetById(id);
            if (kullanici == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            kullanici.RoleId = 1;
            _userRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık admin";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeEditor(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            user.RoleId = 2;
            _userRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık editör";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeAuthor(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            user.RoleId = 4;
            _userRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık yazar";
            return RedirectToAction("Index", "Kullanici");
        }

        public ActionResult MakeMember(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Bilgi"] = "Kullanıcı bulunamadı!";
            user.RoleId = 3;
            _userRepository.Save();
            TempData["Bilgi"] = "Kullanıcı artık sadece üye";
            return RedirectToAction("Index", "Kullanici");
        }
    }
}
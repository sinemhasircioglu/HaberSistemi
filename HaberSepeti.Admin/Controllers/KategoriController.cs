using HaberSepeti.Admin.Class;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HaberSepeti.Admin.CustomFilter;

namespace HaberSepeti.Admin.Controllers
{
    public class KategoriController : Controller
    {
        private readonly IKategoriRepository _kategoriRepository;

        public KategoriController(IKategoriRepository kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 5;
            return View(_kategoriRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu));
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View();
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Ekle(Kategori kategori)
        {
            _kategoriRepository.Insert(kategori);
            _kategoriRepository.Save();
            TempData["Bilgi"] = "Kategori ekleme işleminiz başarılı";
            return RedirectToAction("Index", "Kategori");
        }

        public void SetKategoriListele()
        {
            var KategoriList = _kategoriRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }

        [LoginFilter]
        public ActionResult Sil(int id)
        {
            Kategori kategori = _kategoriRepository.GetById(id);
            if (kategori == null)
                TempData["Bilgi"] = "Kategori bulunamadı!";
            _kategoriRepository.Delete(id);
            _kategoriRepository.Save();
            TempData["Bilgi"] = "Kategori başarıyla silindi";
            return RedirectToAction("Index", "Kategori");
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            Kategori gelenKategori = _kategoriRepository.GetById(id);
            if (gelenKategori == null)
                TempData["Bilgi"] = "Kategori bulunamadı!";
            SetKategoriListele();
            return View(gelenKategori);
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult Duzenle(Kategori kategori)
        {
            Kategori dbKategori = _kategoriRepository.GetById(kategori.Id);
            dbKategori.AktifMi = kategori.AktifMi;
            dbKategori.Ad = kategori.Ad;
            dbKategori.URL = kategori.URL;
            dbKategori.ParentId = kategori.ParentId;
            _kategoriRepository.Save();
            TempData["Bilgi"] = "Kategori düzenleme işleminiz başarılı.";
            return RedirectToAction("Index", "Kategori");
        }
    }
}
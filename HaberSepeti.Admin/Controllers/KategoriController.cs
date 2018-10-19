using HaberSepeti.Admin.Class;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
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
        private readonly ICategoryRepository _categoryRepository;

        public KategoriController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 5;
            return View(_categoryRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu));
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
        public ActionResult Ekle(Category category)
        {
            _categoryRepository.Insert(category);
            _categoryRepository.Save();
            TempData["Bilgi"] = "Kategori ekleme işleminiz başarılı";
            return RedirectToAction("Index", "Kategori");
        }

        public void SetKategoriListele()
        {
            var KategoriList = _categoryRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }

        [LoginFilter]
        public ActionResult Sil(int id)
        {
            Category kategori = _categoryRepository.GetById(id);
            if (kategori == null)
                TempData["Bilgi"] = "Kategori bulunamadı!";
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
            TempData["Bilgi"] = "Kategori başarıyla silindi";
            return RedirectToAction("Index", "Kategori");
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            Category gelenKategori = _categoryRepository.GetById(id);
            if (gelenKategori == null)
                TempData["Bilgi"] = "Kategori bulunamadı!";
            SetKategoriListele();
            return View(gelenKategori);
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult Duzenle(Category kategori)
        {
            Category dbKategori = _categoryRepository.GetById(kategori.Id);
            dbKategori.IsActive = kategori.IsActive;
            dbKategori.Name = kategori.Name;
            dbKategori.URL = kategori.URL;
            dbKategori.ParentId = kategori.ParentId;
            _categoryRepository.Save();
            TempData["Bilgi"] = "Kategori düzenleme işleminiz başarılı.";
            return RedirectToAction("Index", "Kategori");
        }
    }
}
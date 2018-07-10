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
        public JsonResult Ekle(Kategori kategori)
        {
            try
            {
                _kategoriRepository.Insert(kategori);
                _kategoriRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Ekleme işleminiz başarılı" });
            }
            catch (Exception)
            {
                return Json(new ResultJson { Success = false, Message = "Ekleme işleminiz başarısız!" });
            }
        }

        public void SetKategoriListele()
        {
            var KategoriList = _kategoriRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }

        public JsonResult Sil(int id)
        {
            Kategori kategori = _kategoriRepository.GetById(id);
            if (kategori == null)
                return Json(new ResultJson { Success = false, Message = "Kategori bulunamadı!" });
            _kategoriRepository.Delete(id);
            _kategoriRepository.Save();
            return Json(new ResultJson { Success = true, Message = "Kategori silme işleminiz başarılı" });
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            Kategori kategori = _kategoriRepository.GetById(id);
            if (kategori == null)
                throw new Exception("Kategori bulunamadı!");
            SetKategoriListele();
            return View(kategori);
        }

        [HttpPost]
        public JsonResult Duzenle(Kategori kategori)
        {
            Kategori dbKategori = _kategoriRepository.GetById(kategori.Id);
            dbKategori.AktifMi = kategori.AktifMi;
            dbKategori.Ad = kategori.Ad;
            dbKategori.URL = kategori.URL;
            dbKategori.ParentId = kategori.ParentId;
            _kategoriRepository.Save();
            return Json(new ResultJson { Success = true, Message = "Düzenleme işlemi başarılı" });
        }
    }
}
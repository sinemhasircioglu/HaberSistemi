using HaberSepeti.Admin.Class;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            return View(_kategoriRepository.GetAll().ToList());
        }

        [HttpGet]
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

        public ActionResult Sil(int id)
        {
            Kategori kategori = _kategoriRepository.GetById(id);
            if (kategori == null)
                throw new Exception("Kategori bulunamadı");
            _kategoriRepository.Delete(id);
            _kategoriRepository.Save();
            return RedirectToAction("Index", "Kategori");
        }
    }
}
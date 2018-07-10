using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class HaberController : Controller
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IHaberRepository _haberRepository;
        private readonly IKategoriRepository _kategoriRepository;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View();
        }

        public ActionResult Ekle(Haber haber, int kategoriId, HttpPostedFileBase vitrinResmi)
        {
            var sessionControl = HttpContext.Session["KulaniciEmail"];
            if (ModelState.IsValid)
            {
                Kullanici kullanici = _kullaniciRepository.GetById(Convert.ToInt32(sessionControl));
                haber.KullaniciId = kullanici.Id;
                haber.KategoriId = kategoriId;
                if (vitrinResmi != null)
                {
                    string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                    Request.Files[0].SaveAs(Server.MapPath(tamYol));
                    haber.Resim = tamYol;
                }
                _haberRepository.Insert(haber);
                _haberRepository.Save();
                return View();
            }
            return View();
        }

        public void SetKategoriListele()
        {
            var KategoriList = _kategoriRepository.GetAll().ToList();
            ViewBag.Kategori = KategoriList;
        }
    }
}
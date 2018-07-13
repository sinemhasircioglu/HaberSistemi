using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IResimRepository _resimRepository;

        public HaberController(IKullaniciRepository kullaniciRepository, IHaberRepository haberRepository, IKategoriRepository kategoriRepository, IResimRepository resimRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _haberRepository = haberRepository;
            _kategoriRepository = kategoriRepository;
            _resimRepository = resimRepository;
        }

        [LoginFilter]
        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 5;
            var HaberListesi = _haberRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            return View(HaberListesi);
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
        public ActionResult Ekle(Haber haber, int KategoriId, HttpPostedFileBase VitrinResim,IEnumerable<HttpPostedFileBase> DetayResim)
        {
            var sessionControl = HttpContext.Session["KullaniciEmail"];
            Kullanici kullanici = _kullaniciRepository.Get(x => x.Email == sessionControl);
            haber.KullaniciId = kullanici.Id;
            haber.KategoriId = KategoriId;
            if (VitrinResim != null)
            {
                string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(tamYol));
                haber.VitrinResim = tamYol;
            }
            _haberRepository.Insert(haber);
            _haberRepository.Save();
            string cokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if(cokluResim != "")
            {
                foreach (var file in DetayResim)
                {
                    if(file.ContentLength > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                        string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                        file.SaveAs(Server.MapPath(tamYol));
                        var resim = new Resim {
                            ResimUrl = tamYol                           
                        };
                        resim.HaberId = haber.Id;
                        _resimRepository.Insert(resim);
                        _resimRepository.Save();
                    }
                }
            }
            TempData["Bilgi"] = "Haber ekleme işleminiz başarılı";
            return RedirectToAction("Index", "Haber");
        }

        public void SetKategoriListele(object kategori = null)
        {
            var KategoriList = _kategoriRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }

        public ActionResult Sil(int id)
        {
            Haber dbHaber = _haberRepository.GetById(id);
            var dbDetayResim = _resimRepository.GetMany(x => x.HaberId == id);
            if (dbHaber == null)
                throw new Exception("Haber Bulunamadı!");

            string file_name = dbHaber.VitrinResim;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists) // dosyanın varlığı kontrol ediliyor. fiziksel olarak varsa siliniyor.
                file.Delete();
            if(dbDetayResim!=null)
            {
                foreach (var item in dbDetayResim)
                {
                    string detayPath = Server.MapPath(item.ResimUrl);
                    FileInfo files = new FileInfo(detayPath);
                    if (files.Exists)
                        files.Delete();
                }
            }
            _haberRepository.Delete(id);
            _haberRepository.Save();
            TempData["Bilgi"] = "Haber başarıyla silindi";
            return RedirectToAction("Index", "Haber");
        }
    }
}
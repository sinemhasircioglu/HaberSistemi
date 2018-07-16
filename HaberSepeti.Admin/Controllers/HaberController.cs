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
        private readonly IEtiketRepository _etiketRepository;

        public HaberController(IKullaniciRepository kullaniciRepository, IHaberRepository haberRepository, IKategoriRepository kategoriRepository, IResimRepository resimRepository, IEtiketRepository etiketRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _haberRepository = haberRepository;
            _kategoriRepository = kategoriRepository;
            _resimRepository = resimRepository;
            _etiketRepository = etiketRepository;
        }

        [HttpGet]
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
        public ActionResult Ekle(Haber haber, int KategoriId, HttpPostedFileBase VitrinResim, IEnumerable<HttpPostedFileBase> DetayResim, string Etiket)
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

            _etiketRepository.EtiketEkle(haber.Id, Etiket);

            string cokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if (cokluResim != "")
            {
                foreach (var file in DetayResim)
                {
                    if (file.ContentLength > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                        string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                        file.SaveAs(Server.MapPath(tamYol));
                        var resim = new Resim
                        {
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

        [LoginFilter]
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
            if (dbDetayResim != null)
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

        [HttpGet]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Duzenle(int id)
        {
            Haber gelenHaber = _haberRepository.GetById(id);
            if (gelenHaber == null)
                throw new Exception("Haber bulunamadı!");
            SetKategoriListele();
            return View(gelenHaber);
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Duzenle(Haber haber, int kategoriId, HttpPostedFileBase VitrinResim, IEnumerable<HttpPostedFileBase> DetayResim)
        {
            Haber gelenHaber = _haberRepository.GetById(haber.Id);
            gelenHaber.Aciklama = haber.Aciklama;
            gelenHaber.KisaAciklama = haber.KisaAciklama;
            gelenHaber.Baslik = haber.Baslik;
            gelenHaber.AktifMi = haber.AktifMi;
            gelenHaber.KategoriId = kategoriId;

            if(VitrinResim!=null)
            {
                string dosyaAdi = gelenHaber.VitrinResim;
                string dosyaYolu = Server.MapPath(dosyaAdi);
                FileInfo dosya = new FileInfo(dosyaYolu);
                if (dosya.Exists)
                    dosya.Delete();
                string dosya_adi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string tam_yol = "/External/Haber/" + dosya_adi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(tam_yol));
                gelenHaber.VitrinResim = tam_yol;
            }

            string cokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if(cokluResim!="")
            {
                foreach (var detay in DetayResim)
                {
                    string dosya_adi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                    string tamyol = "/External/Haber/" + dosya_adi + uzanti;
                    detay.SaveAs(Server.MapPath(tamyol));
                    var img = new Resim {
                        ResimUrl = tamyol
                    };
                    img.HaberId = gelenHaber.Id;
                    _resimRepository.Insert(img);
                    _resimRepository.Save();
                }
            }
            _haberRepository.Save();
            TempData["Bilgi"] = "Haber düzenleme işleminiz başarılı.";
            return RedirectToAction("Index", "Haber");
        }

        public ActionResult ResimSil(int id)
        {
            Resim dbResim = _resimRepository.GetById(id);
            if (dbResim == null)
                throw new Exception("Resim bulunamadı!");
            string file_name = dbResim.ResimUrl;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
                file.Delete();
            _resimRepository.Delete(id);
            _resimRepository.Save();
            TempData["Bilgi"] = "Resim silme işleminiz başarılı!";
            return RedirectToAction("Index", "Haber");
        }
    }
}
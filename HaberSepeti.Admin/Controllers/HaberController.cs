using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using HaberSepeti.Data.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class HaberController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly ITagRepository _tagRepository;
        string uploadpath = ConfigurationManager.AppSettings["UploadPathHaber"].ToString();


        public HaberController(IUserRepository kullaniciRepository, INewsRepository haberRepository, ICategoryRepository kategoriRepository, IPictureRepository resimRepository, ITagRepository etiketRepository)
        {
            _userRepository = kullaniciRepository;
            _newsRepository = haberRepository;
            _categoryRepository = kategoriRepository;
            _pictureRepository = resimRepository;
            _tagRepository = etiketRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 5;
            var HaberListesi = _newsRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
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
        public ActionResult Ekle(News haber, int KategoriId, HttpPostedFileBase VitrinResim, IEnumerable<HttpPostedFileBase> DetayResim, string Etiket)
        {
            var sessionControl = HttpContext.Session["KullaniciEmail"];
            User kullanici = _userRepository.Get(x => x.Email == sessionControl);
            haber.UserId = kullanici.Id;
            haber.CategoryId = KategoriId;
            if (VitrinResim != null)
            {
                string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string tamYol = uploadpath + dosyaAdi + uzanti;
                Request.Files[0].SaveAs(tamYol);
                haber.ShowcasePicture = tamYol.Substring(46);
            }
            _newsRepository.Insert(haber);
            _newsRepository.Save();

            _tagRepository.EtiketEkle(haber.Id, Etiket);

            string cokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if (cokluResim != "")
            {
                foreach (var file in DetayResim)
                {
                    if (file.ContentLength > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                        //string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                        //string tamYol = "C:\\Users\\sinem\\Source\\Repos\\HaberSepeti\\Exter\\Haber\\" + dosyaAdi + uzanti; // doğru olan
                        string tamYol = uploadpath + dosyaAdi + uzanti;


                        //file.SaveAs(Server.MapPath(tamYol));
                        file.SaveAs(tamYol);
                        var resim = new Picture
                        {
                            PictureUrl = tamYol.Substring(46)
                        };
                        resim.NewsId = haber.Id;
                        _pictureRepository.Insert(resim);
                        _pictureRepository.Save();
                    }
                }
            }
            TempData["Bilgi"] = "Haber ekleme işleminiz başarılı";
            return RedirectToAction("Index", "Haber");
        }

        public void SetKategoriListele(object kategori = null)
        {
            var KategoriList = _categoryRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }

        [LoginFilter]
        public ActionResult Sil(int id)
        {
            News dbHaber = _newsRepository.GetById(id);
            var dbDetayResim = _pictureRepository.GetMany(x => x.NewsId == id);
            if (dbHaber == null)
                throw new Exception("Haber Bulunamadı!");

            string file_name = dbHaber.ShowcasePicture;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists) // dosyanın varlığı kontrol ediliyor. fiziksel olarak varsa siliniyor.
                file.Delete();
            if (dbDetayResim != null)
            {
                foreach (var item in dbDetayResim)
                {
                    string detayPath = Server.MapPath(item.PictureUrl);
                    FileInfo files = new FileInfo(detayPath);
                    if (files.Exists)
                        files.Delete();
                }
            }
            _newsRepository.Delete(id);
            _newsRepository.Save();
            TempData["Bilgi"] = "Haber başarıyla silindi";
            return RedirectToAction("Index", "Haber");
        }

        [HttpGet]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Duzenle(int id)
        {
            News gelenHaber = _newsRepository.GetById(id);
            var gelenEtiket = gelenHaber.Tags.Select(x => x.Name).ToArray();
            HaberEtiketViewModel model = new HaberEtiketViewModel
            {
                News = gelenHaber,
                Tags = _tagRepository.GetAll(),
                GelenEtiketler = gelenEtiket
            };
            StringBuilder birlestir = new StringBuilder();
            foreach (var etiket in model.GelenEtiketler)
            {
                birlestir.Append(etiket.ToString());
                birlestir.Append(",");
            }
            model.TagName = birlestir.ToString();
            if (gelenHaber == null)
                throw new Exception("Haber bulunamadı!");
            SetKategoriListele();
            return View(model);
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Duzenle(News haber, HttpPostedFileBase VitrinResim, IEnumerable<HttpPostedFileBase> DetayResim,string EtiketAdi)
        {
            News gelenHaber = _newsRepository.GetById(haber.Id);
            gelenHaber.Content = haber.Content;
            gelenHaber.Description = haber.Description;
            gelenHaber.Title = haber.Title;
            gelenHaber.IsActive = haber.IsActive;
            gelenHaber.CategoryId = haber.CategoryId;

            if(VitrinResim!=null)
            {
                string dosyaAdi = gelenHaber.ShowcasePicture;
                string dosyaYolu = Server.MapPath(dosyaAdi);
                FileInfo dosya = new FileInfo(dosyaYolu);
                if (dosya.Exists)
                    dosya.Delete();
                string dosya_adi = Guid.NewGuid().ToString().Replace("-", "");
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                //string tam_yol = "/External/Haber/" + dosya_adi + uzanti;
                //string tam_yol = "C:\\Users\\sinem\\Source\\Repos\\HaberSepeti\\Exter\\Haber\\" + dosya_adi + uzanti;
                string tam_yol = uploadpath + dosya_adi + uzanti;


                Request.Files[0].SaveAs(tam_yol);
                gelenHaber.ShowcasePicture = tam_yol;
            }

            string cokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if(cokluResim!="")
            {
                foreach (var detay in DetayResim)
                {
                    string dosya_adi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                    //string tamyol = "/External/Haber/" + dosya_adi + uzanti;
                    string tamyol = uploadpath + dosya_adi + uzanti;
                
                    detay.SaveAs(tamyol);
                    var img = new Picture {
                        PictureUrl = tamyol
                    };
                    img.NewsId = gelenHaber.Id;
                    _pictureRepository.Insert(img);
                    _pictureRepository.Save();
                }
            }
            _tagRepository.EtiketEkle(haber.Id, EtiketAdi);

            _newsRepository.Save();
            TempData["Bilgi"] = "Haber düzenleme işleminiz başarılı.";
            return RedirectToAction("Index", "Haber");
        }

        public ActionResult ResimSil(int id)
        {
            Picture dbResim = _pictureRepository.GetById(id);
            if (dbResim == null)
                throw new Exception("Resim bulunamadı!");
            string file_name = dbResim.PictureUrl;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
                file.Delete();
            _pictureRepository.Delete(id);
            _pictureRepository.Save();
            TempData["Bilgi"] = "Resim silme işleminiz başarılı!";
            return RedirectToAction("Index", "Haber");
        }
    }
}
using HaberSepeti.Admin.Class;
using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;

namespace HaberSepeti.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderController(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int sayfa=1)
        {
            int sayfaBoyutu = 5;
            var SliderListesi = _sliderRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            return View(SliderListesi);
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {

            return View();
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Ekle(Slider slider, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null && ResimURL.ContentLength>0)
                {
                    string Dosya = Guid.NewGuid().ToString().Replace("-", "");
                    string Uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string ResimYolu = "C:\\Users\\sinem\\Source\\Repos\\HaberSepeti\\Exter\\Slider\\" + Dosya + Uzanti;
                    ResimURL.SaveAs(ResimYolu);
                    slider.ResimURL = ResimYolu;
                }
                _sliderRepository.Insert(slider);
                try
                {
                    _sliderRepository.Save();
                    TempData["Bilgi"] = "Slider ekleme işleminiz başarılı.";
                    return RedirectToAction("Index", "Slider");
                }
                catch (Exception)
                {
                    TempData["Bilgi"] = "Slider ekleme işleminiz başarısız!";
                    return RedirectToAction("Index", "Slider");
                }
            }        
            TempData["Bilgi"] = "Slider ekleme işleminiz başarısız!";
            return RedirectToAction("Index", "Slider");
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int Id)
        {
            var gelenSlider = _sliderRepository.GetById(Id);
            if (gelenSlider == null)
                TempData["Bilgi"] = "Slider bulunamadı!";
            return View(gelenSlider);
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Duzenle(Slider slider, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                Slider dbSlider = _sliderRepository.GetById(slider.Id);
                dbSlider.Aciklama = slider.Aciklama;
                dbSlider.AktifMi = slider.AktifMi;
                dbSlider.Baslik = slider.Baslik;
                dbSlider.URL = slider.URL;
                if(ResimURL != null && ResimURL.ContentLength > 0)
                {
                    if(dbSlider.ResimURL != null)
                    {
                        string url = dbSlider.ResimURL;
                        string resimPath = Server.MapPath(url);
                        FileInfo img = new FileInfo(resimPath);
                        if (img.Exists)
                        {
                            img.Delete();
                        }                        
                    }
                    string Dosya = Guid.NewGuid().ToString().Replace("-", "");
                    string Uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string ResimYolu = "C:\\Users\\sinem\\Source\\Repos\\HaberSepeti\\Exter\\Slider\\" + Dosya + Uzanti;
                    ResimURL.SaveAs(ResimYolu);
                    dbSlider.ResimURL = ResimYolu;
                }
                try
                {
                    _sliderRepository.Save();
                    TempData["Bilgi"] = "Slider düzenleme işleminiz başarılı.";
                    return RedirectToAction("Index", "Slider");
                }
                catch (Exception)
                {
                    TempData["Bilgi"] = "Slider düzenleme işleminiz başarısız!";
                    return RedirectToAction("Index", "Slider");
                }
            }                    
            TempData["Bilgi"] = "Slider düzenleme işleminiz başarısız!";
            return RedirectToAction("Index", "Slider");
        }

        [LoginFilter]
        public ActionResult Sil(int Id)
        {
            Slider dbSlider = _sliderRepository.GetById(Id);
            if (dbSlider == null)
            {
                TempData["Bilgi"] = "Slider bulunamadı!";
                return RedirectToAction("Index", "Slider");
            }
            if(dbSlider.ResimURL!=null)
            {
                string resimUrl = dbSlider.ResimURL;
                string resimPath = Server.MapPath(resimUrl);
                FileInfo img = new FileInfo(resimPath);
                if (img.Exists)
                {
                    img.Delete();
                }
            }
            _sliderRepository.Delete(Id);
            try
            {
                _sliderRepository.Save();
                TempData["Bilgi"] = "Slider silme işleminiz başarılı.";
                return RedirectToAction("Index", "Slider");
            }
            catch (Exception)
            {
                TempData["Bilgi"] = "Slider silme işleminiz başarısız!";
                return RedirectToAction("Index", "Slider");
            }                       
        }
    }
}
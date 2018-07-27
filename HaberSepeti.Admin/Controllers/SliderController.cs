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
                if (ResimURL.ContentLength>0)
                {
                    string Dosya = Guid.NewGuid().ToString().Replace("-", "");
                    string Uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string ResimYolu = "/External/Slider/" + Dosya + Uzanti;
                    ResimURL.SaveAs(Server.MapPath(ResimYolu));
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
    }
}
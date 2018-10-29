using HaberSepeti.Admin.Class;
using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using HaberSepeti.Data.Entities;
using System.Configuration;

namespace HaberSepeti.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderRepository _sliderRepository;
        string uploadpathSlider = ConfigurationManager.AppSettings["UploadPathSlider"].ToString();

        public SliderController(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int page=1)
        {
            int pageSize = 5;
            var sliderList = _sliderRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return View(sliderList);
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Add(Slider slider, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                if (PictureURL != null && PictureURL.ContentLength>0)
                {
                    string file = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string path = uploadpathSlider + file + extension;
                    PictureURL.SaveAs(path);
                    slider.PictureURL = path;
                }
                _sliderRepository.Insert(slider);
                try
                {
                    _sliderRepository.Save();
                    TempData["Message"] = "Slider ekleme işleminiz başarılı.";
                    return RedirectToAction("Index", "Slider");
                }
                catch (Exception)
                {
                    TempData["Message"] = "Slider ekleme işleminiz başarısız!";
                    return RedirectToAction("Index", "Slider");
                }
            }        
            TempData["Bilgi"] = "Slider ekleme işleminiz başarısız!";
            return RedirectToAction("Index", "Slider");
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Edit(int Id)
        {
            var slider = _sliderRepository.GetById(Id);
            if (slider == null)
                TempData["Message"] = "Slider bulunamadı!";
            return View(slider);
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Edit(Slider slider, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                Slider dbSlider = _sliderRepository.GetById(slider.Id);
                dbSlider.Description = slider.Description;
                dbSlider.IsActive = slider.IsActive;
                dbSlider.Title = slider.Title;
                dbSlider.URL = slider.URL;
                if(PictureURL != null && PictureURL.ContentLength > 0)
                {
                    if(dbSlider.PictureURL != null)
                    {
                        string url = dbSlider.PictureURL;
                        FileInfo img = new FileInfo(url);
                        if (img.Exists)
                        {
                            img.Delete();
                        }                        
                    }
                    string file = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string path = uploadpathSlider + file + extension;
                    PictureURL.SaveAs(path);
                    dbSlider.PictureURL = path;
                }
                try
                {
                    _sliderRepository.Save();
                    TempData["Message"] = "Slider düzenleme işleminiz başarılı.";
                    return RedirectToAction("Index", "Slider");
                }
                catch (Exception)
                {
                    TempData["Message"] = "Slider düzenleme işleminiz başarısız!";
                    return RedirectToAction("Index", "Slider");
                }
            }                    
            TempData["Message"] = "Slider düzenleme işleminiz başarısız!";
            return RedirectToAction("Index", "Slider");
        }

        [LoginFilter]
        public ActionResult Delete(int Id)
        {
            Slider dbSlider = _sliderRepository.GetById(Id);
            if (dbSlider == null)
            {
                TempData["Message"] = "Slider bulunamadı!";
                return RedirectToAction("Index", "Slider");
            }
            if(dbSlider.PictureURL!=null)
            {
                string pictureUrl = dbSlider.PictureURL;
                FileInfo img = new FileInfo(pictureUrl);
                if (img.Exists)
                {
                    img.Delete();
                }
            }
            _sliderRepository.Delete(Id);
            try
            {
                _sliderRepository.Save();
                TempData["Message"] = "Slider silme işleminiz başarılı.";
                return RedirectToAction("Index", "Slider");
            }
            catch (Exception)
            {
                TempData["Message"] = "Slider silme işleminiz başarısız!";
                return RedirectToAction("Index", "Slider");
            }                       
        }
    }
}
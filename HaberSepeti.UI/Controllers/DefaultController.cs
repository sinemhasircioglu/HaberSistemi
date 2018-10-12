using HaberSepeti.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HaberSepeti.UI.Models.ViewModels;

namespace HaberSepeti.UI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHaberRepository _haberRepository;
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IYorumRepository _yorumRepository;
        private readonly ISliderRepository _sliderRepository;

        public DefaultController(IHaberRepository haberRepository, IKategoriRepository kategoriRepository, IYorumRepository yorumRepository, ISliderRepository sliderRepository)
        {
            _haberRepository = haberRepository;
            _kategoriRepository = kategoriRepository;
            _yorumRepository = yorumRepository;
            _sliderRepository = sliderRepository;
        }

        public ActionResult Index()
        {
            var haberler = _haberRepository.GetAll().OrderByDescending(x => x.Id);
            return View(haberler);
        }

        public ActionResult Arama(string kelime, int sayfa = 1)
        {
            int sayfaBoyutu = 4;
            var arananHaberler = _haberRepository.GetMany(x => x.Baslik.Contains(kelime) || x.Aciklama.Contains(kelime)).OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            ViewBag.Aranan = kelime;
            return View(arananHaberler);
        }

        public ActionResult Sidebar()
        {
            SidebarViewModel model = new SidebarViewModel
            {
                SonHaberler = _haberRepository.GetAll().OrderByDescending(x => x.EklenmeTarihi).Take(4),
                SonYorumlar = _yorumRepository.GetAll().OrderByDescending(x => x.EklenmeTarihi).Take(4),
                PopulerHaberler = _haberRepository.GetAll().OrderByDescending(x => x.Okunma).Take(4),
            };
            return View(model);
        }

        public ActionResult SliderGoruntule()
        {
            SliderViewModel model = new SliderViewModel
            {
                Slider = _sliderRepository.GetAll().OrderByDescending(x => x.Id).Take(3),
                Haber = _haberRepository.GetAll().OrderByDescending(x => x.Id).Take(2),
            };
            return View(model);
        }

        public ActionResult Detay(int Id)
        {
            var haber = _haberRepository.Get(x => x.Id == Id);
            if (haber == null)
            {
                return HttpNotFound();
            }
            return View(haber);
        }


        public ActionResult OkunmaArttir(int Id)
        {
            var haber = _haberRepository.Get(x => x.Id == Id);
            haber.Okunma += 1;
            _haberRepository.Save();
            return View();
        }
    }
}
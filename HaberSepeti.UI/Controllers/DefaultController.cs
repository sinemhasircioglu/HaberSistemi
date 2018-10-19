﻿using HaberSepeti.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HaberSepeti.UI.Models.ViewModels;
using HaberSepeti.Data.Entities;

namespace HaberSepeti.UI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly INewsRepository _haberRepository;
        private readonly ICategoryRepository _kategoriRepository;
        private readonly ICommentRepository _yorumRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly ITagRepository _etiketRepository;
        private readonly IUserRepository _kullaniciRepository;

        public DefaultController(INewsRepository haberRepository, ICategoryRepository kategoriRepository, ICommentRepository yorumRepository, ISliderRepository sliderRepository,
            ITagRepository etiketRepository, IUserRepository kullaniciRepository)
        {
            _haberRepository = haberRepository;
            _kategoriRepository = kategoriRepository;
            _yorumRepository = yorumRepository;
            _sliderRepository = sliderRepository;
            _etiketRepository = etiketRepository;
            _kullaniciRepository = kullaniciRepository;
        }

        public ActionResult Index()
        {
            var haberler = _haberRepository.GetAll().OrderByDescending(x => x.Id);
            return View(haberler);
        }

        public ActionResult Arama(string kelime, int sayfa = 1)
        {
            int sayfaBoyutu = 4;
            var arananHaberler = _haberRepository.GetMany(x => x.Title.Contains(kelime) || x.Content.Contains(kelime)).OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            ViewBag.Aranan = kelime;
            return View(arananHaberler);
        }

        public ActionResult Sidebar()
        {
            SidebarViewModel model = new SidebarViewModel
            {
                SonHaberler = _haberRepository.GetAll().OrderByDescending(x => x.CreatedDate).Take(4),
                SonYorumlar = _yorumRepository.GetAll().OrderByDescending(x => x.CreatedDate).Take(4),
                PopulerHaberler = _haberRepository.GetAll().OrderByDescending(x => x.Reads).Take(4),
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
            DetayViewModel model = new DetayViewModel
            {
                Haber = _haberRepository.Get(x => x.Id == Id),
                Yorumlar = _yorumRepository.GetMany(x => x.NewsId == Id),
                IliskiliHaberler = _haberRepository.GetAll().OrderByDescending(x => x.Id).Take(3),
                Slider = _sliderRepository.Get(x => x.Id == Id),
            };
            return View(model);
        }

        public ActionResult YorumYap(string icerik, int haberId)
        {
            var sessionControl = HttpContext.Session["KullaniciId"];
            User kullanici = _kullaniciRepository.Get(x => x.Id == (int)sessionControl);
            if (icerik != null)
            {
                Comment yeniyorum = new Comment
                {
                    Content = icerik,
                    NewsId = haberId,
                    UserId = kullanici.Id,
                    CreatedDate = DateTime.Now,
                };
                try
                {
                    _yorumRepository.Insert(yeniyorum);
                    _yorumRepository.Save();
                    return RedirectToAction("Detay/" + haberId, "Default");
                }
                catch
                {
                    return RedirectToAction("Detay/" + haberId, "Default");
                }
            }
            return RedirectToAction("Detay/" + haberId, "Default");
        }

        public ActionResult OkunmaArttir(int haberId)
        {
            var haber = _haberRepository.GetById(haberId);
            haber.Reads += 1;
            _haberRepository.Save();
            return View();

        }

        public ActionResult Etiketler()
        {
            var etiketler = _etiketRepository.GetAll().Take(30).ToList();
            return View(etiketler);
        }

        public ActionResult EtiketeGoreListele(string etiketAdi, int sayfa = 1)
        {
            int sayfaBoyutu = 4;
            var etiketHaberler = _haberRepository.GetAll().Where(x => x.Tags.Any(y => y.Name == etiketAdi)).OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            ViewBag.EtiketAd = etiketAdi;
            return View(etiketHaberler);
        }

    }
}
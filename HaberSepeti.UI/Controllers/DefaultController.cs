using HaberSepeti.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HaberSepeti.UI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHaberRepository _haberRepository;
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IYorumRepository _yorumRepository;

        public DefaultController(IHaberRepository haberRepository, IKategoriRepository kategoriRepository, IYorumRepository yorumRepository)
        {
            _haberRepository = haberRepository;
            _kategoriRepository = kategoriRepository;
            _yorumRepository = yorumRepository;
        }

        public ActionResult Index(int sayfa = 1)
        {
            int sayfaBoyutu = 4;
            var haberler = _haberRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(sayfa, sayfaBoyutu);
            return View(haberler);
        }

        public ActionResult Arama(string Ara = null)
        {

            var aranan = _haberRepository.GetMany(x => x.Baslik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(x => x.EklenmeTarihi));
        }

        public ActionResult Kategoriler()
        {
            var kategoriler = _kategoriRepository.GetAll();
            return View(kategoriler);
        }

        public ActionResult PopulerHaberler()
        {
            var populer = _haberRepository.GetAll().OrderByDescending(x => x.Okunma).Take(5);
            return View(populer);
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

        public ActionResult SonYorumlar()
        {
            var yorumlar = _yorumRepository.GetAll().OrderByDescending(x => x.Id).Take(5);
            return View(yorumlar);
        }
    }
}
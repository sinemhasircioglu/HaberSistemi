using HaberSepeti.Admin.Class;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HaberSepeti.Admin.CustomFilter;

namespace HaberSepeti.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 5;
            return View(_categoryRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(page, pageSize));
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Add()
        {
            SetCategoryList();
            return View();
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Add(Category category)
        {
            _categoryRepository.Insert(category);
            _categoryRepository.Save();
            TempData["Message"] = "Kategori ekleme işleminiz başarılı";
            return RedirectToAction("Index", "Category");
        }

        public void SetCategoryList()
        {
            var categoryList = _categoryRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Category = categoryList;
        }

        [LoginFilter]
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetById(id);
            if (category == null)
                TempData["Message"] = "Kategori bulunamadı!";
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
            TempData["Message"] = "Kategori başarıyla silindi";
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);
            if (category == null)
                TempData["Message"] = "Kategori bulunamadı!";
            SetCategoryList();
            return View(category);
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult Edit(Category category)
        {
            Category dbCategory = _categoryRepository.GetById(category.Id);
            dbCategory.IsActive = category.IsActive;
            dbCategory.Name = category.Name;
            dbCategory.URL = category.URL;
            dbCategory.ParentId = category.ParentId;
            _categoryRepository.Save();
            TempData["Message"] = "Kategori düzenleme işleminiz başarılı.";
            return RedirectToAction("Index", "Category");
        }
    }
}
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
    public class NewsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly ITagRepository _tagRepository;
        string uploadpath = ConfigurationManager.AppSettings["UploadPathHaber"].ToString();


        public NewsController(IUserRepository userRepository, INewsRepository newsRepository, ICategoryRepository categoryRepository, IPictureRepository pictureRepository, 
            ITagRepository tagRepository)
        {
            _userRepository = userRepository;
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
            _pictureRepository = pictureRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 5;
            var newsList = _newsRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return View(newsList);
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
        public ActionResult Add(News news, int CategoryId, HttpPostedFileBase ShowcasePicture, IEnumerable<HttpPostedFileBase> DetailPicture, string Tag)
        {
            var sessionControl = HttpContext.Session["UserId"];
            User user = _userRepository.Get(x => x.Id == (int)sessionControl);
            news.UserId = user.Id;
            news.CategoryId = CategoryId;
            if (ShowcasePicture != null)
            {
                string file = Guid.NewGuid().ToString().Replace("-", "");
                string extension = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string path = uploadpath + file + extension;
                Request.Files[0].SaveAs(path);
                news.ShowcasePicture = path.Substring(46);
            }
            _newsRepository.Insert(news);
            _newsRepository.Save();

            _tagRepository.AddTag(news.Id, Tag);

            string multiplePicture = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if (multiplePicture != "")
            {
                foreach (var file in DetailPicture)
                {
                    if (file.ContentLength > 0)
                    {
                        string fileName = Guid.NewGuid().ToString().Replace("-", "");
                        string fileExtension = System.IO.Path.GetExtension(Request.Files[1].FileName);
                        //string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                        //string tamYol = "C:\\Users\\sinem\\Source\\Repos\\HaberSepeti\\Exter\\Haber\\" + dosyaAdi + uzanti; // doğru olan
                        string filePath = uploadpath + fileName + fileExtension;


                        //file.SaveAs(Server.MapPath(tamYol));
                        file.SaveAs(filePath);
                        var image = new Picture
                        {
                            PictureUrl = filePath.Substring(46)
                        };
                        image.NewsId = news.Id;
                        _pictureRepository.Insert(image);
                        _pictureRepository.Save();
                    }
                }
            }
            TempData["Message"] = "Haber ekleme işleminiz başarılı";
            return RedirectToAction("Index", "News");
        }

        public void SetCategoryList(object category = null)
        {
            var categoryList = _categoryRepository.GetMany(x => x.ParentId == 0).ToList();
            ViewBag.Category = categoryList;
        }

        [LoginFilter]
        public ActionResult Delete(int id)
        {
            News dbNews = _newsRepository.GetById(id);
            var dbDetailPicture = _pictureRepository.GetMany(x => x.NewsId == id);
            if (dbNews == null)
                throw new Exception("Haber Bulunamadı!");

            string file_name = dbNews.ShowcasePicture;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists) // dosyanın varlığı kontrol ediliyor. fiziksel olarak varsa siliniyor.
                file.Delete();
            if (dbDetailPicture != null)
            {
                foreach (var item in dbDetailPicture)
                {
                    string detailPath = Server.MapPath(item.PictureUrl);
                    FileInfo files = new FileInfo(detailPath);
                    if (files.Exists)
                        files.Delete();
                }
            }
            _newsRepository.Delete(id);
            _newsRepository.Save();
            TempData["Message"] = "Haber başarıyla silindi";
            return RedirectToAction("Index", "News");
        }

        [HttpGet]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {
            News news = _newsRepository.GetById(id);
            var tags = news.Tags.Select(x => x.Name).ToArray();
            NewsTagViewModel model = new NewsTagViewModel
            {
                News = news,
                Tags = _tagRepository.GetAll(),
                AddedTags = tags
            };
            StringBuilder sb = new StringBuilder();
            foreach (var t in model.AddedTags)
            {
                sb.Append(t.ToString());
                sb.Append(",");
            }
            model.TagName = sb.ToString();
            if (news == null)
                throw new Exception("Haber bulunamadı!");
            SetCategoryList();
            return View(model);
        }

        [HttpPost]
        [LoginFilter]
        [ValidateInput(false)]
        public ActionResult Edit(News news, HttpPostedFileBase ShowcasePicture, IEnumerable<HttpPostedFileBase> DetailPicture, string TagName)
        {
            News dbNews = _newsRepository.GetById(news.Id);
            dbNews.Content = news.Content;
            dbNews.Description = news.Description;
            dbNews.Title = news.Title;
            dbNews.IsActive = news.IsActive;
            dbNews.CategoryId = news.CategoryId;

            if(ShowcasePicture != null)
            {
                string fileName = dbNews.ShowcasePicture;
                string filePath = Server.MapPath(fileName);
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                    file.Delete();
                string file_name = Guid.NewGuid().ToString().Replace("-", "");
                string file_extension = System.IO.Path.GetExtension(Request.Files[0].FileName);
                //string tam_yol = "/External/Haber/" + dosya_adi + uzanti;
                //string tam_yol = "C:\\Users\\sinem\\Source\\Repos\\HaberSepeti\\Exter\\Haber\\" + dosya_adi + uzanti;
                string file_path = uploadpath + file_name + file_extension;

                Request.Files[0].SaveAs(file_path);
                dbNews.ShowcasePicture = file_path;
            }

            string multiplePicture = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if(multiplePicture != "")
            {
                foreach (var detail in DetailPicture)
                {
                    string file = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = System.IO.Path.GetExtension(Request.Files[1].FileName);
                    //string tamyol = "/External/Haber/" + dosya_adi + uzanti;
                    string path = uploadpath + file + extension;
                
                    detail.SaveAs(path);
                    var img = new Picture {
                        PictureUrl = path
                    };
                    img.NewsId = dbNews.Id;
                    _pictureRepository.Insert(img);
                    _pictureRepository.Save();
                }
            }
            _tagRepository.AddTag(news.Id, TagName);

            _newsRepository.Save();
            TempData["Message"] = "Haber düzenleme işleminiz başarılı.";
            return RedirectToAction("Index", "News");
        }

        public ActionResult DeletePicture(int id)
        {
            Picture dbPicture = _pictureRepository.GetById(id);
            if (dbPicture == null)
                throw new Exception("Resim bulunamadı!");
            string file_name = dbPicture.PictureUrl;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
                file.Delete();
            _pictureRepository.Delete(id);
            _pictureRepository.Save();
            TempData["Message"] = "Resim silme işleminiz başarılı!";
            return RedirectToAction("Index", "News");
        }
    }
}
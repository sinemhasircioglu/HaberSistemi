using HaberSepeti.Core.Infrastructure;
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
        private readonly INewsRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;

        public DefaultController(INewsRepository newsRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, ISliderRepository sliderRepository,
            ITagRepository tagRepository, IUserRepository userRepository)
        {
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _sliderRepository = sliderRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            var news = _newsRepository.GetAll().OrderByDescending(x => x.Id);
            return View(news);
        }

        public ActionResult Search(string word, int page = 1)
        {
            int pageSize = 4;
            var news = _newsRepository.GetMany(x => x.Title.Contains(word) || x.Content.Contains(word)).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            ViewBag.Word = word;
            return View(news);
        }

        public ActionResult Sidebar()
        {
            SidebarViewModel model = new SidebarViewModel
            {
                LatestNews = _newsRepository.GetAll().OrderByDescending(x => x.CreatedDate).Take(4),
                LatestComments = _commentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Take(4),
                PopularNews = _newsRepository.GetAll().OrderByDescending(x => x.Reads).Take(4),
            };
            return View(model);
        }

        public ActionResult Slider()
        {
            SliderViewModel model = new SliderViewModel
            {
                Slider = _sliderRepository.GetAll().OrderByDescending(x => x.Id).Take(3),
                News = _newsRepository.GetAll().OrderByDescending(x => x.Id).Take(2),
            };
            return View(model);
        }

        public ActionResult Detail(int Id)
        {
            DetailViewModel model = new DetailViewModel
            {
                News = _newsRepository.Get(x => x.Id == Id),
                Comments = _commentRepository.GetMany(x => x.NewsId == Id),
                RelatedNews = _newsRepository.GetAll().OrderByDescending(x => x.Id).Take(3),
                Slider = _sliderRepository.Get(x => x.Id == Id),
            };
            return View(model);
        }

        public ActionResult Comment(string content, int newsId)
        {
            var sessionControl = HttpContext.Session["UserId"];
            User user = _userRepository.Get(x => x.Id == (int)sessionControl);
            if (content != null)
            {
                Comment newComment = new Comment
                {
                    Content = content,
                    NewsId = newsId,
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                };
                try
                {
                    _commentRepository.Insert(newComment);
                    _commentRepository.Save();
                    return RedirectToAction("Detail/" + newsId, "Default");
                }
                catch
                {
                    return RedirectToAction("Detail/" + newsId, "Default");
                }
            }
            return RedirectToAction("Detail/" + newsId, "Default");
        }

        public ActionResult ReadIncrease(int newsId)
        {
            var news = _newsRepository.GetById(newsId);
            news.Reads += 1;
            _newsRepository.Save();
            return View();

        }

        public ActionResult Tags()
        {
            var tags = _tagRepository.GetAll().Take(30).ToList();
            return View(tags);
        }

        public ActionResult ListByTag(string tagName, int page = 1)
        {
            int pageSize = 4;
            var news = _newsRepository.GetAll().Where(x => x.Tags.Any(y => y.Name == tagName)).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            ViewBag.TagName = tagName;
            return View(news);
        }

    }
}
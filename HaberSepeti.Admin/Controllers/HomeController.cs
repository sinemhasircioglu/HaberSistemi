using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Admin.Models.ViewModels;
using HaberSepeti.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INewsRepository _newsRepository;
        private readonly ICommentRepository _commentRepository;

        public HomeController(IUserRepository userRepository, INewsRepository newsRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _newsRepository = newsRepository;
            _commentRepository = commentRepository;
        }

        [LoginFilter]
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel()
            {
                TotalNews = _newsRepository.Count(),
                TotalReads = _newsRepository.GetAll().Sum(x => x.Reads),
                PassiveUsers = _userRepository.GetAll().ToList().Count(x => x.IsActive == false),
                ActiveUsers = _userRepository.GetAll().ToList().Count(x => x.IsActive == true),
                PassiveComments = _commentRepository.GetAll().ToList().Count(x => x.IsActive == false),
                ActiveComments = _commentRepository.GetAll().ToList().Count(x => x.IsActive == true),
            };
            return View(model);
        }
    }
}
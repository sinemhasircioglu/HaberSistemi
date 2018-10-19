using HaberSepeti.Admin.CustomFilter;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 5;
            return View(_commentRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(page, pageSize));
        }

        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            if (comment == null)
                TempData["Message"] = "Yorum bulunamadı!";
            _commentRepository.Delete(id);
            _commentRepository.Save();
            TempData["Message"] = "Yorum başarıyla silindi";
            return RedirectToAction("Index", "Comment");
        }

        public ActionResult Confirm(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            if (comment == null)
                TempData["Message"] = "Yorum bulunamadı!";
            comment.IsActive = true;
            _commentRepository.Save();
            TempData["Message"] = "Yoruma izin verildi";
            return RedirectToAction("Index", "Comment");
        }

        public ActionResult Reject(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            if (comment == null)
                TempData["Message"] = "Yorum bulunamadı!";
            comment.IsActive = false;
            _commentRepository.Save();
            TempData["Message"] = "Yorumun izni kaldırıldı";
            return RedirectToAction("Index", "Comment");
        }
    }
}
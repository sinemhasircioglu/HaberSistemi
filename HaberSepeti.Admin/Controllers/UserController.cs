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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index(int page = 1)
        {
            int pageSize = 5;
            var userList = _userRepository.GetAll().OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return View(userList);
        }

        public ActionResult Delete(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            _userRepository.Delete(id);
            _userRepository.Save();
            TempData["Message"] = "Kullanıcı başarıyla silindi";
            return RedirectToAction("Index", "User");
        }

        public ActionResult MakeAdmin(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            user.RoleId = 1;
            _userRepository.Save();
            TempData["Message"] = "Kullanıcı artık admin";
            return RedirectToAction("Index", "User");
        }

        public ActionResult MakeEditor(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            user.RoleId = 2;
            _userRepository.Save();
            TempData["Message"] = "Kullanıcı artık editör";
            return RedirectToAction("Index", "User");
        }

        public ActionResult MakeAuthor(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            user.RoleId = 4;
            _userRepository.Save();
            TempData["Message"] = "Kullanıcı artık yazar";
            return RedirectToAction("Index", "User");
        }

        public ActionResult MakeMember(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            user.RoleId = 3;
            _userRepository.Save();
            TempData["Message"] = "Kullanıcı artık sadece üye";
            return RedirectToAction("Index", "User");
        }
    }
}
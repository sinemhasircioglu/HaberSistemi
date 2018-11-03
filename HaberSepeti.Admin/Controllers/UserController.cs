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

        public ActionResult SetRole(int userId, int roleId)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            user.RoleId = roleId;
            _userRepository.Save();
            TempData["Message"] = "Kullanıcının rolü değiştirildi.";
            return RedirectToAction("Index", "User");
        }

    }
}
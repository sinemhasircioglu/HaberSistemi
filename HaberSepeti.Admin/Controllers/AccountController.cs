using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user) //giriş yap butonuna basınca çalışacak kodlar
        {
            var isRegister = _userRepository.GetMany(x => x.Email == user.Email && x.Password == user.Password && x.IsActive == true).SingleOrDefault();
            if(isRegister != null)
            {
                if(isRegister.Role.Name == "Admin")
                {
                    Session["UserId"] = isRegister.Id;
                    return RedirectToAction("Index","Home");
                }
                ViewBag.Message = "Yetkisiz kullanıcı";
                return View();
            }
            ViewBag.Message = "Kullanıcı bulunamadı";
            return View();
        }
    }
}
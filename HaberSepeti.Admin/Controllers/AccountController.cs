using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Controllers
{
    public class CaptchaResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

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
        public ActionResult Login(User user) 
        {
            var isRegister = _userRepository.GetMany(x => x.Email == user.Email && x.Password == user.Password && x.IsActive == true).SingleOrDefault();
            if(isRegister != null)
            {
                if(isRegister.Role.Name == "Admin" || isRegister.Role.Name == "Editör" || isRegister.Role.Name == "Yazar")
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

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, string PasswordAgain)
        {
            var isRegister = _userRepository.GetMany(x => x.Email == user.Email).SingleOrDefault();
            if (isRegister != null)
            {
                ViewBag.Message = "Bu email kullanılmış !";
                return View();
            }
            User newUser = new User
            {
                NameSurname = user.NameSurname,
                Email = user.Email,
                Password = user.Password,
                RoleId = 3,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };

            var recaptcha = Request.Form["g-recaptcha-response"];
            const string secretkey = "6LfO72kUAAAAANUJJDtClqUZ4n_ei8_bpK5DuHLs";
            var restUrl = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretkey, recaptcha);
            WebRequest request = WebRequest.Create(restUrl);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            JsonSerializer serializer = new JsonSerializer();
            CaptchaResult result = null;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string resultObject = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<CaptchaResult>(resultObject);
            }
            if (!result.Success)
            {
                ModelState.AddModelError("", "captcha mesajınız hatalı");
                if (result.ErrorCodes != null)
                {
                    ModelState.AddModelError("", result.ErrorCodes[0]);
                }
                return View();
            }
            else
            {
                if (PasswordAgain != null && user.Password == PasswordAgain)
                {
                    try
                    {
                        _userRepository.Insert(newUser);
                        _userRepository.Save();
                        return RedirectToAction("Login", "Account");
                    }
                    catch (Exception)
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Bu email kullanılmış !";
                    return View();
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            User dbUser = _userRepository.GetById(user.Id);
            dbUser.NameSurname = user.NameSurname;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            _userRepository.Save();
            TempData["Message"] = "Kullanıcı düzenleme işleminiz başarılı.";
            return RedirectToAction("Edit", "Account");
        }

        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                TempData["Message"] = "Kullanıcı bulunamadı!";
            return View(user);
        }

        [HttpPost]
        public ActionResult ChangePassword(User user, string oldPassword, string newPassword, string newPasswordAgain)
        {
            User dbUser = _userRepository.GetById(user.Id);
            if (oldPassword != null && oldPassword == dbUser.Password)
            {
                if (newPassword == newPasswordAgain && newPassword != null)
                {
                    dbUser.Password = newPassword;
                }
            }
            _userRepository.Save();
            TempData["Message"] = "Şifre değiştirme işleminiz başarılı.";
            return RedirectToAction("Edit/" + user.Id, "Account");
        }
    }
}
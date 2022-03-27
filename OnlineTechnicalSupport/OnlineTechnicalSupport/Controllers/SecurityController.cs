using OnlineTechnicalSupport.Models.Context;
using OnlineTechnicalSupport.Models.Data;
using OnlineTechnicalSupport.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineTechnicalSupport.Models.Classes;
using System.Web.Helpers;
using System.IO;
using System.Web.UI;
using System.Net.Mail;
using System.Net;
using System.Web.Routing;
using System.Data.Entity.Validation;

namespace OnlineTechnicalSupport.Controllers
{
    public class SecurityController : Controller
    {
        OTSContext db = new OTSContext();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLoginViewModel model)
        {
            if (model.UserType == 1)
            {
                var user = db.Customers.FirstOrDefault(u => (u.LoginName == model.LoginNameOrEmail || u.Email == model.LoginNameOrEmail) && u.LoginPassword == model.LoginPassword);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["AuthenticatedCustomer"] = user.Email;
                    user.LastLoginDate = DateTime.Now;
                    user.ReLoginPassword = user.LoginPassword;
                    db.SaveChanges();
                    //SetCookies("AuthenticatedConsumer", user.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.LoginError = "Kullanıcı adı veya şifre hatalı.";
                }
            }
            else if (model.UserType == 2)
            {
                var user = db.Managers.FirstOrDefault(u => (u.LoginName == model.LoginNameOrEmail || u.Email == model.LoginNameOrEmail) && u.LoginPassword == model.LoginPassword);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["AuthenticatedManager"] = user.Email;
                    user.LastLoginDate = DateTime.Now;
                    user.ReLoginPassword = user.LoginPassword;
                    db.SaveChanges();
                    //SetCookies("AuthenticatedManager", user.Email);
                    return RedirectToAction("Index", "Manager");
                }
                else
                {
                    ViewBag.LoginError = "Kullanıcı adı veya şifre hatalı.";
                }
            }
            else
            {
                ViewBag.LoginError = "Kullanıcı adı veya şifre hatalı.";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(Customer model)
        {
            if(ModelState.IsValid)
            {
                var checkUser = db.Customers.FirstOrDefault(x => x.Email == model.Email || x.LoginName == model.LoginName);
                if(checkUser != null)
                {
                    ViewBag.Result = "<script>alert('Girmiş olduğunuz eposta adresi veya kullanıcı adı daha önce kullanılmıştır.')</script>";
                    return View();
                }
                if(model.ReLoginPassword != model.LoginPassword)
                {
                    ViewBag.Result = "<script>alert('Şifreniz hatalıdır. Tekrar deneyiniz.')</script>";
                    return View();
                }
                Customer user = new Customer();
                user.Email = model.Email;
                user.LoginName = model.LoginName;
                user.LoginPassword = model.LoginPassword;
                user.ReLoginPassword = model.ReLoginPassword;
                user.DateOfCreate = DateTime.Now;
                user.ConfirmedEmail = "No";
                user.Activated = "No";

                db.Customers.Add(user);
                db.SaveChanges();

                SelectedUser u = new SelectedUser();
                u.email = user.Email;
                u.loginName = user.LoginName;

                CreateRequest(u, "vrf");
                return RedirectToAction("RequestInfo", new { reason = "vrf", email = u.email.ToString() });
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string userNameOrEmail)
        {
            var consumer = db.Customers.FirstOrDefault(x => x.LoginName.Equals(userNameOrEmail) || x.Email.Equals(userNameOrEmail));
            var manager = db.Managers.FirstOrDefault(x => x.LoginName.Equals(userNameOrEmail) || x.Email.Equals(userNameOrEmail));
            if (consumer != null && consumer.ConfirmedEmail == "Yes")
            {
                SelectedUser user = new SelectedUser();
                user.email = consumer.Email;
                user.loginName = consumer.LoginName;
                CreateRequest(user, "pwr");
                return RedirectToAction("RequestInfo", new { reason = "pwr", email = user.email.ToString() });
            }
            else if (manager != null && manager.ConfirmedEmail == "Yes")
            {
                SelectedUser user = new SelectedUser();
                user.email = manager.Email;
                user.loginName = manager.LoginName;
                CreateRequest(user, "pwr");
                return RedirectToAction("RequestInfo", new { reason = "pwr", email = user.email.ToString() });
            }
            ViewBag.Result = "Hesap bulunamadı, tekrar deneyiniz.";
            return View();
        }

        public class SelectedUser
        {
            public string email { get; set; }
            public string loginName { get; set; }
        }

        [AllowAnonymous]
        public void CreateRequest(SelectedUser user, string reason)
        {
            CryptoServices cs = new CryptoServices();
            string code = cs.CreateCode(8);

            AccountSecurityRequest u = new AccountSecurityRequest();
            if(reason == "vrf")
                u.Reason = "Email Verification"; 
            else if (reason == "pwr")
                u.Reason = "Password Reset";
            u.Expires = DateTime.Now.AddDays(1);
            u.Email = user.email;
            u.VerificationCode = code;
            
            db.AccountSecurityRequests.Add(u);
            db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 587;
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "gmail adresiniz"; // gmail stmp ayarı yapmanız gerekebilir.
                    WebMail.Password = "gmail adresinizin şifresi";

                    if (reason == "vrf")
                    {
                        StringWriter stringWriter = new StringWriter();
                        using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "https://localhost:44320/Security/EmailVerification?confirmCode=" + code);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write("Bağlantı");
                            writer.RenderEndTag();
                        }

                        WebMail.Send(user.email, "Hesap Doğrulama", "Merhaba " + user.loginName + ", Hesabınızı doğrulamak için tıklayın: " + stringWriter.ToString());                }
                    else if (reason == "pwr")
                    {
                        StringWriter stringWriter = new StringWriter();
                        using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, "https://localhost:44320/Security/PasswordReset?confirmCode=" + code);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write("Bağlantı");
                            writer.RenderEndTag();
                        }

                        WebMail.Send(user.email, "Parola Değiştirme İsteği", "Merhaba " + user.loginName + ", Parolanızı değiştirmek için tıklayın: " + stringWriter.ToString());
                    }
        }

        [AllowAnonymous]
        public ActionResult PasswordReset(string confirmCode)
        {
            var request = db.AccountSecurityRequests.FirstOrDefault(x => x.VerificationCode == confirmCode);
            if(request == null)
                Response.Redirect("/Home/Index");
            if(DateTime.Now > request.Expires)
                Response.Redirect("/Security/Login");
            ViewBag.Email = request.Email;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PasswordReset(string confirmCode, string password, string rePassword)
        {
            var request = db.AccountSecurityRequests.FirstOrDefault(x => x.VerificationCode == confirmCode);
            if (request == null)
                Response.Redirect("/Home/Index");
            if (DateTime.Now > request.Expires)
                Response.Redirect("/Security/Login");
            if (password.Length < 8)
            {
                ViewBag.Result = "Parola uzunluğu en az 8 karakter olmalıdır.";
                return View();
            }
            else if (password != rePassword)
            {
                ViewBag.Result = "Parola girişi hatalı, tekrar deneyiniz.";
                return View();
            }
            var customer = db.Customers.FirstOrDefault(x => x.Email.Equals(request.Email));
            var manager = db.Managers.FirstOrDefault(x => x.Email.Equals(request.Email));
            if(customer != null)
            {
                customer.LoginPassword = password;
                customer.ReLoginPassword = password;
                db.SaveChanges();
                return RedirectToAction("Success", new { code = confirmCode });
            }
            else if (manager != null)
            {
                manager.LoginPassword = password;
                manager.ReLoginPassword = password;
                db.SaveChanges();
                return RedirectToAction("Success", new { code = confirmCode });
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult RequestInfo(string reason, string email)
        {
            if (reason == "vrf")
            {
                ViewBag.ttl = "Eposta Doğrulama";
                ViewBag.titl = email + " epostanızı doğrulamak için ileti gönderdik.";
            }
            else if (reason == "pwr")
            {
                ViewBag.ttl = "Parola Yenileme";
                ViewBag.titl = "Parolanızı yenilemek için " + email + " epostanıza ileti gönderdik.";
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Success(string code)
        {
            var request = db.AccountSecurityRequests.FirstOrDefault(x => x.VerificationCode == code);
            if (request == null)
                Response.Redirect("/Home/Index");
            if (DateTime.Now > request.Expires)
                Response.Redirect("/Home/Index");
            if (request.Reason == "Email Verification")
            {
                ViewBag.ttl = "Eposta Doğrulama";
                ViewBag.titl = request.Email + " epostanız doğrulandı, şimdi giriş yapabilirsiniz.";
            }   
            else if (request.Reason == "Password Reset")
            {
                ViewBag.ttl = "Parola Değiştirme";
                ViewBag.titl = request.Email + " hesabınız için parolanızı değiştirdiniz. Şimdi giriş yapabilirsiniz.";
            }
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult EmailVerification(string confirmCode)
        {
            var request = db.AccountSecurityRequests.FirstOrDefault(x => x.VerificationCode == confirmCode);
            if (request == null)
                Response.Redirect("/Home/Index");
            if (DateTime.Now > request.Expires)
                Response.Redirect("/Security/Login");
            var customer = db.Customers.FirstOrDefault(x => x.Email.Equals(request.Email));
            var manager = db.Managers.FirstOrDefault(x => x.Email.Equals(request.Email));
            if (customer != null)
            {
                customer.ConfirmedEmail = "Yes";
                customer.Activated = "Yes";
                customer.ReLoginPassword = customer.LoginPassword;
                db.SaveChanges();
                return RedirectToAction("Success", new { code = confirmCode });
            }
            else if (manager != null)
            {
                manager.ConfirmedEmail = "Yes";
                manager.Activated = "Yes";
                manager.ReLoginPassword = manager.LoginPassword;
                db.SaveChanges();
                return RedirectToAction("Success", new { code = confirmCode });
            }
            return RedirectToAction("Index", "Home");
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public void SetCookies(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key, value);
            HttpContext.Response.Cookies.Add(cookie);
        }

    }
}
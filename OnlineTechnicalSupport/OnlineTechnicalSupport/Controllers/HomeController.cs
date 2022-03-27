using OnlineTechnicalSupport.Hubs;
using OnlineTechnicalSupport.Models.Context;
using OnlineTechnicalSupport.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Web.Routing;
using System.Web;
using OnlineTechnicalSupport.Models.ViewModels;
using System.Collections;
using System.IO;

namespace OnlineTechnicalSupport.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        OTSContext db = new OTSContext();

        public ActionResult Index()
        {
            GetUser();
            
            HomeOverViewModel model = new HomeOverViewModel();
            string[] assetArray = GetCookieArray("TechnicalSupportVisitor", "Assets");
            string[] servicesArray = GetCookieArray("TechnicalSupportVisitor", "ServiceRequests");
            string[] issueReportsArray = GetCookieArray("TechnicalSupportVisitor", "IssueReports");
            string[] chatsArray = GetCookieArray("TechnicalSupportVisitor", "Chats");
            List<Asset> assets = new List<Asset>();
            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
            List<IssueReport> issueReports = new List<IssueReport>();
            List<Chat> chats = new List<Chat>();
            if (assetArray != null)
            {
                for (int i = 0; i < assetArray.Length; i++)
                {
                    var assetTag = assetArray[i];
                    var asset =
                    (
                        from a in db.Assets
                        where (a.ServiceTag == assetTag)
                        select a
                    ).ToList();
                    if (asset.Count > 0)
                        assets.Add(asset[0]);
                }
            }
            if (servicesArray != null)
            {
                for (int i = 0; i < servicesArray.Length; i++)
                {
                    var serviceReqTag = servicesArray[i];
                    var serviceReqs =
                    (
                        from a in db.ServiceRequests
                        where (a.Tag == serviceReqTag)
                        select a
                    ).ToList();
                    if (serviceReqs.Count > 0)
                        serviceRequests.Add(serviceReqs[0]);
                }
            }
            if (issueReportsArray != null)
            {
                for (int i = 0; i < issueReportsArray.Length; i++)
                {
                    var issueReportTag = issueReportsArray[i];
                    var issueReps =
                    (
                        from a in db.IssueReports
                        where (a.Tag == issueReportTag)
                        select a
                    ).ToList();
                    if (issueReps.Count > 0)
                        issueReports.Add(issueReps[0]);
                }
            }
            if (chatsArray != null)
            {
                for (int i = 0; i < chatsArray.Length; i++)
                {
                    var chatTag = chatsArray[i];
                    var chts =
                    (
                        from a in db.Chats
                        where (a.Tag == chatTag)
                        select a
                    ).ToList();
                    if (chts.Count > 0)
                        chats.Add(chts[0]);
                }
            }
            model.Assets = assets;
            model.ServiceRequests = serviceRequests;
            model.IssueReports = issueReports;
            model.Chats = chats;
            return View(model);
        }

        [HttpGet]
        public RedirectToRouteResult GetAny(string search, string btn)
        {
            if(btn == "asset")
            {
                var asset =
            (
                from a in db.Assets
                where (a.AssetTag == search) || (a.ServiceTag == search)
                select a
            ).ToList();
                if (asset.Count > 0)
                {
                    AddCookies("TechnicalSupportVisitor", "Assets", asset[0].ServiceTag);
                }
            }
            else if (btn == "serviceRequest")
            {
                var serviceRequests =
            (
                from a in db.ServiceRequests
                where a.Tag == search
                select a
            ).ToList();
                if (serviceRequests.Count > 0)
                {
                    AddCookies("TechnicalSupportVisitor", "ServiceRequests", serviceRequests[0].Tag);
                }
            }
            else if (btn == "issueReport")
            {
                var issueReports =
            (
                from a in db.IssueReports
                where a.Tag == search
                select a
            ).ToList();
                if (issueReports.Count > 0)
                {
                    AddCookies("TechnicalSupportVisitor", "IssueReports", issueReports[0].Tag);
                }
            }
            else if (btn == "chat")
            {
                var chats =
            (
                from a in db.Chats
                where a.Tag == search
                select a
            ).ToList();
                if (chats.Count > 0)
                {
                    AddCookies("TechnicalSupportVisitor", "Chats", chats[0].Tag);
                }
            }
            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
        }

        [HttpPost]
        public RedirectToRouteResult SetAny(int id, string btn, string page, string type)
        {
            if(type == "asset")
            {
                var asset = db.Assets.Where(x => x.AssetId == id).FirstOrDefault();

                if (btn == "Show")
                {
                    RedirectToRouteResult result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                            { "action", "FindYourProduct" },
                            { "controller", "Home" },
                            { "searchAsset", asset.ServiceTag }
                        }
                    );
                    return result;
                }
                else if (btn == "Remove")
                {
                    RemoveCookies("TechnicalSupportVisitor", "Assets", asset.ServiceTag);
                }

                if(page == "FindYourProduct")
                    return new RedirectToRouteResult(new RouteValueDictionary(new { action = "FindYourProduct", controller = "Home" }));
            }
            else if(type == "serviceRequest")
            {
                var serviceRequest = db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();

                if (btn == "Show")
                {
                    RedirectToRouteResult result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "action", "CheckServiceRequest" },
                        { "controller", "Home" },
                        { "searchService", serviceRequest.Tag }
                        }
                    );
                    return result;
                }
                else if (btn == "Remove")
                {
                    RemoveCookies("TechnicalSupportVisitor", "ServiceRequests", serviceRequest.Tag);
                }
            }
            else if(type == "issueReport")
            {
                var issueReport = db.IssueReports.Where(x => x.IssueReportId == id).FirstOrDefault();

                if (btn == "Show")
                {
                    RedirectToRouteResult result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "action", "CheckIssueReport" },
                        { "controller", "Home" },
                        { "searchReport", issueReport.Tag }
                        }
                    );
                    return result;
                }
                else if (btn == "Remove")
                {
                    RemoveCookies("TechnicalSupportVisitor", "IssueReports", issueReport.Tag);
                }
            }
            else if(type == "chat")
            {
                var chat = db.Chats.Where(x => x.ChatId == id).FirstOrDefault();

                if (btn == "Show")
                {
                    RedirectToRouteResult result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "action", "LiveSupport" },
                        { "controller", "Home" },
                        { "code", chat.Tag }
                        }
                    );
                    return result;
                }
                else if (btn == "Remove")
                {
                    RemoveCookies("TechnicalSupportVisitor", "Chats", chat.Tag);
                }
            }
            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
        }

        public ActionResult FindYourProduct(string searchAsset)
        {
            GetUser();

            ServiceSupportOverViewModel s = new ServiceSupportOverViewModel();
            var asset =
            (
                from a in db.Assets
                where (a.AssetTag == searchAsset) || (a.ServiceTag == searchAsset)
                select a
            ).ToList();
            if (asset.Count > 0)
            {
                s.SelectedAsset = asset;
                Session["Asset"] = asset[0];
                AddCookies("TechnicalSupportVisitor", "Assets", asset[0].ServiceTag);
            }
            string[] assetArray = GetCookieArray("TechnicalSupportVisitor", "Assets");
            List<Asset> assets = new List<Asset>();
            if (assetArray != null)
            {
                for (int i = 0; i < assetArray.Length; i++)
                {
                    var assetTag = assetArray[i];
                    var a =
                    (
                        from b in db.Assets
                        where (b.ServiceTag == assetTag)
                        select b
                    ).ToList();
                    if (a.Count > 0)
                        assets.Add(a[0]);
                }
            }
            s.Assets = assets;
            return View(s);
        }

        public ActionResult NewIssueReport()
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            if (Session["Asset"] is null)
                return RedirectToAction("FindYourProduct");
            getIssueDataToList();
            return View();
        }

        [HttpPost]
        public ActionResult NewIssueReport(IssueReport model)
        {
            if (Session["Asset"] is null)
                return RedirectToAction("FindYourProduct");
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");
            GetUser();

            if (ModelState.IsValid)
            {
                IssueReport i = new IssueReport();

                string code = createCode(8);
                var test = db.ServiceRequests.FirstOrDefault(x => x.Tag == code);
                if (test == null)
                {
                    i.Tag = code.ToString();
                }
                else
                {
                    TempData["msg"] = "<script>alert('İşlem sırasında beklenmedik bir hata oluştu.')</script>";
                    return RedirectToAction("NewServiceRequest", "Home");
                }

                var asset = Session["Asset"] as Asset;

                string email = Session["AuthenticatedCustomer"].ToString();
                var user = db.Customers.FirstOrDefault(x => x.Email == email);
                if (user == null)
                    return RedirectToAction("Login", "Security");

                i.Email = model.Email;
                i.Title = model.Title;
                i.Description = model.Description;
                i.DateOfCreate = DateTime.Now;
                i.IssueId = model.IssueId;
                i.AssetId = asset.AssetId;
                i.IssueReportStatusId = 1;
                i.CustomerId = user.CustomerId;

                AddCookies("TechnicalSupportVisitor", "IssueReports", i.Tag);

                db.IssueReports.Add(i);
                db.SaveChanges();
                ViewBag.Result = "Kayıt yapıldı. Servis talebi kodunuz: " + i.Tag;
            }
            getIssueDataToList();
            return View();
        }

        public ActionResult LiveSupportRequest()
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            if (Session["Asset"] is null)
                return RedirectToAction("FindYourProduct");
            getIssueDataToList();
            return View();
        }

        [HttpPost]
        public ActionResult LiveSupportRequest(Chat model)
        {
            if (Session["Asset"] is null)
                return RedirectToAction("FindYourProduct");
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");
            GetUser();

            if (ModelState.IsValid)
            {
                Chat c = new Chat();

                string code = createCode(8);
                var test = db.Chats.FirstOrDefault(x => x.Tag == code);
                if (test == null)
                {
                    c.Tag = code.ToString();
                }
                else
                {
                    TempData["msg"] = "<script>alert('İşlem sırasında beklenmedik bir hata oluştu.')</script>";
                    return RedirectToAction("LiveSupportRequest", "Home");
                }

                var asset = Session["Asset"] as Asset;

                string email = Session["AuthenticatedCustomer"].ToString();
                var user = db.Customers.FirstOrDefault(x => x.Email == email);
                if (user == null)
                    return RedirectToAction("Login", "Security");

                c.Title = model.Title;
                c.IssueId = model.IssueId;
                c.AssetId = asset.AssetId;
                c.ChatStatusId = 1;
                c.DateOfCreate = DateTime.Now;
                c.CustomerId = user.CustomerId;
                db.Chats.Add(c);
                db.SaveChanges();
                ViewBag.Result = "İstek gönderildi. Sohbet kodunuz: " + c.Tag;
                ViewBag.ChatTag = c.Tag;

                AddCookies("TechnicalSupportVisitor", "Chats", c.Tag);
            }
            getIssueDataToList();
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult ConnectToChat(string code)
        {
            var chat = db.Chats.FirstOrDefault(x => x.Tag == code);

            RedirectToRouteResult result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "action", "LiveSupport" },
                        { "controller", "Home" },
                        { "code", code }
                    }
                );
            return result;
        }

        [Authorize]
        public ActionResult LiveSupport(string code)
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            //if (Session["ChatTag"] is null)
            //    return RedirectToAction("Index", "Home");
            var chat = db.Chats.FirstOrDefault(x => x.Tag == code);
            //var chat =
            //(
            //    from c in db.Chats
            //    where c.Tag == Session["ChatTag"].ToString()
            //    select c
            //).ToList();
            //var chat = Session["Chat"] as Chat;
            if (chat is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı bulunamadı. Tekrar deneyiniz.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else if(chat.CustomerId != user.CustomerId)
            {
                TempData["msg"] = "<script>alert('Erişim red edildi.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else if (chat.ChatStatusId == 3)
            {
                TempData["msg"] = "<script>alert('Sohbet sonlandırıldı.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["msg"] = "";
                if (Session["Chat"] is null)
                    Session["Chat"] = chat;
                else
                {
                    var test = Session["Chat"] as Chat;
                    if (code != test.Tag)
                    {
                        TempData["msg"] = "<script>alert('Yoğunluk yaşanmaması için sadece bir canlı destek bağlantısı kurabilirsiniz. Daha sonra tekrar bağlanabilirsiniz. Sohbet kodu: " + code + "')</script>";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LiveSupport(TextMessage model)
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            var chatSession = Session["Chat"] as Chat;
            if (chatSession is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı kurulamadı. Yeni bir istek göndermeyi deneyin.')</script>";
                return RedirectToAction("Index", "Home");
            }
            var chat = db.Chats.FirstOrDefault(x => x.ChatId == chatSession.ChatId);
            if (chat is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı kurulamadı. Yeni bir istek göndermeyi deneyin.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else if (chat.CustomerId != user.CustomerId)
            {
                TempData["msg"] = "<script>alert('Erişim red edildi.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else if (chat.ChatStatusId == 1)
            {
                TempData["msg"] = "<script>alert('Temsilcimiz henüz bağlanmadı. Lütfen bekleyiniz.')</script>";
            }
            else if (chat.ChatStatusId == 3)
            {
                TempData["msg"] = "<script>alert('Sohbet sonlandırıldı.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                if(model.TextContent != null)
                {
                    int id = chatSession.ChatId;

                    TextMessage t = new TextMessage();

                    t.SenderName = "Kullanıcı";
                    t.TextContent = model.TextContent;
                    t.ChatId = chat.ChatId;
                    t.Date = DateTime.Now;
                    db.TextMessages.Add(t);
                    db.SaveChanges();
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult CheckIssueReport(string searchReport)
        {
            //var issueReport =
            //(
            //    from s in db.IssueReports
            //    where s.Tag == searchReport
            //    select s
            //).ToList();

            if(Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            var issueReport = db.IssueReports.FirstOrDefault(x => x.Tag == searchReport);

            //if (issueReport.Count > 0 && Session["AuthenticatedCustomer"] == null)
            //    return RedirectToAction("Login", "Security");

            //if (issueReport != null && Session["AuthenticatedCustomer"] == null)
            //    return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            //if (issueReport.Count > 0 && user == null)
            //    return RedirectToAction("Login", "Security");
            if(user == null)
                return RedirectToAction("Login", "Security");
            //if (issueReport != null && user == null)
            //    return RedirectToAction("Login", "Security");
            if (issueReport != null && issueReport.CustomerId != user.CustomerId)
            {
                TempData["msg"] = "<script>alert('Erişim red edildi.')</script>";
                return RedirectToAction("Index", "Home");
            }

            GetUser();

            return View(issueReport);
        }

        public ActionResult CheckServiceRequest(string searchService)
        {
            GetUser();

            //var serviceRequest =
            //(
            //    from s in db.ServiceRequests
            //    where s.Tag == searchService
            //    select s
            //).ToList();

            var serviceRequest = db.ServiceRequests.FirstOrDefault(x => x.Tag == searchService);

            return View(serviceRequest);
        }

        [Authorize]
        public ActionResult MyAccount()
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            getGenderDataToList();
            getEducationDataToList();
            getJobDataToList();
            getCompSkillDataToList();
            getCountryDataToList();

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyAccount(Customer model)
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            ModelState["Email"].Errors.Clear();

            if (model.LoginPassword == null || model.ReLoginPassword == null)
            {
                ModelState["LoginPassword"].Errors.Clear();
                ModelState["ReLoginPassword"].Errors.Clear();
                model.LoginPassword = user.LoginPassword;
                model.ReLoginPassword = user.LoginPassword;
            }

            if (ModelState.IsValid)
            {
                if (model.LoginName != user.LoginName)
                {
                    var test1 = db.Customers.FirstOrDefault(x => x.LoginName == model.LoginName);
                    var test2 = db.Managers.FirstOrDefault(x => x.LoginName == model.LoginName);
                    if (test1 != null || test2 != null)
                    {
                        ViewBag.Error = "Kullanıcı adı daha önce alınmış.";
                        return View();
                    }
                    else
                        user.LoginName = model.LoginName;
                }
                if (model.PhoneNumber != user.PhoneNumber)
                {
                    var test1 = db.Customers.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
                    var test2 = db.Managers.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
                    if (test1 != null || test2 != null)
                    {
                        ViewBag.Error = "Telefon numarası başka bir kullanıcı üzerine kayıtlı.";
                        return View();
                    }
                    else
                        user.PhoneNumber = model.PhoneNumber;
                }
                if (model.LoginPassword != model.ReLoginPassword)
                {
                    ViewBag.Error = "Parola girişi hatalı, tekrar deneyin.";
                    return View();
                }
                else
                {
                    user.LoginPassword = model.LoginPassword;
                    user.ReLoginPassword = model.ReLoginPassword;
                }
                db.SaveChanges();
                ViewBag.Result = "Profil ayarlarınızı güncellediniz.";
            }

            getGenderDataToList();
            getEducationDataToList();
            getJobDataToList();
            getCompSkillDataToList();
            getCountryDataToList();

            return View(user);
        }

        public ActionResult Campaigns()
        {
            GetUser();

            var campaigns = db.Campaigns.ToList();

            return View(campaigns);
        }

        public ActionResult Campaign(int id)
        {
            //if (Session["AuthenticatedCustomer"] == null)
            //    return RedirectToAction("Login", "Security");

            //string email = Session["AuthenticatedCustomer"].ToString();
            //var user = db.Customers.FirstOrDefault(x => x.Email == email);
            //if (user == null)
            //    return RedirectToAction("Login", "Security");

            GetUser();

            var campaign = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);
            if (campaign == null)
                return RedirectToAction("Campaigns");

            return View(campaign);
        }

        [Authorize]
        public ActionResult Application(int id)
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            var campaign = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);
            if (campaign == null)
                return RedirectToAction("Campaigns");

            var application = db.CampaignApplications.FirstOrDefault(x => x.CampaignId == id && x.CustomerId == user.CustomerId);
            if(application != null)
            {
                TempData["msg"] = "<script>alert('Bu kampanyada başvurunuz bulunmaktadır.')</script>";
                return RedirectToAction("Campaigns");
            }

            Session["Campaign"] = campaign;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Application(int id, CampaignApplication model, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            if (Session["AuthenticatedCustomer"] == null)
                return RedirectToAction("Login", "Security");

            string email = Session["AuthenticatedCustomer"].ToString();
            var user = db.Customers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Security");

            GetUser();

            var campaign = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);
            if (campaign == null)
                return RedirectToAction("Campaigns");

            Session["Campaign"] = campaign;

            model.CampaignId = campaign.CampaignId;
            model.CustomerId = user.CustomerId;

            model.ApplicationDate = DateTime.Now;

            if (ModelState.ContainsKey("AttachmentDeviceSerialNoImage"))
                ModelState["AttachmentDeviceSerialNoImage"].Errors.Clear();
            if (ModelState.ContainsKey("AttachmentBillImage"))
                ModelState["AttachmentBillImage"].Errors.Clear();

            if (model.SelectUserName == true)
                model.Name = user.Name;
            if (model.SelectUserPhoneNumber == true)
                model.PhoneNumber = user.PhoneNumber;
            if (model.SelectUserEmail == true)
                model.Email = user.Email;
            if (model.SelectUserAddress == true)
                model.Address = user.Address;

            if (ModelState.IsValid)
            {
                string fileName1 = "";
                string fileName2 = "";
                string imgPath1 = "";
                string imgPath2 = "";
                Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Files/Images/Campaigns/" + campaign.CampaignId.ToString() + "/Applications/"), user.CustomerId.ToString()));
                if (file1 != null && file1.ContentLength > 0)
                {
                    fileName1 = Path.GetFileName(file1.FileName);
                    imgPath1 = Path.Combine(Server.MapPath("~/Files/Images/Campaigns/" + campaign.CampaignId + "/Applications/" + user.CustomerId), fileName1);
                }
                else
                {
                    TempData["msg"] = "<script>alert('Dosya yükleme hatası! Tekrar deneyin.')</script>";
                    return View();
                }
                if (file2 != null && file2.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(file2.FileName);
                    imgPath2 = Path.Combine(Server.MapPath("~/Files/Images/Campaigns/" + campaign.CampaignId + "/Applications/" + user.CustomerId), fileName2);
                }
                else
                {
                    TempData["msg"] = "<script>alert('Dosya yükleme hatası! Tekrar deneyin.')</script>";
                    return View();
                }
                if(fileName1 == fileName2)
                {
                    TempData["msg"] = "<script>alert('Dosya adları farklı olmalıdır.')</script>";
                    return View();
                }
                model.AttachmentDeviceSerialNoImage = file1.FileName;
                model.AttachmentBillImage = file2.FileName;

                db.CampaignApplications.Add(model);
                db.SaveChanges();
                file1.SaveAs(imgPath1);
                file2.SaveAs(imgPath2);
                TempData["msg"] = "<script>alert('Başvurunuz alınmıştır.')</script>";
                return RedirectToAction("Campaigns");
            }
            return View();
        }

        public string createCode(int length)
        {
            //Create tag for request
            char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[8];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder code = new StringBuilder(length);
            foreach (byte b in data)
            {
                code.Append(chars[b % (chars.Length)]);
            }
            return code.ToString();
        }

        public JsonResult GetMessages(string code)
        {
            var chat = db.Chats.FirstOrDefault(x => x.Tag == code);
            if (chat != null)
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["OTSContext"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(@"SELECT [TextMessageId], [TextContent], [ImageContent], [SenderName], [Date], [ManagerId], [ChatId]  from [dbo].[TextMessage] where ChatId=" + chat.ChatId + " order by TextMessageId", connection))
                    {
                        command.Notification = null;
                        SqlDependency dependency = new SqlDependency(command);
                        dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                        if (connection.State == ConnectionState.Closed)
                            connection.Open();

                        SqlDataReader dr = command.ExecuteReader();
                        var messageList = dr.Cast<IDataRecord>()
                                    .Select(x => new
                                    {
                                        TextMessageId = (int)x["TextMessageId"],
                                        TextContent = (string)x["TextContent"],
                                        SenderName = (string)x["SenderName"],
                                        Date = x["Date"].ToString(),
                                        ManagerId = x["ManagerId"].ToString()
                                    }).ToList();

                        return Json(new { msgList = messageList }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return new JsonResult();
        }
        public void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            ChatHub.Show();
        }

        public void SetCookies(string name)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Values["Assets"] = "";
            cookie.Values["ServiceRequests"] = "";
            cookie.Values["IssueReports"] = "";
            cookie.Values["Chats"] = "";
            cookie.Secure = true;
            cookie.Expires = DateTime.Now.AddDays(365);
            HttpContext.Response.Cookies.Add(cookie);
        }

        public void AddCookies(string name, string key, string value)
        {
            if (!GetCookie(name))
                SetCookies(name);
            var cookie = Request.Cookies[name];
            if(!cookie.Values[key].Contains(value))
            {
                if (GetCookie(name, key))
                    value = "," + value;
                cookie.Values[key] += value;
                Response.Cookies.Add(cookie);
            }
        }

        public void RemoveCookies(string name, string key, string value)
        {
            if (!GetCookie(name))
                SetCookies(name);
            var cookie = Request.Cookies[name];
            if (GetCookie(name, key))
            {
                string n = "";
                string str = cookie.Values[key];
                if (str.Contains(','))
                {
                    string[] s = str.Split(',');
                    if (s[0] == value)
                        n = str.Replace(value + ",", string.Empty);
                    else
                        n = str.Replace("," + value, string.Empty);
                    cookie.Values[key] = n;
                }
                else if (str == value)
                    cookie.Values[key] = string.Empty;
                Response.Cookies.Add(cookie);
            }
        }

        public bool GetCookie(string name)
        {
            if (Request.Cookies.AllKeys.Contains(name))
                return true;
            return false;
        }

        public bool GetCookie(string name, string key)
        {
            if (Request.Cookies.AllKeys.Contains(name) && Request.Cookies[name].Values[key] != "")
                return true;
            return false;
        }

        public string[] GetCookieArray(string cookieName, string cookieKey)
        {
            if (GetCookie(cookieName, cookieKey))
            {
                string str = Request.Cookies[cookieName].Values[cookieKey];
                string[] a = str.Split(',');
                return a;
            }
            string[] b = null;
            return b;
        }

        public void GetUser()
        {
            if (Session["AuthenticatedCustomer"] != null)
            {
                string email = Session["AuthenticatedCustomer"].ToString();
                var user = db.Customers.FirstOrDefault(x => x.Email == email);
                if (user != null)
                    ViewBag.User = user.LoginName;
            }
        }

        public void getIssueDataToList()
        {
            List<SelectListItem> Issues = new List<SelectListItem>();
            int issueId = 0;

            foreach (var item in db.Issues.ToList())
            {
                Issues.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.IssueId.ToString(),
                        Selected = (item.IssueId == issueId ? true : false),
                    });
            }
            ViewBag.Issues = Issues;
        }

        public void getGenderDataToList()
        {
            List<SelectListItem> Genders = new List<SelectListItem>();
            int gId = 0;

            foreach (var item in db.Genders.ToList())
            {
                Genders.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.GenderId.ToString(),
                        Selected = (item.GenderId == gId ? true : false),
                    });
            }
            ViewBag.Genders = Genders;
        }

        public void getEducationDataToList()
        {
            List<SelectListItem> Eds = new List<SelectListItem>();
            int eduId = 0;

            foreach (var item in db.Educations.ToList())
            {
                Eds.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.EducationId.ToString(),
                        Selected = (item.EducationId == eduId ? true : false),
                    });
            }
            ViewBag.Educations = Eds;
        }

        public void getJobDataToList()
        {
            List<SelectListItem> Jobs = new List<SelectListItem>();
            int jId = 0;

            foreach (var item in db.Jobs.ToList())
            {
                Jobs.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.JobId.ToString(),
                        Selected = (item.JobId == jId ? true : false),
                    });
            }
            ViewBag.Jobs = Jobs;
        }

        public void getCompSkillDataToList()
        {
            List<SelectListItem> Skills = new List<SelectListItem>();
            int sId = 0;

            foreach (var item in db.ComputerSkills.ToList())
            {
                Skills.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.ComputerSkillId.ToString(),
                        Selected = (item.ComputerSkillId == sId ? true : false),
                    });
            }
            ViewBag.ComputerSkills = Skills;
        }

        public void getCountryDataToList()
        {
            List<SelectListItem> Countries = new List<SelectListItem>();
            int cId = 0;

            foreach (var item in db.Countries.ToList())
            {
                Countries.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.CountryId.ToString(),
                        Selected = (item.CountryId == cId ? true : false),
                    });
            }
            ViewBag.Countries = Countries;
        }
    }
}
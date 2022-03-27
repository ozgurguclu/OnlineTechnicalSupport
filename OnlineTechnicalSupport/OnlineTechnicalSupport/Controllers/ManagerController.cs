using Microsoft.Owin;
using OnlineTechnicalSupport.Hubs;
using OnlineTechnicalSupport.Models.Classes;
using OnlineTechnicalSupport.Models.Context;
using OnlineTechnicalSupport.Models.Data;
using OnlineTechnicalSupport.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineTechnicalSupport.Controllers
{
    public class ManagerController : Controller
    {
        OTSContext db = new OTSContext();

        [Authorize]
        public ActionResult Index()
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();

            OverViewModel model = new OverViewModel();

            model.TotalChatCount = db.Chats.Count();
            model.TotalReportCount = db.IssueReports.Count();
            model.TotalRepliedChatCount = db.Chats.Where(x => x.ChatStatusId == 3).Count();
            model.TotalRepliedReportCount = db.IssueReports.Where(x => x.IssueReportStatusId == 4 || x.IssueReportStatusId == 5).Count();
            model.ReceivedChatCount = db.Chats.Where(x => x.ManagerId == user.ManagerId).Count();
            model.ReceivedReportCount = db.IssueReports.Where(x => x.ManagerId == user.ManagerId).Count();
            model.RepliedChatCount = db.Chats.Where(x => x.ManagerId == user.ManagerId && x.ChatStatusId == 3).Count();
            model.RepliedReportCount = db.IssueReports.Where(x => x.ManagerId == user.ManagerId && x.RepliedBy == user.Name + " " + user.Surname && (x.IssueReportStatusId == 4 || x.IssueReportStatusId == 5)).Count();
            model.MissedChatCount = db.Chats.Where(x => x.ManagerId == user.ManagerId && x.ChatStatusId != 3).Count();
            model.MissedReportCount = db.IssueReports.Where(x => x.ManagerId == user.ManagerId && x.RepliedBy == "" && (x.IssueReportStatusId != 4 || x.IssueReportStatusId != 5)).Count();

            if (model.MissedChatCount > 0)
                ViewBag.MissedChats = model.MissedChatCount;
            if (model.MissedReportCount > 0)
                ViewBag.MissedReports = model.MissedReportCount;
            return View(model);
        }

        [Authorize]
        public ActionResult IssueReports()
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();

            IssueReportsViewModel model = new IssueReportsViewModel();

            model.UnManagedRequests = db.IssueReports.Where(x => x.ManagerId == null && x.IssueReportStatusId != 4).ToList();
            model.ManagedRequests = db.IssueReports.Where(x => x.ManagerId == user.ManagerId).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult LiveSupportRequests()
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();

            LiveSupportRequestsViewModel model = new LiveSupportRequestsViewModel();

            model.UnManagedRequests = db.Chats.Where(x => x.ManagerId == null && x.ChatStatusId != 3).ToList();
            model.ManagedRequests = db.Chats.Where(x => x.ManagerId == user.ManagerId).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult ReplyIssueReport(int id)
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();

            var issueReport = db.IssueReports.FirstOrDefault(x => x.IssueReportId == id);

            if (issueReport is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı yok.')</script>";
                return RedirectToAction("IssueReports", "Manager");
            }
            else if (issueReport.ManagerId != user.ManagerId)
            {
                TempData["msg"] = "<script>alert('Görüntülemeye izniniz yok.')</script>";
                return RedirectToAction("IssueReports", "Manager");
            }

            ViewBag.ServiceRequestTitle = issueReport.Title;
            ViewBag.IssueName = issueReport.Issue.Name;
            ViewBag.ProductId = issueReport.Asset.ProductId;
            ViewBag.ProductName = issueReport.Asset.Product.ProductModel.Name + " " + issueReport.Asset.Product.Name;
            ViewBag.ProductImage = issueReport.Asset.Product.Image;
            ViewBag.ServiceTag = issueReport.Asset.ServiceTag;

            ReplyRequestView model = new ReplyRequestView();
            model.ReplyContent = issueReport.Reply;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ReplyIssueReport(int id, ReplyRequestView model)
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();

            var issueReport = db.IssueReports.FirstOrDefault(x => x.IssueReportId == id);

            if (issueReport is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı yok.')</script>";
                return RedirectToAction("ServiceRequests", "Manager");
            }
            else if (issueReport.ManagerId != user.ManagerId)
            {
                TempData["msg"] = "<script>alert('Görüntülemeye izniniz yok.')</script>";
                return RedirectToAction("ServiceRequests", "Manager");
            }
            else
            {
                if (model.ReplyContent != null)
                {
                    issueReport.RepliedBy = user.Name + " " + user.Surname;
                    issueReport.Reply = model.ReplyContent;
                    issueReport.DateOfReply = DateTime.Now;
                    issueReport.IssueReportStatusId = 4;
                    db.SaveChanges();

                    ViewBag.Result = "Yanıtınız gönderildi.";
                }
            }

            ViewBag.ServiceRequestTitle = issueReport.Title;
            ViewBag.IssueName = issueReport.Issue.Name;
            ViewBag.ProductName = issueReport.Asset.Product.ProductModel.Name + " " + issueReport.Asset.Product.Name;
            ViewBag.ProductImage = issueReport.Asset.Product.Image;
            ViewBag.ServiceTag = issueReport.Asset.ServiceTag;

            return View();
        }

        [Authorize]
        public ActionResult LiveSupport(int id)
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();

            var chat = db.Chats.FirstOrDefault(x => x.ChatId == id);
            if(chat is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı yok.')</script>";
                return RedirectToAction("LiveSupportRequests", "Manager");
            }
            else if(chat.ManagerId != user.ManagerId)
            {
                TempData["msg"] = "<script>alert('Görüntülemeye izniniz yok.')</script>";
                return RedirectToAction("LiveSupportRequests", "Manager");
            }
            else if(chat.ChatStatusId == 3)
            {
                TempData["msg"] = "<script>alert('Bağlantı kapatılmıştır.')</script>";
                return RedirectToAction("LiveSupportRequests", "Manager");
            }

            ViewBag.ChatTitle = chat.Title;
            ViewBag.IssueName = chat.Issue.Name;
            ViewBag.ProductId = chat.Asset.ProductId;
            ViewBag.ProductName = chat.Asset.Product.ProductModel.Name + " " + chat.Asset.Product.Name;
            ViewBag.ProductImage = chat.Asset.Product.Image;
            ViewBag.ServiceTag = chat.Asset.ServiceTag;

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult LiveSupport(int id, TextMessage model)
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            //ViewData["FullName"] = user.Name + " " + user.Surname;
            var user = GetUser();
            var chat = db.Chats.FirstOrDefault(x => x.ChatId == id);
            if (chat is null)
            {
                TempData["msg"] = "<script>alert('Bağlantı kurulamadı. Yeni bir istek göndermeyi deneyin.')</script>";
                return RedirectToAction("Index", "Home");
            }
            else if (chat.ManagerId != user.ManagerId)
            {
                TempData["msg"] = "<script>alert('Mesaj göndermeye izniniz yok.')</script>";
                return RedirectToAction("LiveSupportRequests", "Manager");
            }
            else if (chat.ChatStatusId == 1 || chat.ChatStatusId == 3)
            {
                TempData["msg"] = "<script>alert('Sohbet aktif değil veya kapandı.')</script>";
            }
            else
            {
                if (model.TextContent != null)
                {
                    TextMessage t = new TextMessage();

                    t.SenderName = "Temsilci";
                    t.TextContent = model.TextContent;
                    t.ChatId = chat.ChatId;
                    t.ManagerId = user.ManagerId;
                    t.Date = DateTime.Now;
                    db.TextMessages.Add(t);
                    db.SaveChanges();
                }
            }

            ViewBag.ChatTitle = chat.Title;
            ViewBag.IssueName = chat.Issue.Name;
            ViewBag.ProductId = chat.Asset.ProductId;
            ViewBag.ProductName = chat.Asset.Product.ProductModel.Name + " " + chat.Asset.Product.Name;
            ViewBag.ProductImage = chat.Asset.Product.Image;
            ViewBag.ServiceTag = chat.Asset.ServiceTag;

            return View();
        }

        [Authorize]
        public ActionResult ServiceRequests(string searchService)
        {
            var user = GetUser();

            //var servicess = (
            //        from s in db.ServiceRequests
            //        where s.ServiceRequestId.ToString() == searchService || s.Tag.Contains(searchService) || s.Asset.ServiceTag.Contains(searchService) || s.Asset.AssetTag.Contains(searchService)
            //        select s
            //    ).ToList();

            var servicess = db.ServiceRequests.Where(x => x.ServiceRequestId.ToString() == searchService || x.Tag.Contains(searchService) || x.Asset.ServiceTag.Contains(searchService) || x.Asset.AssetTag.Contains(searchService)).ToList();

            if (servicess.Count > 0)
                return View(servicess);

            var services = db.ServiceRequests.ToList();

            return View(services);
        }

        [Authorize]
        public ActionResult NewServiceRequest()
        {
            var user = GetUser();

            getIssueDataToList();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewServiceRequest(ServiceRequest model, string searchAsset)
        {
            var user = GetUser();

            getIssueDataToList();

            var asset = db.Assets.FirstOrDefault(x => x.AssetTag == searchAsset || x.ServiceTag == searchAsset);
            if (asset == null)
            {
                return View();
            }
            ViewBag.ProductId = asset.Product.ProductId;
            ViewBag.Asset = asset.Product.ProductModel.Name + " " + asset.Product.Name + " " + asset.Product.ProductModel.ProductCategory.Name;
            ViewBag.ProductImage = asset.Product.Image;

            model.AssetId = asset.AssetId;
            model.DateOfCreate = DateTime.Now;
            model.ManagerId = user.ManagerId;
            model.ServiceRequestStatusId = 1;

            if (ModelState.IsValid)
            {
                CryptoServices cs = new CryptoServices();
                string code = cs.CreateCode(8);
                model.Tag = code;
                model.CreatedBy = user.Name + " " + user.Surname;
                db.ServiceRequests.Add(model);
                db.SaveChanges();
                ViewBag.Result = "Servis kaydı oluşturuldu. Takip kodu: " + model.Tag;
            }

            return View();
        }

        [Authorize]
        public ActionResult EditServiceRequest(int id)
        {
            var user = GetUser();

            var s = db.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == id);

            if (s == null)
                RedirectToAction("ServiceRequests");

            getServiceStatusDataToList();

            return View(s);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditServiceRequest(int id, ServiceRequest model)
        {
            var user = GetUser();

            ModelState["Name"].Errors.Clear();
            ModelState["Surname"].Errors.Clear();
            ModelState["PhoneNumber"].Errors.Clear();

            var s = db.ServiceRequests.FirstOrDefault(x => x.ServiceRequestId == id);

            if (s == null)
                RedirectToAction("ServiceRequests");

            if(ModelState.IsValid)
            {
                s.ServiceRequestStatusId = model.ServiceRequestStatusId;
                s.Notes = model.Notes;
                s.DateOfEdit = DateTime.Now;
                s.EditedBy = user.Name + " " + user.Surname;
                db.SaveChanges();
                ViewBag.Result = "Servis kaydı başarıyla güncellendi.";
            }

            getServiceStatusDataToList();

            return View(s);
        }

        [Authorize]
        public ActionResult Products(string searchProduct)
        {
            var user = GetUser();

            var productss = db.Products.Where(x => x.ProductId.ToString() == searchProduct || x.Name.Contains(searchProduct) || x.Tag.Contains(searchProduct)).ToList();

            if (productss.Count > 0)
                return View(productss);

            var products = db.Products.ToList();

            return View(products);
        }

        [Authorize]
        public ActionResult NewProduct()
        {
            var user = GetUser();

            getProductModelDataToList();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewProduct(Product model, HttpPostedFileBase file)
        {
            var user = GetUser();

            if (ModelState.ContainsKey("Image"))
                ModelState["Image"].Errors.Clear();

            if (file != null && file.ContentLength > 0)
            {
                model.Image = file.FileName;
            }
            else
            {
                TempData["msg"] = "<script>alert('Dosya yükleme hatası! Tekrar deneyin.')</script>";
                getProductModelDataToList();
                return View();
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(model);
                db.SaveChanges();

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Files/Images/Products/"), model.ProductId.ToString()));
                    string imgPath1 = Path.Combine(Server.MapPath("~/Files/Images/Products/" + model.ProductId + "/"), fileName);
                    file.SaveAs(imgPath1);
                    ViewBag.pId = model.ProductId;
                    ViewBag.pImage = file.FileName;
                }

                ViewBag.Result = "Yeni ürün başarıyla eklendi.";
            }

            getProductModelDataToList();

            return View();
        }

        [Authorize]
        public ActionResult EditProduct(int id)
        {
            var user = GetUser();

            var p = db.Products.FirstOrDefault(x => x.ProductId == id);

            if (p == null)
                RedirectToAction("Products");

            getProductModelDataToList();

            return View(p);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProduct(int id, Product model, HttpPostedFileBase file)
        {
            var user = GetUser();

            if (ModelState.ContainsKey("Image"))
                ModelState["Image"].Errors.Clear();

            var p = db.Products.FirstOrDefault(x => x.ProductId == id);

            if (p == null)
                RedirectToAction("Products");

            string imgPath1 = "";
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                imgPath1 = Path.Combine(Server.MapPath("~/Files/Images/Products/" + p.ProductId + "/"), fileName);
                model.Image = file.FileName;
            }

            if(ModelState.IsValid)
            {
                if (model.Image != null)
                    p.Image = model.Image;
                p.Name = model.Name;
                p.Tag = model.Tag;
                p.ProductModelId = model.ProductModelId;
                db.SaveChanges();
                if (model.Image != null)
                    file.SaveAs(imgPath1);
                ViewBag.Result = "Ürün başarıyla güncellendi.";
            }

            getProductModelDataToList();

            return View(p);
        }

        [Authorize]
        public ActionResult Assets(string searchAsset)
        {
            var user = GetUser();

            var assetss = db.Assets.Where(x => x.ProductId.ToString() == searchAsset || x.AssetTag.Contains(searchAsset) || x.ServiceTag.Contains(searchAsset) || x.Product.Name.Contains(searchAsset)).ToList();

            if (assetss.Count > 0)
                return View(assetss);

            var assets = db.Assets.ToList();

            return View(assets);
        }

        [Authorize]
        public ActionResult NewAsset()
        {
            var user = GetUser();

            getProductDataToList();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewAsset(Asset model)
        {
            var user = GetUser();

            if (ModelState.IsValid)
            {
                db.Assets.Add(model);
                db.SaveChanges();
                if (model.PurchaseDate != null)
                {
                    AssetWarrantyDetail a = new AssetWarrantyDetail();
                    a.AssetId = model.AssetId;
                    a.WarrantyType = "Standart";
                    a.WarrantyYears = 2;
                    a.WarrantyStartDate = (DateTime)model.PurchaseDate;
                    a.WarrantyEndDate = ((DateTime)model.PurchaseDate).AddYears(2);
                    db.AssetWarrantyDetails.Add(a);
                    db.SaveChanges();
                }

                ViewBag.Result = "Yeni cihaz başarıyla eklendi.";
            }

            getProductDataToList();

            return View();
        }

        [Authorize]
        public ActionResult EditAsset(int id)
        {
            var user = GetUser();

            var p = db.Assets.FirstOrDefault(x => x.AssetId == id);

            if (p == null)
                RedirectToAction("Assets");

            return View(p);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditAsset(int id, Asset model)
        {
            var user = GetUser();

            if (ModelState.ContainsKey("AssetTag"))
                ModelState["AssetTag"].Errors.Clear();
            if (ModelState.ContainsKey("ServiceTag"))
                ModelState["ServiceTag"].Errors.Clear();

            var p = db.Assets.FirstOrDefault(x => x.AssetId == id);

            if (p == null)
                RedirectToAction("Assets");

            if (ModelState.IsValid)
            {
                if(p.PurchaseDate == null && model.PurchaseDate != null)
                {
                    AssetWarrantyDetail a = new AssetWarrantyDetail();
                    a.AssetId = p.AssetId;
                    a.WarrantyType = "Standart";
                    a.WarrantyYears = 2;
                    a.WarrantyStartDate = (DateTime)model.PurchaseDate;
                    a.WarrantyEndDate = ((DateTime)model.PurchaseDate).AddYears(2);
                    p.PurchaseDate = model.PurchaseDate;
                    db.AssetWarrantyDetails.Add(a);
                    db.SaveChanges();
                }
                ViewBag.Result = "Cihaz bilgileri başarıyla güncellendi.";
            }

            return View(p);
        }

        [Authorize]
        public ActionResult NewWarranty(int id)
        {
            var user = GetUser();

            var p = db.Assets.FirstOrDefault(x => x.AssetId == id);

            if (p == null)
                RedirectToAction("Assets");

            else if(p.AssetWarrantyDetails.Count == 0)
                RedirectToAction("Assets");

            WarrantyYearsToList();

            ViewBag.Asset = p;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewWarranty(int id, AssetWarrantyDetail model)
        {
            var user = GetUser();

            if (ModelState.ContainsKey("WarrantyType"))
                ModelState["WarrantyType"].Errors.Clear();
            if (ModelState.ContainsKey("WarrantyStartDate"))
                ModelState["WarrantyStartDate"].Errors.Clear();
            if (ModelState.ContainsKey("WarrantyEndDate"))
                ModelState["WarrantyEndDate"].Errors.Clear();

            var p = db.Assets.FirstOrDefault(x => x.AssetId == id);

            if (p == null)
                RedirectToAction("Assets");

            else if (p.AssetWarrantyDetails.Count == 0)
                RedirectToAction("Assets");

            if(ModelState.IsValid)
            {
                model.AssetId = p.AssetId;
                model.WarrantyType = "Ek Garanti";
                model.WarrantyStartDate = p.AssetWarrantyDetails.Last().WarrantyEndDate;
                model.WarrantyEndDate = model.WarrantyStartDate.AddYears(model.WarrantyYears);
                db.AssetWarrantyDetails.Add(model);
                db.SaveChanges();

                ViewBag.Result = "Belirtilen cihaza ek garanti süresi başarıyla eklendi.";
            }

            WarrantyYearsToList();

            ViewBag.Asset = p;
            return View();
        }

        [Authorize]
        public ActionResult Campaigns(string searchCampaign)
        {
            var user = GetUser();

            var campaignss = db.Campaigns.Where(x => x.CampaignId.ToString() == searchCampaign || x.Name.Contains(searchCampaign)).ToList();

            if (campaignss.Count > 0)
                return View(campaignss);

            var campaigns = db.Campaigns.ToList();

            return View(campaigns);
        }

        [Authorize]
        public ActionResult NewCampaign()
        {
            var user = GetUser();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewCampaign(Campaign model, HttpPostedFileBase file)
        {
            var user = GetUser();

            if (ModelState.ContainsKey("Image"))
                ModelState["Image"].Errors.Clear();

            if (file != null && file.ContentLength > 0)
            {
                model.Image = file.FileName;
            }
            else
            {
                TempData["msg"] = "<script>alert('Dosya yükleme hatası! Tekrar deneyin.')</script>";
                return View();
            }
            model.Activated = 0;

            if (ModelState.IsValid)
            {
                if(model.StartDate >= model.EndDate)
                {
                    TempData["msg"] = "<script>alert('Başlangıç ve bitiş tarihlerini kontrol ediniz.')</script>";
                    return View();
                }

                db.Campaigns.Add(model);
                db.SaveChanges();

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Files/Images/Campaigns/"), model.CampaignId.ToString()));
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Files/Images/Campaigns/"), model.CampaignId.ToString() + "/Applications"));
                    string imgPath1 = Path.Combine(Server.MapPath("~/Files/Images/Campaigns/" + model.CampaignId + "/"), fileName);
                    file.SaveAs(imgPath1);
                    ViewBag.CId = model.CampaignId;
                    ViewBag.CImage = file.FileName;
                }

                ViewBag.Result = "Yeni kampanya başarıyla eklendi.";
            }

            return View();
        }

        [Authorize]
        public ActionResult EditCampaign(int id)
        {
            var user = GetUser();

            var c = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);

            if (c == null)
                RedirectToAction("Campaigns");

            return View(c);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditCampaign(int id, Campaign model, HttpPostedFileBase file)
        {
            var user = GetUser();

            if (ModelState.ContainsKey("Image"))
                ModelState["Image"].Errors.Clear();

            var c = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);

            if (c == null)
                RedirectToAction("Campaigns");

            string imgPath1 = "";
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                imgPath1 = Path.Combine(Server.MapPath("~/Files/Images/Campaigns/" + c.CampaignId + "/"), fileName);
                model.Image = file.FileName;
            }

            if (model.Activated != 0 && model.Activated != 1)
                model.Activated = 0;

            if (ModelState.IsValid)
            {
                if(model.StartDate >= model.EndDate)
                {
                    TempData["msg"] = "<script>alert('Başlangıç ve bitiş tarihlerini kontrol ediniz.')</script>";
                    return View();
                }

                if (model.Image != null)
                    c.Image = model.Image;
                c.Name = model.Name;
                c.Title = model.Title;
                c.Description = model.Description;
                c.StartDate = model.StartDate;
                c.EndDate = model.EndDate;
                c.Activated = model.Activated;
                db.SaveChanges();
                if (model.Image != null)
                    file.SaveAs(imgPath1);
                ViewBag.Result = "Kampanya başarıyla güncellendi.";
            }

            return View(c);
        }

        [Authorize]
        public ActionResult Applications(int id)
        {
            var user = GetUser();

            var c = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);
            var ca = db.CampaignApplications.Where(x => x.CampaignId == id).ToList();

            if (c == null)
                RedirectToAction("Campaigns");

            return View(ca);
        }

        [Authorize]
        public ActionResult Application(int id)
        {
            var user = GetUser();

            var c = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);
            var ca = db.CampaignApplications.FirstOrDefault(x => x.CampaignApplicationId == id);

            if (c == null)
                RedirectToAction("Campaigns");
            if (ca == null)
                RedirectToAction("Applications", new { id = c.CampaignId });

            return View(ca);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Application(int id, string btn)
        {
            var user = GetUser();

            var c = db.Campaigns.FirstOrDefault(x => x.CampaignId == id);
            var ca = db.CampaignApplications.FirstOrDefault(x => x.CampaignApplicationId == id);

            if (c == null)
                RedirectToAction("Campaigns");
            if (ca == null)
                RedirectToAction("Applications", new { id = c.CampaignId });

            if(btn == "Approve")
            {
                ca.Result = "Onaylandı";
                db.SaveChanges();
            }
            else if(btn == "Deny")
            {
                ca.Result = "Red edildi";
                db.SaveChanges();
            }

            return View(ca);
        }

        [Authorize]
        public ActionResult MyAccount()
        {
            var user = GetUser();

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyAccount(Manager model, HttpPostedFileBase file)
        {
            var user = GetUser();

            ModelState["Email"].Errors.Clear();
            ModelState["Name"].Errors.Clear();
            ModelState["Surname"].Errors.Clear();

            if(model.LoginPassword == null || model.ReLoginPassword == null)
            {
                ModelState["LoginPassword"].Errors.Clear();
                ModelState["ReLoginPassword"].Errors.Clear();
                model.LoginPassword = user.LoginPassword;
                model.ReLoginPassword = user.LoginPassword;
            }

            string imgPath1 = "";
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Files/Images/Managers/"), user.ManagerId.ToString()));
                imgPath1 = Path.Combine(Server.MapPath("~/Files/Images/Managers/" + user.ManagerId + "/"), fileName);
                model.Image = file.FileName;
            }

            if (ModelState.IsValid)
            {
                if(model.PhoneNumber != null)
                {
                    var phoneNumber1 = db.Customers.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
                    var phoneNumber2 = db.Managers.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
                    if(phoneNumber1 != null || (phoneNumber2 != null && phoneNumber2.ManagerId != user.ManagerId))
                    {
                        ViewBag.Error = "Telefon numarası kayıtlıdır!";
                        user = GetUser();
                        return View(user);
                    }
                }
                var loginName1 = db.Customers.FirstOrDefault(x => x.LoginName == model.LoginName);
                var loginName2 = db.Managers.FirstOrDefault(x => x.LoginName == model.LoginName);
                if (loginName1 != null || (loginName2 != null && loginName2.ManagerId != user.ManagerId))
                {
                    ViewBag.Error = "Kullanıcı adı kayıtlıdır!";
                    user = GetUser();
                    return View(user);
                }
                if (model.Image != null)
                    user.Image = model.Image;
                if(user.PhoneNumber != model.PhoneNumber)
                    user.PhoneNumber = model.PhoneNumber;
                if(user.LoginName != model.LoginName)
                    user.LoginName = model.LoginName;
                user.LoginPassword = model.LoginPassword;
                user.ReLoginPassword = model.ReLoginPassword;
                db.SaveChanges();
                if (model.Image != null)
                    file.SaveAs(imgPath1);
                ViewBag.Result = "Profil ayarlarınızı güncellediniz.";
            }

            user = GetUser();
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult GetReport(int id)
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            var user = GetUser();
            var issueReport = db.IssueReports.Where(x => x.IssueReportId == id).FirstOrDefault();
            issueReport.ManagerId = user.ManagerId;
            db.SaveChanges();

            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "IssueReports", controller = "Manager" }));
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult SetReport(int id, string btn)
        {
            var user = GetUser();
            var issueReport = db.IssueReports.Where(x => x.IssueReportId == id).FirstOrDefault();

            if (btn == "Reply")
            {
                if(issueReport.RepliedBy == "")
                    issueReport.IssueReportStatusId = 2;
                db.SaveChanges();
                RedirectToRouteResult result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "action", "ReplyIssueReport" },
                        { "controller", "Manager" },
                        { "id", id }
                    }
                );
                return result;
                //return new RedirectToRouteResult(new RouteValueDictionary(new { action = "LiveSupport", controller = "Manager" }));
            }
            else if (btn == "Suspend")
            {
                if (issueReport.RepliedBy == "")
                    issueReport.IssueReportStatusId = 3;
                db.SaveChanges();
            }
            else if (btn == "Close")
            {
                if (issueReport.RepliedBy == "")
                    issueReport.IssueReportStatusId = 5;
                db.SaveChanges();
            }
            else if (btn == "Drop")
            {
                if (issueReport.RepliedBy == "")
                    issueReport.IssueReportStatusId = 1;
                issueReport.ManagerId = null;
                db.SaveChanges();
            }
            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "IssueReports", controller = "Manager" }));
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult GetChat(int id)
        {
            //string userEmail = GetUserProfile();
            //var user = db.Managers.FirstOrDefault(x => x.Email == userEmail);
            var user = GetUser();
            var chat = db.Chats.Where(x => x.ChatId == id).FirstOrDefault();
            chat.ManagerId = user.ManagerId;
            db.SaveChanges();

            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "LiveSupportRequests", controller = "Manager" }));
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult SetChat(int id, string btn)
        {
            var user = GetUser();
            var chat = db.Chats.Where(x => x.ChatId == id).FirstOrDefault();

            if(btn == "Connect")
            {
                chat.ChatStatusId = 2;
                db.SaveChanges();
                RedirectToRouteResult result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "action", "LiveSupport" },
                        { "controller", "Manager" },
                        { "id", id }
                    }
                );
                return result;
                //return new RedirectToRouteResult(new RouteValueDictionary(new { action = "LiveSupport", controller = "Manager" }));
            }
            else if (btn == "Lock")
            {
                chat.ChatStatusId = 1;
                db.SaveChanges();
            }
            else if(btn == "Close")
            {
                chat.ChatStatusId = 3;
                db.SaveChanges();
            }
            else if(btn == "Drop")
            {
                if (chat.ChatStatusId == 2)
                    chat.ChatStatusId = 1;
                chat.ManagerId = null;
                db.SaveChanges();
            }
            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "LiveSupportRequests", controller = "Manager" }));
        }

        public string GetUserProfile()
        {
            //var Name = HttpContext.Request.Cookies["AuthenticatedManager"];
            //return Name.Value;
            if (Session["AuthenticatedManager"] == null)
                Response.Redirect("/Security/Login");
            string email = Session["AuthenticatedManager"].ToString();
            return email;
        }

        public Manager GetUser()
        {
            if (Session["AuthenticatedManager"] == null)
                Response.Redirect("/Security/Login");
            string email = Session["AuthenticatedManager"].ToString();
            var user = db.Managers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                Response.Redirect("/Security/Login");
            ViewBag.uId = user.ManagerId;
            ViewBag.uImage = user.Image;
            ViewBag.uName = user.Name;
            ViewBag.uFullName = user.Name + " " + user.Surname;
            return user;
        }

        [Authorize]
        public JsonResult GetMessages(int id)
        {
            var chat = db.Chats.FirstOrDefault(x => x.ChatId == id);
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

        public void getServiceStatusDataToList()
        {
            List<SelectListItem> Situations = new List<SelectListItem>();
            int statusId = 0;

            foreach (var item in db.ServiceRequestSituations.ToList())
            {
                Situations.Add(
                    new SelectListItem
                    {
                        Text = item.Tag,
                        Value = item.ServiceRequestStatusId.ToString(),
                        Selected = (item.ServiceRequestStatusId == statusId ? true : false),
                    });
            }
            ViewBag.ServiceStatus = Situations;
        }

        public void getProductModelDataToList()
        {
            List<SelectListItem> ProductModels = new List<SelectListItem>();
            int modelId = 0;

            foreach (var item in db.ProductModels.ToList())
            {
                ProductModels.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.ProductModelId.ToString(),
                        Selected = (item.ProductModelId == modelId ? true : false),
                    });
            }
            ViewBag.ProductModels = ProductModels;
        }

        public void getProductDataToList()
        {
            List<SelectListItem> Products = new List<SelectListItem>();
            int pId = 0;

            foreach (var item in db.Products.ToList())
            {
                Products.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.ProductModelId.ToString(),
                        Selected = (item.ProductModelId == pId ? true : false),
                    });
            }
            ViewBag.Products = Products;
        }

        public void WarrantyYearsToList()
        {
            List<SelectListItem> Years = new List<SelectListItem>();
            int yId = 0;
            int[] years = { 1, 2, 3, 4, 5 };

            foreach (var item in years)
            {
                Years.Add(
                    new SelectListItem
                    {
                        Text = item.ToString() + " yıl",
                        Value = item.ToString(),
                        Selected = (item == yId ? true : false),
                    });
            }
            ViewBag.Years = Years;
        }
    }
}
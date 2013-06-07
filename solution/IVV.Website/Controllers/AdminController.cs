using IVV.Website.ClassLib;
using IVV.Website.DataModel;
using IVV.Website.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVV.Website.Controllers {

	[Authorize]
	[ValidateInput(false)]
    public class AdminController : BaseController {

		#region 登录

		[AllowAnonymous]
		public ActionResult Login() {
			return View("Login", new LoginModel());
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(LoginModel model) {
			JsonData reply;

			if (!ModelState.IsValid) {
				reply = new JsonData(ModelState);
				return Json(reply);
			}

			SysUser user = 
				context.SysUser.FirstOrDefault(x => x.UserName == model.UserName);

			if (user == null) {
				ModelState.AddModelError("UserName", "不存在此用户名");
			} else if (user.PWD != model.Password) {
				ModelState.AddModelError("Password", "密码输入不正确");
			}

			object imgCode = Session["ImgCode"];

			if (imgCode.ToString() != model.CheckCode.ToUpper()) {
				ModelState.AddModelError("CheckCode", "验证码输入有误");
			}

			if (!ModelState.IsValid) {
				reply = new JsonData(ModelState);
				return Json(reply);
			}

			SetUserData(model.UserName, "");

			reply = new JsonData(Url.Content("Index.aspx"));
			return Json(reply);
		}

		#endregion

		public ActionResult Company() {
			base.SetSubNavId((int)SubNavGroup.SiteConfig);
			SiteInfo item = context.SiteInfo.First();
			CompanyModel model = new CompanyModel(item);
			return View("Company", model);
		}
		
		[HttpPost]
		public ActionResult Company(CompanyModel model) {			
			JsonData reply;
			if (!ModelState.IsValid) {
				reply = new JsonData(ModelState);
				return Json(reply);
			}

			try {
				SiteInfo item = context.SiteInfo.First();
				model.SetEntity(item);
				context.SaveChanges();
			} catch(Exception ex) {
				reply = new JsonData(false, ex.Message);
				return Json(reply);
			}

			reply = new JsonData(true, "更新公司信息成功！");
			JsonResult r = Json(reply);
			string a = r.ToString();

			return Json(reply);
		}

		public ActionResult Copyright() {
			base.SetSubNavId((int)SubNavGroup.SiteConfig);
			SiteInfo item = context.SiteInfo.First();
			CompanyModel model = new CompanyModel(item);
			return View("Copyright", model);
		}
		
		[HttpPost]
		public ActionResult Copyright(string copyright) {			
			JsonData reply;
			
			try {
				SiteInfo item = context.SiteInfo.First();
				item.Copyright = copyright;
				context.SaveChanges();
			} catch(Exception ex) {
				reply = new JsonData(false, ex.Message);
				return Json(reply);
			}

			reply = new JsonData(true, "更新公司信息成功！");
			JsonResult r = Json(reply);
			string a = r.ToString();

			return Json(reply);
		}

		public ActionResult Index() {
            return View("Index");
        }


		public ActionResult ProductCategory() {
			return View();
		}


		public ActionResult ImageUpload() {
			HttpFileCollectionBase fc = Request.Files;
			NameValueCollection c = Request.Form;

			foreach (string key in c.Keys) {				
				string a = c[key];
			}
			return Json(new { name = "abc" }, JsonRequestBehavior.AllowGet);
		}


		public ActionResult KendoUI() {
			return View("KendoUI");
		}

		public ActionResult News(int? pageIndex,string arcTitle) {
			pageIndex = pageIndex ?? 1;

			var list = context.Post.AsQueryable();
            
			if (!string.IsNullOrEmpty(arcTitle)) {
                list = list.Where(t => t.Title.Contains(arcTitle));
            }

            ViewData["list"] = new PageList<Post>( list.OrderByDescending(t => t.Id).ThenByDescending(t => t.CreateDate)
													, pageIndex.Value,10);

            
            ViewData["pageIndex"] = pageIndex;
			ViewData["arcTitle"] = arcTitle;

            return View();
		}

		public ActionResult AddNews(int? id) {
			var m = new Post();
			if (id.HasValue) {
				var old = context.Post.SingleOrDefault(t => t.Id == id.Value);
				m = old ?? m;
			}
			return View(m);
		}

		[HttpPost]
		public ActionResult AddNews(Post model) {
			
			if (!ModelState.IsValid) {
				return View();
			}
			if (model.Id == 0) {
				try {
					model.CreateDate = DateTime.Now;
					model.Poster = "Admin";
					context.Post.Add(model);
					context.SaveChanges();
				}
				catch (Exception ex) {
					ModelState.AddModelError("DbError", ex.Message);
				}
			}
			else {
				var old = context.Post.SingleOrDefault(t => t.Id == model.Id);
				old.Title = model.Title;
				old.Content = model.Content;
				old.UpdateDate = DateTime.Now;
				context.SaveChanges();
			}
			return View();
		}

		public ActionResult DelNews(int id) {
			var m = context.Post.SingleOrDefault(t => t.Id == id);
			if (m != null) {
				context.Post.Remove(m);
				context.SaveChanges();
			}
			return RedirectToAction("News");
		}

		public ActionResult Upload() {
			return View("Uploadify");
		}
    }
}

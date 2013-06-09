using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVV.Website.ClassLib;
using IVV.Website.DataModel;

namespace IVV.Website.Controllers {

	[AllowAnonymous]
	public class WebsiteController : Controller {

		

		IVV.Website.DataModel.SiteEntity context;

		public WebsiteController() {
			context = new DataModel.SiteEntity();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext) {
			var ck = "Http500ErrMsg";
			if (filterContext.Exception != null) {
				var internalError = 
					string.Format("<div>{0}</div><div>{1}</div>",filterContext.Exception.Message,filterContext.Exception.StackTrace);
				if (HttpRuntime.Cache[ck] != null) {
					HttpRuntime.Cache.Remove(ck);
				}
				HttpRuntime.Cache.Insert(ck, internalError);
				filterContext.HttpContext.Response.Redirect("/error.aspx");
			}

		}
		protected override void OnResultExecuted(ResultExecutedContext filterContext) {
			var ck = "Http500ErrMsg";
			if (filterContext.Exception != null) {
				var internalError = 
					string.Format("<div>{0}</div><div>{1}</div>",filterContext.Exception.Message,filterContext.Exception.StackTrace);
				
				if (HttpRuntime.Cache[ck] != null) {
					HttpRuntime.Cache.Remove(ck);
				}

				HttpRuntime.Cache.Insert(ck, internalError);
				
				filterContext.HttpContext.Response.Redirect("/error.aspx");

				
			}

		}


		public ActionResult Index() {
			return Redirect("~/index.html");
		}
		public ActionResult Error() {
			var ck = "Http500ErrMsg";
			if (HttpRuntime.Cache[ck] != null) {
				ViewData[ck] = HttpRuntime.Cache[ck];
					HttpRuntime.Cache.Remove(ck);
				}
			return View();
		}
		public ActionResult Product() {
			return View("Product");
		}

		public ActionResult Menu(string name) {
			ViewBag.Selected = name;
			return View("_Menu");
		}

		public ActionResult ProductList(string s,string type,int? c,string sc,string color) {
			var cateList = context.ProductCategory.Where(t => t.ParentId == null);
			if (!string.IsNullOrEmpty(s)) {
				var si = IVV.Website.ClassLib.EnumHelper.GetEnumKey<IVV.Website.ClassLib.ProductSeries>(s);
				cateList = cateList.Where(t => t.Series == si);
			}
			var m = new IVV.Website.Models.ProductModel();

			m.CategoryList = cateList;
			var subCates = new List<string>();
			context.Product.Select(t => new { Sn = t.SubCategory, Cid = t.CategoryId })
							.Distinct().ForEach(a => {
								subCates.Add(string.Format("{0}*$*{1}",a.Cid,a.Sn));
							});
			m.SubCategoryList = subCates;
			m.ColorList = context.Product.Select(t => t.Color).Distinct().ToList();

			if (type == "in") {
				var plist = context.Product.AsQueryable();
				if (c.HasValue) {
					plist = plist.Where(t => t.CategoryId == c.Value);
				}
				if (!string.IsNullOrEmpty(sc)) {
					plist = plist.Where(t => t.SubCategory == sc);
				}
				if (!string.IsNullOrEmpty(color)) {
					plist = plist.Where(t => t.Color == color);
				}
				m.ProductList = plist;
				m.ColorList = plist.Select(t => t.Color).Distinct().ToList();
			}
			return View(m);
		}

		public ActionResult ProductItem(string s,string type,int? c,int id) {
			var cateList = context.ProductCategory.Where(t => t.ParentId == null);
			if (!string.IsNullOrEmpty(s)) {
				var si = IVV.Website.ClassLib.EnumHelper.GetEnumKey<IVV.Website.ClassLib.ProductSeries>(s);
				cateList = cateList.Where(t => t.Series == si);
			}
			var m = new IVV.Website.Models.ProductModel();

			m.CategoryList = cateList;
			var subCates = new List<string>();
			context.Product.Select(t => new { Sn = t.SubCategory, Cid = t.CategoryId })
							.Distinct().ForEach(a => {
								subCates.Add(string.Format("{0}*$*{1}",a.Cid,a.Sn));
							});
			m.SubCategoryList = subCates;
			m.ColorList = context.Product.Select(t => t.Color).Distinct().ToList();

			if (type == "in" && c.HasValue) {
				m.ProductList = context.Product.Where(t => t.CategoryId == c.Value);
			}
			m.Item = context.Product.SingleOrDefault(t => t.Id == id);
			
			return View(m);
		}

		public ActionResult Maintenance() {
			return View("Maintenance");
		}

		public ActionResult Company() {
			var m = context.SiteInfo.First();
			return View(m);
		}

		public ActionResult News(int? pageIndex) {
			pageIndex = pageIndex ?? 1;

			var list = context.Post.AsQueryable();
            
			

            ViewData["list"] = new PageList<Post>( list.OrderByDescending(t => t.Id).ThenByDescending(t => t.CreateDate)
													, pageIndex.Value,10);

            
            ViewData["pageIndex"] = pageIndex;

            return View();
		}

		public ActionResult ReadNews(int id) {
			var m = context.Post.SingleOrDefault(t => t.Id == id);
			return View(m);
		}

		public ActionResult Download() {
			return View("Download");
		}

		public ActionResult Video(int? pageIndex) {
			pageIndex = pageIndex ?? 1;

			var list = context.Video.AsQueryable();
            ViewData["list"] = new PageList<Video>( list.OrderByDescending(t => t.Id)
													, pageIndex.Value,10);
            ViewData["pageIndex"] = pageIndex;
            return View();
		}

		public ActionResult Gallery() {
			return View("Gallery");
		}

		public ActionResult Contact() {
			var m = new NoteBook();
			return View(m);
		}

		[HttpPost]
		public ActionResult Contact(NoteBook model) {
			model.CreateDate = DateTime.Now;
			object imgCode = Session["ImgCode"];

			if (imgCode.ToString() != model.ValCode.ToUpper()) {
				ModelState.AddModelError("ValCode", "验证码输入有误");
			}

			if (!ModelState.IsValid) { 
				return View();
			}
			try {
				context.NoteBook.Add(model);
				context.SaveChanges();
				TempData["SuccessMsg"] = "提交成功！我们将在第一时间和您联系！";
			}
			catch (Exception ex) {
				ModelState.AddModelError("DbError", ex.Message);
				return View();
			}

			return RedirectToAction("Contact");
			
		}

		public ActionResult Stores() {
			return View("Stores");
		}

	}
}

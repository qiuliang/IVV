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

		public ActionResult Index() {
			return Redirect("~/index.html");
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
			return View("Contact");
		}

		public ActionResult Stores() {
			return View("Stores");
		}

	}
}

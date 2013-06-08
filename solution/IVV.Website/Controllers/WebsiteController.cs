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

		public ActionResult ProductList() {
			return View("ProductList");
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

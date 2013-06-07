using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVV.Website.Controllers {

	[AllowAnonymous]
	public class WebsiteController : Controller {

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
			return View("Company");
		}

		public ActionResult News() {
			return View("News");
		}

		public ActionResult Download() {
			return View("Download");
		}

		public ActionResult Video() {
			return View("Video");
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

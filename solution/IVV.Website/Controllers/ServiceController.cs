using IVV.Website.ClassLib;
using IVV.Website.DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IVV.Website.Controllers
{
    public class ServiceController : Controller {

        public ActionResult CheckImage() {
			string value;
			ImageCoder img = new ImageCoder();
			byte[] bytes = img.GetImageByte(out value);
			Session["ImgCode"] = value;

			return File(bytes, "image/png"); 
		}

		
		public JsonResult GetFooter() {
			var db = new SiteEntity();
			var info = db.SiteInfo.FirstOrDefault();
			if (info == null) return null;
			return Json(new { Copyright = info.Copyright}, JsonRequestBehavior.AllowGet);
		}

    }
}

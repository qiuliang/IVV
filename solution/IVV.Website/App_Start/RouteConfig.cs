using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IVV.Website {
	public class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "root",
				url: "",
				defaults: new { controller = "Website", action = "Index" }
			);

			

			routes.MapRoute(
				name: "Website",
				url: "{action}.aspx",
				defaults: new { controller = "Website", action = "Index" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}.aspx",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "Admin",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
			
		}
	}
}
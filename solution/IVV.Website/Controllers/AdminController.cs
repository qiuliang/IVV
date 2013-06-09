using IVV.Website.ClassLib;
using IVV.Website.DataModel;
using IVV.Website.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

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
			ModelState.Remove("NewPassword");
			ModelState.Remove("ConfirmPassword");
			if (!ModelState.IsValid) {
				reply = new JsonData(ModelState);
				return Json(reply);
			}

			//SysUser user = 
			//	context.SysUser.FirstOrDefault(x => x.UserName == model.UserName);

			//if (user == null) {
			//	ModelState.AddModelError("UserName", "不存在此用户名");
			//} else if (user.PWD != model.Password) {
			//	ModelState.AddModelError("Password", "密码输入不正确");
			//}
			if (model.UserName != "Admin") { 
				ModelState.AddModelError("UserName", "不存在此用户名");
			}
			if (model.Password.ToMd5() != System.Configuration.ConfigurationManager.AppSettings["SuperPwd"]) { 
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

			reply = new JsonData(Url.Content("Company.aspx"));
			return Json(reply);
		}

		public void SignOut() {
			Session.Abandon();
			Session.Clear();
			// 获得Cookie
			HttpCookie authCookie = new HttpCookie(".ASPXAUTH");
			authCookie.Expires = DateTime.Now.AddDays(-1);

			
			Response.Cookies.Add(authCookie);
			Response.Redirect("/admin/login.aspx");
		}
		#endregion

		#region 网站设置
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

		public ActionResult UpdatePwd() {
			return View();
		}

		[HttpPost]
		public ActionResult UpdatePwd(LoginModel m) {
			ModelState.Remove("CheckCode");
			ModelState.Remove("UserName");
			if (!ModelState.IsValid) {
				return View();
			}

			if (m.Password.ToMd5() != System.Configuration.ConfigurationManager.AppSettings["SuperPwd"]) {
				ModelState.AddModelError("Password", "旧密码不正确");
                return View();
            }

			SetValue(Request.PhysicalApplicationPath, m.NewPassword.ToMd5());

            TempData[StaticDefination.TmpErrMsg] = "密码修改成功";
            return View();
		}
		private string Config(string path) {

           return System.IO.Path.Combine(path, "web.config");//此处配置文件在程序目录下
       }

        private void SetValue(string path, string AppValue)
       {
           string configPath = Config(path);
           XmlDocument xDoc = new XmlDocument();
           xDoc.Load(configPath);
           XmlNode xNode;
           XmlElement xElem1;
           XmlElement xElem2;
           xNode = xDoc.SelectSingleNode("//appSettings");
           xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='SuperPwd']");
           if (xElem1 != null)
           {
               string name = xElem1.GetAttribute("value");

               xElem1.SetAttribute("value", AppValue);
           }
           else
           {
               xElem2 = xDoc.CreateElement("add");
               xElem2.SetAttribute("key", "SuperAdminPassword");
               xElem2.SetAttribute("value", AppValue);
               xNode.AppendChild(xElem2);
           }
           xDoc.Save(configPath);
       }

		#endregion

		#region 新闻管理
		
		
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
					return View();
				}
			}
			else {
				var old = context.Post.SingleOrDefault(t => t.Id == model.Id);
				old.Title = model.Title;
				old.Content = model.Content;
				old.UpdateDate = DateTime.Now;
				context.SaveChanges();
			}
			TempData[StaticDefination.TmpSuccessMsg] = "操作成功！";
			return RedirectToAction("News");
		}

		public ActionResult DelNews(int id) {
			var m = context.Post.SingleOrDefault(t => t.Id == id);
			if (m != null) {
				context.Post.Remove(m);
				context.SaveChanges();
			}
			return RedirectToAction("News");
		}
		#endregion

		#region 产品管理
		public ActionResult Product(int? pageIndex,string arcTitle) {
			pageIndex = pageIndex ?? 1;

			var list = context.Product.AsQueryable();
            
			if (!string.IsNullOrEmpty(arcTitle)) {
                list = list.Where(t => t.Name.Contains(arcTitle));
            }

            ViewData["list"] = new PageList<Product>( list.OrderByDescending(t => t.Id)
													, pageIndex.Value,10);

            
            ViewData["pageIndex"] = pageIndex;
			ViewData["arcTitle"] = arcTitle;

			var allCateList = new HashSet<ProductCategory>();
			context.ProductCategory.Where(t => t.ParentId == null)
				.ForEach(t => {
					allCateList.Add(t);
				});
			ViewData["allCateList"] = allCateList;

            return View();
		}

		public ActionResult EditProduct(int? id) { 
			var m = new Product();
			if (id.HasValue) {
				var old = context.Product.SingleOrDefault(t => t.Id == id.Value);
				m = old ?? m;
			}
			m.CategoryList = context.ProductCategory.Where(t => t.ParentId == null);

			var selCateList = new List<SelectListItem>();
			context.ProductCategory.Where(t => t.ParentId == null)
									.ForEach(t => 
										selCateList.Add(new SelectListItem(){
											Text = t.Name,
											Value = t.Id.ToString()											
										})
									);
			ViewData["catlist"] = selCateList;	
			return View(m);
		}

		[HttpPost]
		public ActionResult EditProduct(Product model) {
			
			if (!ModelState.IsValid) {
				return View();
			}
			if (model.Id == 0) {
				try {
					context.Product.Add(model);
					context.SaveChanges();
				}
				catch (Exception ex) {
					ModelState.AddModelError("DbError", ex.Message);
				}
			}
			else {
				var old = context.Product.SingleOrDefault(t => t.Id == model.Id);
				old.Name = model.Name;
				old.CategoryId = model.CategoryId;
				old.SubCategory = model.SubCategory;
				old.Color = model.Color;
				old.ImgUrl = model.ImgUrl;
				old.ThumbnailUrl = model.ThumbnailUrl;
				context.SaveChanges();
			}
			return RedirectToAction("Product");
		}

		public ActionResult DelProduct(int id) {
			var m = context.Product.SingleOrDefault(t => t.Id == id);
			if (m != null) {
				context.Product.Remove(m);
				context.SaveChanges();
			}
			return RedirectToAction("Product");
		}

		public ActionResult ProductCategory(int? pageIndex,string arcTitle) {
			pageIndex = pageIndex ?? 1;

			var list = context.ProductCategory.AsQueryable();
            
			if (!string.IsNullOrEmpty(arcTitle)) {
                list = list.Where(t => t.Name.Contains(arcTitle));
            }

            ViewData["list"] = new PageList<ProductCategory>( list.OrderByDescending(t => t.Id)
													, pageIndex.Value,10);

            
            ViewData["pageIndex"] = pageIndex;
			ViewData["arcTitle"] = arcTitle;

            return View();
		}

		public ActionResult EditProductCategory(int? id,int? pid) { 
			var m = new ProductCategory();
			if (id.HasValue) {
				var old = context.ProductCategory.SingleOrDefault(t => t.Id == id.Value);
				m = old ?? m;
			}
			if (pid.HasValue) {
				m.ParentId = pid.Value;
				m.ParentNode = context.ProductCategory.SingleOrDefault(t => t.Id == pid.Value);
			}
			return View(m);
		}

		[HttpPost]
		public ActionResult EditProductCategory(ProductCategory model) {
			
			if (!ModelState.IsValid) {
				return View();
			}
			if (model.Id == 0) {
				try {
					context.ProductCategory.Add(model);
					context.SaveChanges();
				}
				catch (Exception ex) {
					ModelState.AddModelError("DbError", ex.Message);
					return View();
				}
			}
			else {
				var old = context.ProductCategory.SingleOrDefault(t => t.Id == model.Id);
				old.Name = model.Name;
				old.Series = model.Series;
				old.Cover = model.Cover;
				old.ParentId = model.ParentId;
				context.SaveChanges();
				
			}
			TempData[StaticDefination.TmpSuccessMsg] = "操作成功！";
			return RedirectToAction("ProductCategory");
		}

		public ActionResult DelProductCategory(int id) {
			var m = context.ProductCategory.SingleOrDefault(t => t.Id == id);
			if (m != null) {
				var isRef = context.Product.Where(t => t.CategoryId == m.Id).Count() > 0;
				if (isRef) {
					TempData[IVV.Website.ClassLib.StaticDefination.TmpErrMsg] = "该类别被产品引用，不能删除";
					return RedirectToAction("ProductCategory");
				}
				context.ProductCategory.Remove(m);
				context.SaveChanges();
			}
			TempData[StaticDefination.TmpSuccessMsg] = "操作成功！";
			return RedirectToAction("ProductCategory");
		}

		#endregion

		#region 视频管理
		
		
		public ActionResult Video(int? pageIndex,string arcTitle) {
			pageIndex = pageIndex ?? 1;

			var list = context.Video.AsQueryable();
            
			if (!string.IsNullOrEmpty(arcTitle)) {
                list = list.Where(t => t.Title.Contains(arcTitle));
            }

            ViewData["list"] = new PageList<Video>( list.OrderByDescending(t => t.Id)
													, pageIndex.Value,10);

            
            ViewData["pageIndex"] = pageIndex;
			ViewData["arcTitle"] = arcTitle;

            return View();
		}

		public ActionResult EditVideo(int? id) { 
			var m = new Video();
			if (id.HasValue) {
				var old = context.Video.SingleOrDefault(t => t.Id == id.Value);
				m = old ?? m;
			}
			return View(m);
		}

		[HttpPost]
		public ActionResult EditVideo(Video model) {
			
			if (!ModelState.IsValid) {
				return View();
			}
			if (model.Id == 0) {
				try {
					context.Video.Add(model);
					context.SaveChanges();
				}
				catch (Exception ex) {
					ModelState.AddModelError("DbError", ex.Message);
					return View();
				}
			}
			else {
				var old = context.Video.SingleOrDefault(t => t.Id == model.Id);
				old.Title = model.Title;
				old.Description = model.Description;
				old.HtmlCode = model.HtmlCode;
				old.ImgUrl = model.ImgUrl;
				context.SaveChanges();
			}
			TempData[StaticDefination.TmpSuccessMsg] = "操作成功！";
			return RedirectToAction("Video");
		}

		public ActionResult DelVideo(int id) {
			var m = context.Video.SingleOrDefault(t => t.Id == id);
			if (m != null) {
				context.Video.Remove(m);
				context.SaveChanges();
				TempData[StaticDefination.TmpSuccessMsg] = "操作成功！";
			}
			return RedirectToAction("Video");
		}
		#endregion

		#region 留言管理
		
		
		public ActionResult NoteBook(int? pageIndex,string arcTitle) {
			pageIndex = pageIndex ?? 1;

			var list = context.NoteBook.AsQueryable();
            
			

            ViewData["list"] = new PageList<NoteBook>( list.OrderByDescending(t => t.Id)
													, pageIndex.Value,10);

            
            ViewData["pageIndex"] = pageIndex;

            return View();
		}

		public ActionResult NotebookDetail(int id) {
			var m = context.NoteBook.SingleOrDefault(t => t.Id == id);
			return View(m);
		}

		public ActionResult EditNoteBook(int? id) { 
			var m = new NoteBook();
			if (id.HasValue) {
				var old = context.NoteBook.SingleOrDefault(t => t.Id == id.Value);
				m = old ?? m;
			}
			return View(m);
		}

		[HttpPost]
		public ActionResult EditNoteBook(NoteBook model) {
			
			if (!ModelState.IsValid) {
				return View();
			}
			if (model.Id == 0) {
				try {
					context.NoteBook.Add(model);
					context.SaveChanges();
				}
				catch (Exception ex) {
					ModelState.AddModelError("DbError", ex.Message);
				}
			}
			else {
				var old = context.NoteBook.SingleOrDefault(t => t.Id == model.Id);
				old.Name = model.Name;
				old.Surname = model.Surname;
				old.Company = model.Company;
				old.Country = model.Country;
				old.Email = model.Email;
				old.Message = model.Message;
				old.Phone = model.Phone;
				context.SaveChanges();
			}
			return View();
		}

		public ActionResult DelNoteBook(int id) {
			var m = context.NoteBook.SingleOrDefault(t => t.Id == id);
			if (m != null) {
				context.NoteBook.Remove(m);
				context.SaveChanges();
			}
			return RedirectToAction("NoteBook");
		}
		#endregion

		#region 上传组件
		
		
		public ActionResult Upload() {
			return View("Uploadify");
		}
		public JsonResult ImageUpload() {
            string path = "/content/uploads/";

			var futil = new FileUtil();
            futil.HttpPostedFile = Request.Files["FileData"];
            futil.RootDirect = path;
            
            var fp = futil.SaveByDay();
			return Json(new { path = fp }, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region 其他
		public ActionResult Index() {
            return View("Index");
        }

		public ActionResult KendoUI() {
			return View("KendoUI");
		}

		public ActionResult DelAll(string t,string ids) {
			var arr = ids.Split(',');
			if (t == "news") {
				arr.ForEach(tt => {
					var id1 = int.Parse(tt);
					var m = context.Post.SingleOrDefault(a => a.Id == id1);
					if (m != null) {
						context.Post.Remove(m);
					}
				});
			}
			else if (t == "p") { 
				arr.ForEach(tt => {
					var id = int.Parse(tt);
					var m = context.Product.SingleOrDefault(a => a.Id == id);
					if (m != null) {
						context.Product.Remove(m);
					}
				});
			}
			else if (t == "pc") { 
				arr.ForEach(tt => {
					var id = int.Parse(tt);
					var m = context.ProductCategory.SingleOrDefault(a => a.Id == id);
					if (m != null) {
						var isRef = context.Product.Where(b => b.CategoryId == m.Id).Count() > 0;
						if(!isRef)	context.ProductCategory.Remove(m);
					}
				});
			}
			else if (t == "v") { 
				arr.ForEach(tt => {
					var id = int.Parse(tt);
					var m = context.Video.SingleOrDefault(a => a.Id == id);
					if (m != null) {
						context.Video.Remove(m);
					}
				});
			}
			else if (t == "n") { 
				arr.ForEach(tt => {
					var id = int.Parse(tt);
					var m = context.NoteBook.SingleOrDefault(a => a.Id == id);
					if (m != null) {
						context.NoteBook.Remove(m);
					}
				});
			}
			context.SaveChanges();
			return Json(new { Result = true},JsonRequestBehavior.AllowGet);
		}
		#endregion
		
    }
}

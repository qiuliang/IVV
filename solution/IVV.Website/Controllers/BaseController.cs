using IVV.Website.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IVV.Website.Controllers {
	public class BaseController:Controller {
		protected SiteEntity context = new SiteEntity();

		// 将用户保存到Session中 
		protected void saveUserToSession(string loginName, object userData) {
			SysUser user = context.SysUser.First(x => x.UserName == loginName);
			user.LastActiveTime = DateTime.Now;
			Session["SysUser"] = user;
		}

		/// <summary>
		/// 获取当前登录用户
		/// </summary>
		protected SysUser GetUser() {

			SysUser sessionUser = Session["SysUser"] as SysUser;

			if (sessionUser == null) {
				if (String.IsNullOrEmpty(User.Identity.Name)) {
					return null;
				} else {
					FormsIdentity identity = User.Identity as FormsIdentity;
					saveUserToSession(identity.Name, identity.Ticket.UserData);
				}
			} else {
				TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
				TimeSpan ts2 = new TimeSpan(sessionUser.LastActiveTime.Ticks);
				TimeSpan ts = ts1.Subtract(ts2).Duration();
				int min = ts.Minutes;
				if (min > 20) {		// 可配置项
					// 超过20分钟不活跃，则跳转到登录页
					return null;
				} else {
					sessionUser.LastActiveTime = DateTime.Now;
					Session["SysUser"] = sessionUser;
				}
			}
			return Session["SysUser"] as SysUser;
		}

		/// <summary>
		/// 经用户信息写入到Cookie中
		/// </summary>
		protected void SetUserData(string loginName, string userData) {
			// 获得Cookie
			HttpCookie authCookie = FormsAuthentication.GetAuthCookie(loginName, true);

			// 得到ticket凭据
			FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

			// 根据之前的ticket凭据创建新ticket凭据，然后加入自定义信息
			FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
				ticket.Version, ticket.Name, ticket.IssueDate,
				ticket.Expiration, ticket.IsPersistent, userData);

			// 将新的Ticke转变为Cookie值，然后添加到Cookies集合中
			authCookie.Value = FormsAuthentication.Encrypt(newTicket);
			Response.Cookies.Add(authCookie);

			saveUserToSession(loginName, userData);
		}

		protected void SetSubNavId(int id) {
			TempData["SubNavId"] = id;
		}
	}

	/// <summary>
	/// 用于控制左侧导航render
	/// </summary>
	public enum SubNavGroup {
 		SiteConfig = 1,
		News = 2
	}
}
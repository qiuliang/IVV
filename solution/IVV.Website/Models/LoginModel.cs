using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IVV.Website.Models {
	public class LoginModel {

		[Required(ErrorMessage="请输入用户名")]
		public string UserName { get; set; }

		[Required(ErrorMessage="请输入密码")]
		public string Password { get; set; }

		// 验证码
		[Required(ErrorMessage="请输入验证码")]
		public string CheckCode { get; set; }
	}
}
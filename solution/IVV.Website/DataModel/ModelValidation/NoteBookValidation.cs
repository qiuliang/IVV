using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	public class NoteBookValidation {
		[Required(ErrorMessage="姓名不能为空")]
		[Display(Name="姓名")]
		public string Name { get; set; }

		[Required(ErrorMessage="手机不能为空")]
		[Display(Name="手机")]
		public string Phone { get; set; }

		[Required(ErrorMessage="Email不能为空")]
		[Display(Name="Email")]
		[RegularExpression(@"^([A-Z0-9a-z_]([._-]?[A-Z0-9a-z])*@[A-Z0-9a-z]([-_]?[A-Z0-9a-z])*[.][A-Z0-9a-z]([._-]?[A-Z0-9a-z])*)", ErrorMessage = "邮箱格式不正确")]
		public string Email { get; set; }

		[Required(ErrorMessage="留言不能为空")]
		[Display(Name="留言")]
		public string Message { get; set; }

		[Required(ErrorMessage="验证码不能为空")]
		public string ValCode { get; set; }
	}
}
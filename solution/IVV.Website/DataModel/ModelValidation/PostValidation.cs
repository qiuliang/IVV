using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	public class PostValidation {
		[Required(ErrorMessage="标题不能为空")]
		[Display(Name="标题")]
		public string Title { get; set; }

		[Required(ErrorMessage="正文不能为空")]
		[Display(Name="正文")]
		public string Content { get; set; }
	}
}
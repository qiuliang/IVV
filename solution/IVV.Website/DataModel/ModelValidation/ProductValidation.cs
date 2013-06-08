using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	public class ProductValidation {
		[Required(ErrorMessage="名称不能为空")]
		[Display(Name="名称")]
		public string Name { get; set; }

		[Required(ErrorMessage="产品图不能为空")]
		[Display(Name="产品图")]
		public string ImgUrl { get; set; }

		[Display(Name="颜色")]
		public string Color { get; set; }

		[Display(Name="类别")]
		public int CategoryId { get; set; }

		[Display(Name="子类别")]
		public string SubCategory { get; set; }
		
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	public class ProductCategoryValidation {
		[Required(ErrorMessage="名称不能为空")]
		[Display(Name="名称")]
		public string Name { get; set; }

		
	}
}
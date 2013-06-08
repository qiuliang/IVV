using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	[MetadataType(typeof(ProductCategoryValidation))]
	public partial class ProductCategory {
		public ProductCategory ParentNode { get; set; }

		public IEnumerable<ProductCategory> ChildNodes { get; set; }
	}
}
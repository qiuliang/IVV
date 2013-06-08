using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IVV.Website.DataModel;

namespace IVV.Website.Models {
	public class ProductModel {

		public IEnumerable<ProductCategory> CategoryList { get; set; }
		public IEnumerable<string> SubCategoryList { get; set; }
		public IEnumerable<string> ColorList { get; set; }
		public IEnumerable<Product> ProductList { get; set; }
		public Product Item { get; set; }
	}
}
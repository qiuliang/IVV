using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	[MetadataType(typeof(ProductValidation))]
	public partial class Product {
		public IEnumerable<ProductCategory> CategoryList { get; set; }
	}
}
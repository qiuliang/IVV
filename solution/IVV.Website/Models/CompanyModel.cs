using IVV.Website.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IVV.Website.Models {
	public class CompanyModel {

		[Required(ErrorMessage="请输入公司介绍")]
		public string CompanyInfo { get; set; }

		public string Copyright { get; set; }

		public void SetEntity(SiteInfo item) {
			item.Company = this.CompanyInfo;			
		}

		public CompanyModel(SiteInfo item) {
			this.CompanyInfo = item.Company;
			this.Copyright = item.Copyright;
		}

		public CompanyModel() { }
	}
}
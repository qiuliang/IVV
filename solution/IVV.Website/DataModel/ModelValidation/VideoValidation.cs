using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IVV.Website.DataModel {
	public class VideoValidation {
		[Required(ErrorMessage="标题不能为空")]
		[Display(Name="标题")]
		public string Title { get; set; }

		[Required(ErrorMessage="视频内容不能为空")]
		[Display(Name="视频播放代码")]
		public string HtmlCode { get; set; }

		[Required(ErrorMessage="预览图片不能为空")]
		[Display(Name="预览图")]
		public string ImgUrl { get; set; }

		[Display(Name="内容简介")]
		public string Description { get; set; }
	}
}
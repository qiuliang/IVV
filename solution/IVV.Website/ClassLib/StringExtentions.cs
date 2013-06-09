using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace IVV.Website.ClassLib {
	public static class StringExtentions {
		public static string ToMd5(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = System.Text.Encoding.Default.GetBytes(source);
            var hash = md5.ComputeHash(bs);

            StringBuilder s = new StringBuilder();
            foreach (var item in hash) {
                s.Append(item.ToString("x"));
            }

            return s.ToString();
        }

		public static string SubString(string source,int endIndex,bool filterBlank = true)
        {
            if(string.IsNullOrEmpty(source))
                return null;
            if (filterBlank) {
                source = source.Trim().Replace("&nbsp;","");
            }
            if (source.Length <= endIndex)
                return source;
            source = source.Substring(0, endIndex);
            return source + "...";
        }
	}
}
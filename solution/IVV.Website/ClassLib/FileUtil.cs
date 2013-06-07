using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace IVV.Website.ClassLib {
	/// <summary>
    /// 文件处理辅助类
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 网站相对路径，例如：“~/content/uploads/”，最后带斜杠
        /// </summary>
        public string RootDirect { get; set; }
        

        public HttpPostedFileBase HttpPostedFile { get; set; }

        public FileUtil() { }

        /// <summary>
        /// 返回写入文件的相对URL路径
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SaveByDay()
        {
            var input = HttpPostedFile.InputStream;
            var path = new StringBuilder();
            //应用程序路径
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            //网站存储相对路径
            path.Append(RootDirect);
            //新生成的动态路径
            var today = DateTime.Now.ToString("yyyyMMdd");
            path.Append(today);
            path.Append("/" );
            var returnUrlPath = RootDirect + today + "/";

            var directory = path.ToString();

            var bytes = new byte[HttpPostedFile.ContentLength];
            input.Read(bytes, 0, bytes.Length);
            //文件名
            var fn = System.Guid.NewGuid().ToString().Replace("-", "") + GetFileExtName();
            path.Append(fn);

            returnUrlPath += fn;

            if(!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            
            File.WriteAllBytes(path.ToString(), bytes);

            return returnUrlPath;
        }

        private string GetFileExtName()
        {
            var filename = HttpPostedFile.FileName;

            filename = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.'));
            return filename;
        }
    }
}
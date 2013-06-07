using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IVV.Website.ClassLib {

	public class ImageCoder {

		public Bitmap GetImage(out string value) {
			int width = 90;
			int height = 24;

			Bitmap bm = new Bitmap(width, height);
			
			Graphics graph = Graphics.FromImage(bm);
			graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			graph.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
			Font font = new Font(FontFamily.GenericSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
			Random r = new Random();
			string letters = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";
			string letter;
			StringBuilder sb = new StringBuilder();

			//添加随机的五个字母
			for (int x = 0; x < 5; x++) {
				letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
				sb.Append(letter);
				graph.DrawString(letter, font, new SolidBrush(Color.Black), x * 17, r.Next(0, 5));
			}

			//混淆背景
			Pen linePen = new Pen(new SolidBrush(Color.Black), 1);
			for (int x = 0; x < 4; x++) {
				graph.DrawLine(linePen,
					new Point(r.Next(0, width - 1), r.Next(0, height - 1)),
					new Point(r.Next(0, width - 1), r.Next(0, height - 1))
				);
			}

			value = sb.ToString();			

			return bm;
		}

		public Byte[] GetImageByte(out string value) {
			Bitmap bm = GetImage(out value);
			MemoryStream ms = new MemoryStream();
			bm.Save(ms, ImageFormat.Png);
			bm.Dispose();

			return ms.ToArray();
		}
	}
}
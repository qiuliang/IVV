using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace IVV.Website.ClassLib {

	public class FormFiledError {
		public FormFiledError(string key, string msg) {
			this.ElementId = key;
			this.ErrorMessage = msg;
		}
		public string ElementId { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class JsonData {
		public List<FormFiledError> ErrorList = new List<FormFiledError>();
		public bool HasFormError { get; set; }
		public bool IsRedirect { get; set; }
		public string RedirectUrl { get; set; }
		public bool IsActionSuccess { get; set; }

		public bool IsActionDone { get; set; }
		private string _actionMsg;
		public string ActionMsg {
			get { return _actionMsg; }
			set { _actionMsg = value; IsActionDone = true; }
		}

		public JsonData() {
			this.HasFormError = false;
			this.IsRedirect = false;
			this.IsActionDone = false;
		}

		public JsonData(string redirectUrl) {
			this.RedirectUrl = redirectUrl;
			this.IsRedirect = true;
		}

		public JsonData(bool isActionSuccess, string msg) {
			this.IsActionDone = true;
			this.IsActionSuccess = isActionSuccess;
			this.ActionMsg = msg;
		}

		public JsonData(ModelStateDictionary modelDic) :this() {
			List<string> errorList = new List<string>();
			List<string> keyList = new List<string>();
			List<int> errorIndexList = new List<int>();

			int i = 0;
			foreach (ModelState state in modelDic.Values) {
				if (state.Errors.Count > 0) {
					string error = state.Errors[0].ErrorMessage;
					errorList.Add(error);
					errorIndexList.Add(i);
				}
				i++;
			}

			int j = 0;
			foreach (string key in modelDic.Keys) {
				if (errorIndexList.Any(x => x == j)) {
					keyList.Add(key.Replace(".", "_"));
				}
				j++;
			}

			for (int x = 0; x <= errorList.Count - 1; x++) {
				ErrorList.Add(new FormFiledError(keyList[x], errorList[x]));
			}

			if (errorList.Count > 0) this.HasFormError = true;
		}
	}
}

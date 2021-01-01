using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Helpers {
	public static class TempDataDictionaryExtensions {
		private const string _modelStateKey = ".modelState";

		// ModelStateを追加
		public static void AddModelState(this ITempDataDictionary tempData, ModelStateDictionary modelState) {
			// TempDataがシリアライズできるのは単純データのみ
			// JSON文字列をTempDataに格納する
			var json = ModelStateDictionaryJsonSerializer.Serialize(modelState);

			tempData.Add(_modelStateKey, json);
		}

		// ModelStateを取得
		public static ModelStateDictionary GetModelState(this ITempDataDictionary tempData) {
			var json = tempData[_modelStateKey] as string;
			if (string.IsNullOrWhiteSpace(json)) {
				return null;
			}

			return ModelStateDictionaryJsonSerializer.Deserialize(json);
		}
	}
}

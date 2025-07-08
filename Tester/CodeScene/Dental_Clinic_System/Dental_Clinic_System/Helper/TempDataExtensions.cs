using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Dental_Clinic_System.Helper
{
	public static class TempDataExtensions
	{
		public static void SetObjectAsJson(this ITempDataDictionary tempData, string key, object value)
		{
			tempData[key] = JsonConvert.SerializeObject(value);
		}

		public static T GetObjectFromJson<T>(this ITempDataDictionary tempData, string key)
		{
			tempData.TryGetValue(key, out var o);
			return o == null ? default(T) : JsonConvert.DeserializeObject<T>((string)o);
		}
	}
}

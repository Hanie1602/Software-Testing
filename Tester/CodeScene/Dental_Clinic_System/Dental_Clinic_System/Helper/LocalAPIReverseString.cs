using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dental_Clinic_System.Helper
{
    public static class LocalAPIReverseString
    {
        private static readonly HttpClient client = new();

        private static string FormatId(int id, int length)
        {
            return id.ToString().PadLeft(length, '0');
        }

        private static async Task<JObject> FetchJsonData(string url)
        {
            var response = await client.GetStringAsync(url).ConfigureAwait(false);
            return JObject.Parse(response);
        }

        private static async Task<string> GetLocationName(string url, string id, string idKey)
        {
            var json = await FetchJsonData(url).ConfigureAwait(false);

            if (json["error"].ToString() == "0")
            {
                foreach (var item in json["data"])
                {
                    if (item["id"].ToString() == id)
                    {
                        return item["full_name"].ToString();
                    }
                }
            }

            return "Not Found";
        }

        public static async Task<string> GetProvinceNameById(int provinceId)
        {
            string url = "https://esgoo.net/api-tinhthanh/1/0.htm";
            string formattedProvinceId = FormatId(provinceId, 2);
            return await GetLocationName(url, formattedProvinceId, "id").ConfigureAwait(false);
        }

        public static async Task<string> GetDistrictNameById(int provinceId, int districtId)
        {
            string formattedProvinceId = FormatId(provinceId, 2);
            string url = $"https://esgoo.net/api-tinhthanh/2/{formattedProvinceId}.htm";
            string formattedDistrictId = FormatId(districtId, 3);
            return await GetLocationName(url, formattedDistrictId, "id").ConfigureAwait(false);
        }

        public static async Task<string> GetWardNameById(int districtId, int wardId)
        {
            string formattedDistrictId = FormatId(districtId, 3);
            string url = $"https://esgoo.net/api-tinhthanh/3/{formattedDistrictId}.htm";
            string formattedWardId = FormatId(wardId, 5);
            return await GetLocationName(url, formattedWardId, "id").ConfigureAwait(false);
        }
    }
}

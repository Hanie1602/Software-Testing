using System.Text;

namespace Dental_Clinic_System.Helper
{
	public static class Util
	{
		public static string GenerateRandomKey(int length = 5)
		{
			var pattern = @"asdasdasdasdasQSADAGKJPOK!";
			var sb = new StringBuilder();
			var random = new Random(length);
			for (int i = 0; i < length; i++)
			{
				sb.Append(pattern[random.Next(0, pattern.Length)]);
			}

			return sb.ToString();
		}

        public static string GenerateRandomKey(string data, int length = 5)
        {
            var pattern = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" + data;
            var sb = new StringBuilder();
            var random = new Random(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[random.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
    }
}

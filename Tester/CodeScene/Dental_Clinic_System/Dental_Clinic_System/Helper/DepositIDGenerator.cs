namespace Dental_Clinic_System.Helper
{
	public static class DepositIDGenerator
	{
		public static string GenerateDepositID()
		{
			string prefix = "DC";
			string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss"); // Format: yyyyMMddHHmmss

			string depositID = $"{prefix}{dateTime}";

			// Ensure the length is between 14 and 16 characters
			//if (depositID.Length < 14 || depositID.Length > 16)
			//{
			//	throw new Exception("Generated MedicalReportID is not within the required length range.");
			//}

			return depositID;
		}
	}
}

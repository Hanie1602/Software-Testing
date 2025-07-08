namespace Dental_Clinic_System.Services.VNPAY
{
    public class VNPaymentRequestModel()
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        // For Appointment Info
        public int ScheduleID { get; set; }
        public int PatientRecordID { get; set; }
        public int SpecialtyID { get; set; }
    }
}

namespace Dental_Clinic_System.Services.MOMO
{
    public class MOMOPaymentRequestModel
    {
        public long Amount {  get; set; }
        public string OrderInformation { get; set; }
        public string OrderID { get; set; }

        // For Appointment Info
        public int ScheduleID { get; internal set; }
        public int PatientRecordID { get; internal set; }
        public int SpecialtyID { get; internal set; }
        public string FullName { get; internal set; }

        // For Single Disbursement
        public string? RequestType { get; set; }
        public string? DisbursementMethodRSA { get; set; }

        // Disburse To Bank
        public string? BankAccountNo { get; set; } // Must be having 1 method BankAccountNo or BankCardNo
        public string? BankCardNo { get; set; }    // Must be having 1 method BankAccountNo or BankCardNo
        public string? BankAccountHolderName { get; set; }
        public string? BankCode { get; set; }

        // Disburse To Wallet
        public string? WalletId { get; set; } // Required
        public string? WalletName { get; set; }
        public long? PersonalId { get; set; }
        public bool? ValidatePersonalId { get; set; }

    }
}

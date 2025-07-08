using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Manager.ViewModels
{
    public class ClinicVM
    {
        public int ID { get; set; }

        public int ManagerID { get; set; }

        public string Name { get; set; } = null!;

        public int? Province { get; set; } = null!;

        public int? Ward { get; set; } = null!;

        public int? District { get; set; } = null!;

        public string? Address { get; set; } = null!;

        public string? Basis { get; set; } = null;

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; } = null!;

        public string ClinicStatus { get; set; } = null!;

        public string? MapLinker { get; set; }
    }
}

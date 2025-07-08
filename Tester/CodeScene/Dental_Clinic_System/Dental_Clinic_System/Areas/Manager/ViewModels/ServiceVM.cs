using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dental_Clinic_System.Areas.Manager.ViewModels
{
    public class ServiceVM
    {
        public int ID { get; set; }

        public int ClinicID { get; set; }

        public int SpecialtyID { get; set; }

        public string Name { get; set; } = null!;

        public string Price { get; set; } = null!;
        public string? Description { get; set; }
    }
}

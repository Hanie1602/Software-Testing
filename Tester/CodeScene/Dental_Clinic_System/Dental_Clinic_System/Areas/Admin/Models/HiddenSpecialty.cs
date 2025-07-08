namespace Dental_Clinic_System.Areas.Admin.Models
{
    public class HiddenSpecialtyService()
    {
        private readonly List<int> _hiddenSpeciatlies = new List<int>();

        public void HiddenSpecialty(int id)
        {
            if (!_hiddenSpeciatlies.Contains(id))
                _hiddenSpeciatlies.Add(id);
        }

        public List<int> GetHiddenSpecialty()
        {
            return _hiddenSpeciatlies;
        }
    }

}

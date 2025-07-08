using AutoMapper;
using Dental_Clinic_System.Models.Data;
using Dental_Clinic_System.ViewModels;

namespace Dental_Clinic_System.Helper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile() 
		{
			CreateMap<RegisterVM, Account>();
		}
	}
}

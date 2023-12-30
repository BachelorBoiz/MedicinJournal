using AutoMapper;
using MedicinJournal.API.Dtos;
using MedicinJournal.Core.Models;

namespace MedicinJournal.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Journal, JournalDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using MedicinJournal.API.Dtos;
using MedicinJournal.Core.Models;
using System.Text;

namespace MedicinJournal.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Journal, JournalDto>().ReverseMap();
            CreateMap<Journal, CreateJournalDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}

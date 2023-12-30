using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicinJournal.Core.Models;
using MedicinJournal.Infrastructure.Entities;

namespace MedicinJournal.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientEntity>().ReverseMap();
            CreateMap<Journal, JournalEntity>().ReverseMap();
            CreateMap<Employee, EmployeeEntity>().ReverseMap();
        }
    }
}

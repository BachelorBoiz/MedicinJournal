﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Infrastructure.Entities
{
    public class JournalEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public PatientEntity Patient { get; set; }
        public int PatientId { get; set; }
        public EmployeeEntity Doctor { get; set; }
        public int DoctorId { get; set; }
    }
}

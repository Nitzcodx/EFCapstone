using System;
using System.Collections.Generic;

#nullable disable

namespace PolyclinicDAL.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
        }

        public string DoctorId { get; set; }
        public string Specialization { get; set; }
        public string DoctorName { get; set; }
        public decimal Fees { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

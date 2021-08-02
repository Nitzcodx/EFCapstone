using System;
using System.Collections.Generic;

#nullable disable

namespace PolyclinicDAL.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }

        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public byte Age { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

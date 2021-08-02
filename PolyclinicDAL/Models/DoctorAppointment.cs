using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PolyclinicDAL.Models
{
    public class DoctorAppointment
    {
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }

        [Key]
        public int AppointmentNo { get; set; }
    }
}

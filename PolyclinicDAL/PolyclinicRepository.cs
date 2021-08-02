using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF = PolyclinicDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace PolyclinicDAL
{
    public class PolyclinicRepository
    {
        private EF.PolyclinicDBContext context;

        public PolyclinicRepository()
        {
            context = new EF.PolyclinicDBContext();
        }

        public EF.Patient GetPatientDetails(string patientId)
        {
            EF.Patient patient = null;
            try
            {
                patient = (from _pat in context.Patients
                                      where _pat.PatientId.Equals(patientId)
                                      select _pat).FirstOrDefault();
            }
            catch (Exception ex)
            {
                patient = null;
            }
            return patient;
        }

        public bool AddNewPatientDetails(EF.Patient patientObj)
        {
            bool status = false;
            try
            {
                context.Patients.Add(patientObj);
                context.SaveChanges();
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public bool UpdatePatientAge(string patientId,byte newAge)
        {
            bool status = false;
            EF.Patient patient = GetPatientDetails(patientId);
            if(patient != null)
            {
                try
                {
                        patient.Age = newAge;
                        context.SaveChanges();
                        status = true;   
                }
                catch (Exception)
                {
                    status = false;
                }
            }
            return status;
        }

        public int CancelAppointment(int appointmentNo)
        {
            EF.Appointment appointment = context.Appointments.Find(appointmentNo);
            if(appointment != null)
            {
                try
                {
                    using(var newContext = new EF.PolyclinicDBContext())
                    {
                        newContext.Appointments.Remove(appointment);
                        newContext.SaveChanges();
                        return 1;
                    }
                }
                catch (Exception)
                {
                    return -99;
                }
            }
            return -1;
        }

        public List<EF.DoctorAppointment> FetchAllAppointments(string doctorId,DateTime date)
        {
            List<EF.DoctorAppointment> appointments = null;
            SqlParameter prmDoctorId = new SqlParameter("@DoctorID", doctorId);
            SqlParameter prmDate = new SqlParameter("@DateofAppointment", date);
            try
            {
                appointments = context.DoctorAppointments.
                                FromSqlRaw("SELECT * FROM ufn_FetchAllAppointments(@DoctorID,@DateofAppointment)"
                                            , prmDoctorId, prmDate).ToList();
            }
            catch (Exception ex)
            {
                appointments = null;
            }
            return appointments;
        }

        public decimal CalculateDoctorFees(string doctorId, DateTime date)
        {
            decimal fees = 0;
            try
            {
                fees = (from s in context.Appointments
                        select EF.PolyclinicDBContext.GetDoctorFees()).FirstOrDefault();
                return fees;
            }
            catch (Exception ex)
            {

                return  -99;
            }
        }

        public int GetDoctorAppointment(string pateintID,string doctorId,DateTime dateOfAppointment,out int appointmentNo)
        {
            SqlParameter prmPatientId = new SqlParameter("@PatientID",pateintID);

            //prmPatientId.ParameterName = "@PatientID";
            //prmPatientId.SqlDbType = System.Data.SqlDbType.VarChar;
            //prmPatientId.Size = 4;
            //prmPatientId.Direction = System.Data.ParameterDirection.Input;
            //prmPatientId.Value = pateintID;

            SqlParameter prmDoctorId = new SqlParameter("@DoctorID", doctorId);

            SqlParameter prmDate = new SqlParameter("@DateOfAppointment", dateOfAppointment);

            appointmentNo = 0;
            SqlParameter prmAppoitmnetNo = new SqlParameter("@AppointmentNo", System.Data.SqlDbType.Int);
            prmAppoitmnetNo.Direction = System.Data.ParameterDirection.Output;

            int result = 0;
            SqlParameter prmReturn = new SqlParameter("@RetVal", System.Data.SqlDbType.Int);
            prmReturn.Direction = System.Data.ParameterDirection.Output;


            try
            {
                result = context.Database.ExecuteSqlRaw
                                            ("EXEC @RetVal = usp_GetDoctorAppointment @PatientID, @DoctorID, @DateOfAppointment, @AppointmentNo OUT"
                                            ,new[] { prmReturn, prmPatientId, prmDoctorId, prmDate, prmAppoitmnetNo });
                result = Convert.ToInt32(prmReturn.Value);
                if(result > 0)
                {
                    appointmentNo = Convert.ToInt32(prmAppoitmnetNo.Value);
                }
            }
            catch (Exception ex)
            {
                appointmentNo = 0;
                result = -99;
            }
            return result;
        }
    }
}

using PolyclinicDAL;
using PolyclinicDAL.Models;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestGetPatientDetails();
            //TestAddNewPatientDetails();
            //TestUpdatePatientAge();
            //TestCancelAppointment();
            //TestFetchAllAppointments();
            //TestCalculateDoctorFees();
            //TestGetDoctorAppointment();
        }

        static void TestGetPatientDetails()
        {
            
            PolyclinicRepository repository = new PolyclinicRepository();

            Console.Write("Enter Patient ID: ");
            string patientId = Console.ReadLine();

            Patient patientDetails = repository.GetPatientDetails(patientId);

            if (patientDetails == null)
            {
                Console.WriteLine("No Patient Details available withe given ID!");
            }
            else
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-10}{3,-10}{4}", "PatientId", "PatientName", "Age", "Gender", "ContactNumber");
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("{0,-15}{1,-15}{2,-10}{3,-10}{4}", patientDetails.PatientId, patientDetails.PatientName, patientDetails.Age, patientDetails.Gender, patientDetails.ContactNumber);
                Console.WriteLine();
            }
        }

        static void TestAddNewPatientDetails()
        {
            PolyclinicRepository repository = new PolyclinicRepository();
            Patient patientDetails = new Patient();

            Console.Write("Enter Patient Id : ");
            patientDetails.PatientId = Console.ReadLine();
            Console.Write("Enter Patient Name : ");
            patientDetails.PatientName = Console.ReadLine();
            Console.Write("Enter Patient Age : ");
            patientDetails.Age = Convert.ToByte(Console.ReadLine());
            Console.Write("Enter Patient Gender : ");
            patientDetails.Gender = Convert.ToString(Console.ReadLine());
            Console.Write("Enter Patient Contact Number : ");
            patientDetails.ContactNumber = Console.ReadLine();

            bool result = repository.AddNewPatientDetails(patientDetails);
            if (result)
            {
                Console.WriteLine("\nPatient details added successfully!\n");
            }
            else
            {
                Console.WriteLine("\nSome error occured...Please try again.\n");
            }
        }

        static void TestUpdatePatientAge()
        {
            PolyclinicRepository repository = new PolyclinicRepository();
            Console.Write("Enter Patient ID to update his/her age: ");
            string patientId = Console.ReadLine();
            Console.Write("Enter new age: ");
            byte age = Convert.ToByte(Console.ReadLine());

            bool result = repository.UpdatePatientAge(patientId, age);
            if (result)
            {
                Console.WriteLine("\nPatient's Age updated successfully!\n");
            }
            else
            {
                Console.WriteLine("\nSomething went wrong. Try again!\n");
            }
        }

        static void TestCancelAppointment()
        {
            PolyclinicRepository repository = new PolyclinicRepository();
            Console.Write("Enter the appointment to cancel: ");
            int appointmentNo = Convert.ToInt32(Console.ReadLine());
            int status = repository.CancelAppointment(appointmentNo);
            if (status == 1)
            {
                Console.WriteLine("\nAppointment cancelled successfully..!\n");
            }
            else
            {
                Console.WriteLine("\nSome error occurred. Try again!!\n");
            }
        }

        static void TestFetchAllAppointments()
        {
            PolyclinicRepository repository = new PolyclinicRepository();
            Console.Write("Enter DoctorId: ");
            string doctorId = Console.ReadLine();
            Console.Write("Enter Date in yyyy-mm-dd format: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            var appointmentList = repository.FetchAllAppointments(doctorId, date);
            Console.WriteLine("\n{0,-20}{1,-25}{2,-15}{3,-15}{4}", "DoctorName", "Doctor Specialization", "PatientId", "PatientName", "AppointmentNo");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            if (appointmentList.Count == 0)
            {
                Console.WriteLine("No Appointments scheduled for the given DoctorID on the given Date");
            }
            else
            {
                foreach (var item in appointmentList)
                {
                    Console.WriteLine("{0,-20}{1,-25}{2,-15}{3,-15}{4}", item.DoctorName, item.Specialization, item.PatientId, item.PatientName, item.AppointmentNo);
                }
                Console.WriteLine();
            }
        }

        static void TestCalculateDoctorFees()
        {
            PolyclinicRepository repository = new PolyclinicRepository();
            Console.Write("Enter DoctorId : ");
            string doctorId = Console.ReadLine();
            Console.Write("Enter Date in yyyy-mm-dd format: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            decimal totalFees = repository.CalculateDoctorFees(doctorId, date);
            Console.WriteLine("\nTotal Fees to be paid to the doctor for the given date: " + totalFees);
            Console.WriteLine();
        }

        static void TestGetDoctorAppointment()
        {
            PolyclinicRepository repository = new PolyclinicRepository();
            int appointmentNo = 0;
            Console.Write("Enter Patient ID: ");
            string patientId = Console.ReadLine();
            Console.Write("Enter DoctorId: ");
            string doctorId = Console.ReadLine();
            Console.Write("Enter Date of Appointment in yyyy-mm-dd format: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            int result = repository.GetDoctorAppointment(patientId, doctorId, date, out appointmentNo);
            if (result > 0)
            {
                if (result == 1)
                {
                    Console.WriteLine("\nYour Appointment is fixed with doctor successfully!");
                    Console.WriteLine("Your Appointment No is: " + appointmentNo);
                    Console.WriteLine();
                }
                else if (result == 2)
                {
                    Console.WriteLine("\nYou already have an Appointment with this doctor on the given date with Appointment No : " + appointmentNo);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\nSome error occurred. Try again!\n");
            }
        }
    }
}

using HospitalManagementSystem.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public AppointmentStatus Status { get; set; }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public virtual Visit? Visit { get; set; }

        public Appointment()
        {

        }

        [NotMapped]
        public string CommentsShort => Comments.GetShort();
    }
}

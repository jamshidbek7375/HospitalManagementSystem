namespace HospitalManagementSystem.Models;

public class DoctorSpecialization
{
    public int Id { get; set; }

    public int SpecializationId { get; set; }
    public virtual Specialization Specialization { get; set; }

    public int DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }
}

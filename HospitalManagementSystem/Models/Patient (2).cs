namespace HospitalManagementSystem.Models;

public class Patient
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly Birthdate { get; set; }
    public Gender Gender { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; }

    public Patient()
    {
        Appointments = new List<Appointment>();
    }

    public Patient(int id, string firstName, string lastName, string phoneNumber, DateOnly birthdate, Gender gender)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Birthdate = birthdate;
        Gender = gender;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} [{Id}]";
    }
}

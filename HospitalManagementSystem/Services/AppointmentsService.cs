using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services;

public class AppointmentsService
{
    private readonly HospitalDbContext _context;

    public AppointmentsService()
    {
        _context = new HospitalDbContext();
    }

    public List<Appointment> GetAppointments(string searchText = "")
    {
        var query = _context.Appointments
            .Include(x => x.Doctor)
            .Include(x => x.Patient)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x => x.Comments.Contains(searchText) ||
                x.Patient.FirstName.Contains(searchText) ||
                x.Patient.LastName.Contains(searchText) ||
                x.Doctor.FirstName.Contains(searchText) ||
                x.Doctor.LastName.Contains(searchText));
        }

        return query.ToList();
    }

    public bool DeleteAppointment(int id)
    {
        var appointmentToDelete = _context.Appointments.FirstOrDefault(x => x.Id == id);
        if (appointmentToDelete is null)
        {
            return false;
        }

        _context.Remove(appointmentToDelete);

        int affectedRows = _context.SaveChanges();
        return affectedRows > 0;
    }
}

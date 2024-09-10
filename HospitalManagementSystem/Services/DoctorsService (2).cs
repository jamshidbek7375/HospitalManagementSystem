using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services;

public class DoctorsService
{
	private readonly HospitalDbContext _context;

	public DoctorsService()
	{
		_context = new HospitalDbContext();
	}

	public List<Doctor> GetDoctors(string search = "", int? specializationId = null)
	{
		var query = _context.Doctors
			.Include(x => x.Appointments)
			.ThenInclude(a => a.Patient)
			.Include(x => x.Appointments)
			.ThenInclude(a => a.Visit)
			.AsNoTracking()
			.AsQueryable();

		if (!string.IsNullOrEmpty(search))
		{
			query = query.Where(x => x.FirstName.Contains(search) ||
				x.LastName.Contains(search) ||
				x.PhoneNumber.Contains(search));
		}

		if (specializationId is not null)
		{
			query = query.Where(x => x.Specializations.Any(s => s.SpecializationId == specializationId));
		}

		var doctors = query.ToList();

		return doctors;
	}

	public Doctor? GetDoctorById(int id)
		=> _context.Doctors.FirstOrDefault(x => x.Id == id);

	public bool Create(Doctor doctor)
	{
            _context.Doctors.Add(doctor);

            int affectedRows = _context.SaveChanges();
            return affectedRows > 0;
        }

	public bool UpdateDoctor(Doctor doctor)
	{
            var doctorToUpdate = _context.Doctors.FirstOrDefault(x => x.Id == doctor.Id);
            if (doctorToUpdate == null)
            {
                return false;
            }
            _context.Entry(doctorToUpdate).CurrentValues.SetValues(doctor);

            int affectedRows = _context.SaveChanges();
            return affectedRows > 0;
        }

	public bool DeleteDoctor(Doctor doctor)
	{
            var doctorToDelete = _context.Doctors.FirstOrDefault(x => x.Id == doctor.Id);
            if (doctorToDelete == null)
            {
                return false;
            }

            _context.Doctors.Remove(doctorToDelete);

            int affectedRows = _context.SaveChanges();
            return affectedRows > 0;
        }
}

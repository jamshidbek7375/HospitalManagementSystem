using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services
{
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

		public void Create(Doctor doctor)
		{
			_context.Doctors.Add(doctor);
			_context.SaveChanges();
		}

		public void Update(Doctor doctor)
		{
			_context.Doctors.Update(doctor);
			_context.SaveChanges();
		}

		public void Delete(Doctor doctor)
		{
			_context.Doctors.Remove(doctor);
			_context.SaveChanges();
		}
	}
}

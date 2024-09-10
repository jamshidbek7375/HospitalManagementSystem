using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services;

public class PatientsService
{
    private readonly HospitalDbContext _context;

    public PatientsService()
    {
        _context = new HospitalDbContext();
    }

    public List<Patient> GetPatients(string search = "", int? pageNumber = null, int? pageSize = null)
    {
        var query = _context.Patients
            .Include(x => x.Appointments)
            .ThenInclude(a => a.Doctor)
            .Include(x => x.Appointments)
            .ThenInclude(x => x.Visit)
            .AsNoTracking()
            .AsQueryable();

        if (pageNumber is not null && pageSize is not null)
        {
            query = _context.Patients
                .Skip((int)pageNumber).Take((int)pageSize)
                .Include(x => x.Appointments)
                .ThenInclude(a => a.Doctor)
                .Include(x => x.Appointments)
                .ThenInclude(x => x.Visit)
                .AsNoTracking()
                .AsQueryable();
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.FirstName.Contains(search) ||
                x.LastName.Contains(search) ||
                x.PhoneNumber.Contains(search));
        }


        return query.ToList();
    }

    public int GetPatientsCount(string? searchText = null)
    {
        int count = 0;
        if (!string.IsNullOrEmpty(searchText))
        {
            count = _context.Patients.Where(x => x.FirstName.Contains(searchText) ||
                x.LastName.Contains(searchText) ||
                x.PhoneNumber.Contains(searchText)).Count();

            return count;
        }
        return _context.Patients.Count();
    }

    public int GetTotalCount() => _context.Patients.Count();

    public Patient? GetPatientById(int id)
        => _context.Patients.FirstOrDefault(x => x.Id == id);

    public List<string> GetGenders()
    {
        var genderList = Enum.GetNames(typeof(Gender)).ToList();

        return genderList;
    }

    public bool Create(Patient patient)
    {
        _context.Patients.Add(patient);

        int affetedRows = _context.SaveChanges();
        return affetedRows > 0;
    }

    public bool Update(Patient patient)
    {
        var patientToUpdate = _context.Patients.FirstOrDefault(x => x.Id == patient.Id);
        if (patientToUpdate is null)
        {
            return false;
        }
        _context.Entry(patientToUpdate).CurrentValues.SetValues(patient);

        int affectedRows = _context.SaveChanges();

        return affectedRows > 0;
    }

    public bool Delete(Patient patient)
    {
        var patientToDelete = _context.Patients.FirstOrDefault(x => x.Id == patient.Id);
        if (patientToDelete is null)
        {
            return false;
        }

        _context.Remove(patientToDelete);

        int affectedRows = _context.SaveChanges();
        return affectedRows > 0;
    }
}
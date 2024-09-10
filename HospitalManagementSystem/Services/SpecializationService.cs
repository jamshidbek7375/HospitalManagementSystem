using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services;

public class SpecializationService
{
    private readonly HospitalDbContext _context;

    public SpecializationService()
    {
        _context = new HospitalDbContext();
    }

    public List<Specialization> GetAll()
    {
        return _context.Specializations.ToList();
    }
}

using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services;

public class SpecializationsService
{
    private readonly HospitalDbContext _context;
    public SpecializationsService()
    {
        _context = new HospitalDbContext();
    }

    public List<Specialization> GetAll(string searchText = "")
    {
        var query = _context.Specializations
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(
                x => x.Name.Contains(searchText) ||
                x.Description.Contains(searchText));
        }

        return query.ToList();
    }

    public bool CreateSpecialization(Specialization specialization)
    {
        _context.Specializations.Add(specialization);

        int affetedRows = _context.SaveChanges();
        return affetedRows > 0;
    }

    public bool UpdateSpecialization(Specialization specialization)
    {
        var specializationToUpdate = _context.Specializations.FirstOrDefault(x => x.Id == specialization.Id);
        if (specializationToUpdate is null)
        {
            return false;
        }
        _context.Entry(specializationToUpdate).CurrentValues.SetValues(specialization);

        int affectedRows = _context.SaveChanges();

        return affectedRows > 0;
    }

    public bool DeleteSpecialization(int id)
    {
        var specializationToDelete = _context.Specializations.FirstOrDefault(x => x.Id == id);
        if (specializationToDelete is null)
        {
            return false;
        }

        _context.Remove(specializationToDelete);

        int affectedRows = _context.SaveChanges();
        return affectedRows > 0;
    }
}

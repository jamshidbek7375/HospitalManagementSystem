using Bogus;
using Bogus.DataSets;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services;

public class DataSeederService
{
    private static readonly Faker faker = new();

    public static void SeedDatabase()
    {
        using var context = new HospitalDbContext();

        CreatePatients(context);
        CreateDoctors(context);
        CreateSpecializations(context);
        CreateDoctorSpecializations(context);
        CreateAppointments(context);
        CreateVisits(context);
    }

    private static void CreatePatients(HospitalDbContext context)
    {
        if (context.Patients.Any()) return;

        for (int i = 0; i < 10000; i++)
        {
            var patient = new Patient();
            var (randomGender, fakerGender) = GetGender();
            patient.FirstName = faker.Name.FirstName(fakerGender);
            patient.LastName = faker.Name.LastName(fakerGender);
            patient.PhoneNumber = faker.Phone.PhoneNumber("+998##-###-##-##");
            patient.Birthdate = GetRandomBirthdate();
            patient.Gender = randomGender;

            context.Patients.Add(patient);
        }

        context.SaveChanges();
    }

    private static void CreateDoctors(HospitalDbContext context)
    {
        if (context.Doctors.Any()) return;

        for (int i = 0; i < 20; i++)
        {
            var doctor = new Doctor();
            var (gender, fakerGender) = GetGender();
            doctor.FirstName = faker.Name.FirstName(fakerGender);
            doctor.LastName = faker.Name.LastName(fakerGender);
            doctor.PhoneNumber = faker.Phone.PhoneNumber("+998##-###-##-##");

            context.Doctors.Add(doctor);
        }

        context.SaveChanges();
    }

    private static void CreateSpecializations(HospitalDbContext context)
    {
        if (context.Specializations.Any()) return;

        var values = Enum.GetNames(typeof(DoctorSpecializationType));

        foreach (var value in values)
        {
            var specialization = new Specialization
            {
                Name = value,
                Description = faker.Lorem.Text()
            };

            context.Specializations.Add(specialization);
        }

        context.SaveChanges();
    }

    private static void CreateDoctorSpecializations(HospitalDbContext context)
    {
        if (context.DoctorSpecializations.Any()) return;

        var doctorIds = context.Doctors.Select(x => x.Id).ToArray();
        var specializatoinIds = context.Specializations.Select(x => x.Id).ToArray();

        foreach (var doctorId in doctorIds)
        {
            var specializationsCount = faker.Random.Number(1, 3);
            HashSet<int> specializations = [];

            for (int i = 0; i < specializationsCount; i++)
            {
                var randomSpecializationId = faker.PickRandom(specializatoinIds);
                specializations.Add(randomSpecializationId);
            }

            foreach (var specializationId in specializations)
            {
                var doctorSpecialization = new DoctorSpecialization
                {
                    DoctorId = doctorId,
                    SpecializationId = specializationId
                };
                context.DoctorSpecializations.Add(doctorSpecialization);
            }
        }

        context.SaveChanges();
    }

    private static void CreateAppointments(HospitalDbContext context)
    {
        if (context.Appointments.Any()) return;

        var patientIds = context.Patients.Select(x => x.Id).ToArray();
        var doctorIds = context.Doctors.Select(x => x.Id).ToArray();
        var minDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2));
        var maxDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(2));
        var minTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8));
        var maxTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(20));

        foreach (var patientId in patientIds)
        {
            var appointmentsCount = faker.Random.Int(1, 5);
            for (int i = 0; i < appointmentsCount; i++)
            {
                var randomDoctorId = faker.PickRandom(doctorIds);
                var appointment = new Appointment
                {
                    Comments = faker.Lorem.Sentence(),
                    Date = faker.Date.BetweenDateOnly(minDate, maxDate),
                    Time = faker.Date.BetweenTimeOnly(minTime, maxTime),
                    PatientId = patientId,
                    DoctorId = randomDoctorId
                };
                appointment.Status = GetRandomStatus(appointment);
                context.Appointments.Add(appointment);
            }
        }

        context.SaveChanges();
    }

    private static void CreateVisits(HospitalDbContext context)
    {
        if (context.Visits.Any()) return;

        var appointmentIds = context.Appointments
            .Where(x => x.Status == AppointmentStatus.Closed)
            .Select(x => x.Id)
            .ToArray();

        foreach (var appoinmentId in appointmentIds)
        {
            var visit = new Visit
            {
                Comments = faker.Lorem.Sentence(),
                TotalDue = faker.Random.Decimal(100_000, 1_500_000),
                AppointmentId = appoinmentId
            };
            context.Visits.Add(visit);
        }

        context.SaveChanges();
    }

    private static DateOnly GetRandomBirthdate()
    {
        var minDate = new DateOnly(1940, 1, 1);
        var maxDate = new DateOnly(2023, 1, 1);

        return faker.Date.BetweenDateOnly(minDate, maxDate);
    }

    private static (Gender, Name.Gender) GetGender()
    {
        var randomGender = faker.Random.Enum<Gender>();
        var fakerGender = randomGender == Gender.Male ? Name.Gender.Male : Name.Gender.Female;

        return (randomGender, fakerGender);
    }

    private static AppointmentStatus GetRandomStatus(Appointment appointment)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var now = TimeOnly.FromDateTime(DateTime.Now);

        if (appointment.Date > today && appointment.Time > now)
        {
            return AppointmentStatus.Pending;
        }

        var random = faker.Random.Number(1, 10);

        return random % 2 == 0 ? AppointmentStatus.Cancelled : AppointmentStatus.Closed;
    }
}

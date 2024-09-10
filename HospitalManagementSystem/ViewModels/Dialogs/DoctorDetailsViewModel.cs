using HospitalManagementSystem.Models;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace HospitalManagementSystem.ViewModels.Dialogs;

public class DoctorDetailsViewModel : BaseViewModel
{
    private string _appointmentsTitle = string.Empty;
    public string AppointmentsTitle
    {
        get => _appointmentsTitle;
        set => SetProperty(ref _appointmentsTitle, value);
    }
    private string _historyTitle = string.Empty;
    public string HistoryTitle
    {
        get => _historyTitle;
        set => SetProperty(ref _historyTitle, value);
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public ObservableCollection<Appointment> Appointments { get; }
    public ObservableCollection<Visit> Visits { get; }

    public DoctorDetailsViewModel(Doctor Doctor)
    {
        ArgumentNullException.ThrowIfNull(Doctor);

        FirstName = Doctor.FirstName;
        LastName = Doctor.LastName;
        PhoneNumber = Doctor.PhoneNumber;

        Appointments = new ObservableCollection<Appointment>(Doctor.Appointments);

        var visits = Doctor.Appointments
        .Select(x => x.Visit)
            .ToList();
        Visits = new ObservableCollection<Visit>(visits);

        AppointmentsTitle = Appointments.Count > 0
            ? "Recent Appointments"
            : $"{FirstName} {LastName} has no recenet appointments";
        HistoryTitle = Visits.Count > 0
            ? "Patient Visits"
            : $"{FirstName} {LastName} has no visits yet.";
    }
}

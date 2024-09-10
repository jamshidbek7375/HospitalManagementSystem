using HospitalManagementSystem.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HospitalManagementSystem.ViewModels.Dialogs;

public class PatientDetailsViewModel : BaseViewModel
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
    public DateOnly Birthdate { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }

    public ObservableCollection<Appointment> Appointments { get; }
    public ObservableCollection<Visit> Visits { get; }

    public ICommand SaveCommand { get; }

    public PatientDetailsViewModel(Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient);

        FirstName = patient.FirstName;
        LastName = patient.LastName;
        Birthdate= patient.Birthdate;
        PhoneNumber = patient.PhoneNumber;
        Gender = patient.Gender;

        Appointments = new ObservableCollection<Appointment>(patient.Appointments);
        var visits = patient.Appointments
            .Where(x => x.Visit != null)
            .Select(x => x.Visit)
            .ToList();
        Visits = new ObservableCollection<Visit>(visits);

        AppointmentsTitle = Appointments.Count > 0 
            ? "Recent Appointments" 
            : $"{FirstName} {LastName} has no recenet appointments";
        HistoryTitle = Visits.Count > 0
            ? "Patient Visits"
            : $"{FirstName} {LastName} has no visits yet.";

        SaveCommand = new Command(OnSave);
    }

    private void OnSave()
    {
        int g = 0;
    }
}

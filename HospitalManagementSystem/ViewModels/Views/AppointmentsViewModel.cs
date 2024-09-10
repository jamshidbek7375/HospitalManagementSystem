using Bogus;
using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem.ViewModels.Views;

public class AppointmentsViewModel : BaseViewModel
{
    private readonly Faker faker;
    private readonly AppointmentsService _appointmentsService;
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            SearchAppointment(value);
        }

    }
    public ICommand DeleteCommand { get; }

    private List<Appointment> appointmentsList;
    public ObservableCollection<Appointment> Appointments { get; }
    public AppointmentsViewModel()
    {
        appointmentsList = [];
        Appointments = [];

        faker = new();
        _appointmentsService = new();
        DeleteCommand = new Command<Appointment>(OnDelete);

        Load();
    }

    private void Load()
    {
        var appointments = _appointmentsService.GetAppointments();

        foreach (var appointment in appointments)
        {
            if (appointment.Date < DateOnly.FromDateTime(DateTime.Now) && appointment.Status.ToString() == "Pending")
            {
                int num = faker.PickRandom(1, 2);
                appointment.Status = num == 1 ? AppointmentStatus.Cancelled : AppointmentStatus.Closed;
            }
            Appointments.Add(appointment);
            appointmentsList.Add(appointment);
        }
    }
    private void OnDelete(Appointment appointment)
    {
        var result = MessageBoxExtension.ShowConfirmation("Are you sure you want to delete");

        if (result == MessageBoxResult.No)
        {
            return;
        }

        _appointmentsService.DeleteAppointment(appointment.Id);
        MessageBoxExtension.ShowSuccess($"Appointment was successfully deleted.");
    }

    private void SearchAppointment(string value)
    {
        var appointments = _appointmentsService.GetAppointments(_searchText);

        Appointments.Clear();
        foreach (var appointment in appointments)
        {
            Appointments.Add(appointment);
        }
    }
}

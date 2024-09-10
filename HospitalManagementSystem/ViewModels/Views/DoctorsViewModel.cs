using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Views.Dialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem.ViewModels.Views;

public class DoctorsViewModel : BaseViewModel
{
    private readonly DoctorsService _doctorsService;
    private readonly SpecializationsService _specializationsService;

    private Doctor _selectedDoctor;
    public Doctor SelectedDoctor
    {
        get => _selectedDoctor;
        set => SetProperty(ref _selectedDoctor, value);
    }

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            FilterDoctors();
        }

    }


    private Specialization _selectedSpecialization;
    public Specialization SelectedSpecialization
    {
        get => _selectedSpecialization;
        set
        {
            SetProperty(ref _selectedSpecialization, value);
            FilterDoctors();
        }
    }

    public ICommand ShowDetailsCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand AddCommand { get; }

    private List<Doctor> doctorsList;
    public ObservableCollection<Doctor> Doctors { get; }
    public ObservableCollection<Specialization> Specializations { get; }

    public DoctorsViewModel()
    {
        _doctorsService = new();
        _specializationsService = new();
        Doctors = [];
        Specializations = [];
        doctorsList = [];

        ShowDetailsCommand = new Command(OnShowDetails);
        EditCommand = new Command<Doctor>(OnEdit);
        DeleteCommand = new Command<Doctor>(OnDelete);
        AddCommand = new Command(OnAdd);

        Load();
    }

    private void Load()
    {
        var patients = _doctorsService.GetDoctors();
        var specializations = _specializationsService.GetAll();

        foreach (var patient in patients)
        {
            Doctors.Add(patient);
            doctorsList.Add(patient);
        }

        foreach (var specialization in specializations)
        {
            Specializations.Add(specialization);
        }
    }

    private void FilterDoctors()
    {
        var doctors = _doctorsService.GetDoctors(_searchText, _selectedSpecialization?.Id);

        Doctors.Clear();

        foreach (var doctor in doctors)
        {
            Doctors.Add(doctor);
        }
    }

    private void OnShowDetails()
    {
        if (SelectedDoctor is null)
        {
            return;
        }

        var dialog = new DoctorDetailsDialog(SelectedDoctor);
        dialog.ShowDialog();
    }

    private void OnEdit(Doctor doctor)
    {
        var dialog = new DoctorsDialog(doctor);
        dialog.ShowDialog();
    }

    private void OnDelete(Doctor doctor)
    {
        var result = MessageBoxExtension.ShowConfirmation($"Are you sure to delete {doctor.FirstName} {doctor.LastName}?");

        if (result == MessageBoxResult.No)
        {
            return;
        }

        _doctorsService.DeleteDoctor(doctor);
        MessageBoxExtension.ShowSuccess($"Doctor: {doctor.FirstName} {doctor.LastName} successfully deleted.");
    }

    private void OnAdd()
    {
        var dialog = new DoctorsDialog();
        dialog.ShowDialog();
    }

}
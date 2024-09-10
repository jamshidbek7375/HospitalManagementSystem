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

public partial class PatientsViewModel : BaseViewModel
{
    private readonly PatientsService _patientsService;

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            SearchPatients(value);
        }
    }
    private bool _isFirstEnabled;
    public bool IsFirstEnabled
    {
        get => _isFirstEnabled;
        set
        {
            _isFirstEnabled = value;
            OnPropertyChanged(nameof(IsFirstEnabled));
        }
    }

    private bool _isLastEnabled;
    public bool IsLastEnabled
    {
        get => _isLastEnabled;
        set
        {
            _isLastEnabled = value;
            OnPropertyChanged(nameof(IsLastEnabled));
        }
    }

    private bool _isNextEnabled;
    public bool IsNextEnabled
    {
        get => _isNextEnabled;
        set
        {
            _isNextEnabled = value;
            OnPropertyChanged(nameof(IsNextEnabled));
        }
    }

    private bool _isPreviousEnabled;
    public bool IsPreviousEnabled
    {
        get => _isPreviousEnabled;
        set
        {
            _isPreviousEnabled = value;
            OnPropertyChanged(nameof(IsPreviousEnabled));
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
            UpdateEnableState();
        }
    }
    private void UpdateEnableState()
    {
        IsFirstEnabled = CurrentPage > 1;
        IsPreviousEnabled = CurrentPage > 1;
        IsNextEnabled = CurrentPage < NumberOfPages;
        IsLastEnabled = CurrentPage < NumberOfPages;
    }
    private int _selectedRecord = 15;
    public int SelectedRecord
    {
        get => _selectedRecord;
        set
        {
            _selectedRecord = value;
            OnPropertyChanged(nameof(CurrentPage));
            UpdateRecordCount();
        }
    }
    private void UpdateRecordCount(string? search = null)
    {
        NumberOfPages = _patientsService.GetPatients().Count() / Convert.ToInt32(SelectedRecord);
        NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;

        if (search != null)
        {
            NumberOfPages = _patientsService.GetPatientsCount(search) / Convert.ToInt32(SelectedRecord);
        }
        UpdateCollection(ListPatientsRecords.Take(SelectedRecord));
        CurrentPage = 1;
    }
    private int _numberOfPages;
    public int NumberOfPages
    {
        get => _numberOfPages;
        set
        {
            _numberOfPages = value;
            OnPropertyChanged(nameof(NumberOfPages));
            UpdateEnableState();
        }
    }
    private Patient _selectedPatient;
    public Patient SelectedPatient
    {
        get => _selectedPatient;
        set => SetProperty(ref _selectedPatient, value);
    }
    public ICommand AddCommand { get; }
    public ICommand ShowDetailsCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand FirstCommand { get; }
    public ICommand PreviousCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand LastCommand { get; }
    public ObservableCollection<Patient> Patients { get; }
    public List<Patient> ListPatientsRecords = [];
    public PatientsViewModel()
    {
        _patientsService = new();
        Patients = [];

        AddCommand = new Command(OnAdd);
        ShowDetailsCommand = new Command(OnShowDetails);
        EditCommand = new Command<Patient>(OnEdit);
        DeleteCommand = new Command<Patient>(OnDelete);

        NextCommand = new Command(NextPage);
        FirstCommand = new Command(FirstPage);
        PreviousCommand = new Command(PreviousPage);
        LastCommand = new Command(LastPage);

        Load();
    }
    private int recordStartFrom = 0;
    private void LastPage(object obj)
    {
        CurrentPage = NumberOfPages;
        int count = _patientsService.GetPatientsCount(SearchText);

        if (SearchText is not null)
        {
            UpdateCollection(ListPatientsRecords.Skip(recordStartFrom).Take(SelectedRecord));
            UpdateEnableState();
            return;
        }

        UpdateCollection(_patientsService.GetPatients(pageNumber: count - SelectedRecord, pageSize: SelectedRecord));
        UpdateEnableState();
    }
    private void PreviousPage(object obj)
    {
        CurrentPage--;
        recordStartFrom = (CurrentPage - 1) * SelectedRecord;

        if (SearchText is not null)
        {
            UpdateCollection(ListPatientsRecords.Skip(recordStartFrom).Take(SelectedRecord));
            UpdateEnableState();
            return;
        }
        var recordsToShow = _patientsService.GetPatients(pageNumber: recordStartFrom, pageSize: SelectedRecord);
        UpdateCollection(recordsToShow);
        UpdateEnableState();
    }
    private void FirstPage(object obj)
    {
        CurrentPage = 1;

        if (SearchText is not null)
        {
            UpdateCollection(ListPatientsRecords.Skip(0).Take(SelectedRecord));
            UpdateEnableState();
            return;
        }
        UpdateCollection(_patientsService.GetPatients(pageNumber: 1 , pageSize: SelectedRecord));

        UpdateEnableState();
    }
    private void NextPage(object arg)
    {
        CurrentPage++;
        recordStartFrom = CurrentPage * SelectedRecord;

        if (SearchText is not null)
        {
            UpdateCollection(ListPatientsRecords.Skip(recordStartFrom).Take(SelectedRecord));
            UpdateEnableState();
            return;
        }

        var recordsToShow = _patientsService.GetPatients(pageNumber: recordStartFrom, pageSize: SelectedRecord);
        UpdateCollection(recordsToShow);
        UpdateEnableState();
    }
    private void UpdateCollection(IEnumerable<Patient> enumerable)
    {
        Patients.Clear();
        foreach (var patient in enumerable)
        {
            Patients.Add(patient);
        }
        NumberOfPages = _patientsService.GetPatientsCount(SearchText) / SelectedRecord;
    }
    private void Load()
    {
        var patients = _patientsService.GetPatients(SearchText, pageNumber: recordStartFrom, pageSize: SelectedRecord);

        foreach (var patient in patients)
        {
            ListPatientsRecords.Add(patient);
        }
        UpdateCollection(ListPatientsRecords);

        UpdateRecordCount();
    }
    private void SearchPatients(string searchText)
    {
        ListPatientsRecords = _patientsService.GetPatients(searchText);
        UpdateCollection(ListPatientsRecords);

        UpdateRecordCount(searchText);
    }
    private void OnAdd()
    {
        var dialog = new PatientDialog();
        dialog.ShowDialog();
    }
    private void OnShowDetails()
    {
        if (SelectedPatient is null)
        {
            return;
        }

        var dialog = new PatientDetailsDialog(SelectedPatient);
        dialog.ShowDialog();
    }
    private void OnEdit(Patient patient)
    {
        var dialog = new PatientDialog(patient);
        dialog.ShowDialog();
    }
    private void OnDelete(Patient patient)
    {
        var result = MessageBoxExtension.ShowConfirmation($"Are you sure you want to delete: {patient.FirstName} {patient.LastName}?");

        if (result == MessageBoxResult.No)
        {
            return;
        }

        _patientsService.Delete(patient);
        MessageBoxExtension.ShowSuccess($"Patient: {patient.FirstName} {patient.LastName} was successfully deleted.");
    }
}

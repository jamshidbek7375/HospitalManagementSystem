using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using System.Windows;

namespace HospitalManagementSystem.Views.Dialogs;

/// <summary>
/// Interaction logic for PatientDialog.xaml
/// </summary>
public partial class PatientDialog : Window
{
    private readonly PatientsService _patientsService;
    private readonly bool isEditingMode;
    private static int lastSequenceId;
    public PatientDialog()
    {
        InitializeComponent();

        _patientsService = new PatientsService();
        GenderCombobox.ItemsSource = _patientsService.GetGenders();

        IdInput.Text = lastSequenceId.ToString();
        IdInput.IsEnabled = false;

        Title = "Add Patient";
    }

    public PatientDialog(Patient patient)
        : this()
    {
        Title = "Edit Patient details";

        isEditingMode = true;

        IdInput.IsEnabled = false;

        PopulateData(patient);
    }

    private void PopulateData(Patient patient)
    {
        IdInput.Text = patient.Id.ToString();
        FirstNameInput.Text = patient.FirstName.ToString();
        LastNameInput.Text = patient.LastName.ToString();
        PhoneNumberInput.Text = patient.PhoneNumber.ToString();
        DateTimeInput.Text = patient.Birthdate.ToString();
        GenderCombobox.Text = patient.Gender.ToString();
    }

    private void Save_Clicked(object sender, RoutedEventArgs e)
    {
        int id = 0;
        string firstName = FirstNameInput.Text;
        string lastName = LastNameInput.Text;
        string phoneNumber = PhoneNumberInput.Text;
        DateOnly birthdate = DateOnly.FromDateTime(Convert.ToDateTime(DateTimeInput.Text));
        Gender gender = GenderCombobox.Text == "Female" ? Gender.Female : Gender.Male;

        var newPatient = new Patient(id, firstName, lastName, phoneNumber, birthdate, gender);

        bool isSuccess;

        if (isEditingMode)
        {
            isSuccess = _patientsService.Update(newPatient);
        }
        else
        {
            isSuccess = _patientsService.Create(newPatient);
        }

        if (isSuccess && isEditingMode)
        {
            MessageBoxExtension.ShowSuccess($"{newPatient.FirstName} {newPatient.LastName} was successfully updated.");
            Close();
        }
        else
        {
            lastSequenceId = _patientsService.GetPatients()[^1].Id + 1;
            MessageBoxExtension.ShowSuccess($"{newPatient.FirstName} {newPatient.LastName} was successfully added.");
            Close();
        }
    }

    private void Cancel_Clicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.ViewModels.Dialogs;
using System.Drawing.Drawing2D;
using System.Windows;

namespace HospitalManagementSystem.Views.Dialogs;

/// <summary>
/// Interaction logic for DoctorDialog.xaml
/// </summary>
public partial class DoctorsDialog : Window
{
    private readonly DoctorsService _doctorsService;
    private readonly bool IsEditingMode;
    public DoctorsDialog()
    {
        InitializeComponent();

        Title = "Add Doctor";

        _doctorsService = new DoctorsService();
    }

    public DoctorsDialog(Doctor doctor)
        : this()
    {
        Title = "Edit Doctor";

        IsEditingMode = true;

        PopulateData(doctor);
    }

    private void PopulateData(Doctor doctor)
    {
        IdInput.Text = doctor.Id.ToString();
        FirstNameInput.Text = doctor.FirstName.ToString();
        LastNameInput.Text = doctor.LastName.ToString();
        PhoneNumberInput.Text = doctor.PhoneNumber.ToString();
    }

    private void Save_Clicked(object sender, RoutedEventArgs e)
    {
        int id = 0;
        string firstName = FirstNameInput.Text;
        string lastName = LastNameInput.Text;
        string phoneNumber = PhoneNumberInput.Text;

        var newDoctor = new Doctor();

        bool isSuccess;

        if (IsEditingMode)
        {
            isSuccess = _doctorsService.UpdateDoctor(newDoctor);
            if (isSuccess)
            {
                MessageBoxExtension.ShowSuccess($"{firstName} {lastName} was updated successfully.");
                Close();
                return;
            }

            MessageBoxExtension.ShowError("Something went wrong to update.");
        }
        else
        {
            isSuccess = _doctorsService.Create(newDoctor);
            if (isSuccess)
            {
                MessageBoxExtension.ShowSuccess($"{firstName} {lastName} was created successfully.");
                Close();
                return;
            }

            MessageBoxExtension.ShowError("Something went wrong to create");
        }

        Close();
    }

    private void Cancel_Clicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

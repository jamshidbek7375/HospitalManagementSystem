using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using System.Windows;

namespace HospitalManagementSystem.Views.Dialogs;

/// <summary>
/// Interaction logic for SpecializationDialog.xaml
/// </summary>
public partial class SpecializationDialog : Window
{
    private readonly SpecializationsService _specializationsService;
    private readonly bool IsEditingMode;
    public SpecializationDialog()
    {
        InitializeComponent();

        _specializationsService = new SpecializationsService();
        Title = "Add Specialization";
        IdInput.Text = 0.ToString();
        IdInput.IsEnabled = false;
    }
    public SpecializationDialog(Specialization specialization)
       : this()
    {
        IsEditingMode = true;
        Title = "Update Specialization";
        IdInput.IsEnabled = false;
        PopulateDate(specialization);
    }

    private void PopulateDate(Specialization specialization)
    {
        if (specialization is null)
        {
            return;
        }
        IdInput.Text = specialization.Id.ToString();
        NameInput.Text = specialization.Name;
        DescriptionInput.Text = specialization.Description;
    }

    private void Cancel_Clicked(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Save_Clicked(object sender, RoutedEventArgs e)
    {
        int id = int.Parse(IdInput.Text);
        string name = NameInput.Text;
        string description = DescriptionInput.Text;

        var newSpecialization = new Specialization(id, name, description);

        bool isSuccess;

        if (IsEditingMode)
        {
            isSuccess = _specializationsService.UpdateSpecialization(newSpecialization);
        }
        else
        {
            isSuccess = _specializationsService.CreateSpecialization(newSpecialization);
        }

        if (isSuccess == true && IsEditingMode == true)
        {
            MessageBoxExtension.ShowSuccess($"{newSpecialization.Name} has successfully updated.");
            Close();
        }
        else if (isSuccess)
        {
            MessageBoxExtension.ShowSuccess($"{newSpecialization} has successfully added.");
            Close();
        }
        else
        {
            MessageBoxExtension.ShowError("Something is wrong!");
        }
    }
}

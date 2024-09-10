using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModels.Dialogs;
using System.Windows;

namespace HospitalManagementSystem.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for PatientDialog.xaml
    /// </summary>
    public partial class PatientDialog : Window
    {
        public PatientDialog()
        {
            InitializeComponent();

            // Binding -> 

            DataContext = new PatientDialogViewModel();
        }
    }
}

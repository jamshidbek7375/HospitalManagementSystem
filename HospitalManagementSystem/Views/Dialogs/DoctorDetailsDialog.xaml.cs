using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HospitalManagementSystem.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DoctorDetailsDialog.xaml
    /// </summary>
    public partial class DoctorDetailsDialog : Window
    {
        public DoctorDetailsDialog(Doctor doctor)
        {
            InitializeComponent();

            DataContext = new DoctorDetailsViewModel(doctor);
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

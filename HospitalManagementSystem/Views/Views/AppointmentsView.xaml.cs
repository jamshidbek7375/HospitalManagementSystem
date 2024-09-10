using HospitalManagementSystem.ViewModels.Views;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views.Views;

/// <summary>
/// Interaction logic for AppointmentsView.xaml
/// </summary>
public partial class AppointmentsView : UserControl
{
    public AppointmentsView()
    {
        InitializeComponent();

        DataContext = new AppointmentsViewModel();
    }
}

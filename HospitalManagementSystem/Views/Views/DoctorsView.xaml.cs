using HospitalManagementSystem.ViewModels.Views;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views.Views;

/// <summary>
/// Interaction logic for DoctorsView.xaml
/// </summary>
public partial class DoctorsView : UserControl
{
    public DoctorsView()
    {
        InitializeComponent();

        DataContext = new DoctorsViewModel();
    }
}

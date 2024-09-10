using HospitalManagementSystem.ViewModels.Views;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views.Views;

/// <summary>
/// Interaction logic for SpecializationsView.xaml
/// </summary>
public partial class SpecializationsView : UserControl
{
    public SpecializationsView()
    {
        InitializeComponent();

        DataContext = new SpecializationsViewModel();
    }
}

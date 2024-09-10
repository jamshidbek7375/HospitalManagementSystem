using HospitalManagementSystem.Services;
using System.Windows;

namespace HospitalManagementSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataSeederService.SeedDatabase();
    }
}
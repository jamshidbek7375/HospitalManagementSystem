using HospitalManagementSystem.ViewModels;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views
{
    public partial class PatientsView : UserControl
    {
        public PatientsView()
        {
            InitializeComponent();

            var vm = new PatientsViewModel();
            DataContext = vm;

            // this.Pager = vm.TotalPatientsCount;
        }
    }
}

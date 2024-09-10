using HospitalManagementSystem.ViewModels;
using System.Windows.Controls;

namespace HospitalManagementSystem.Views
{
	public partial class DoctorsView : UserControl
	{
		public DoctorsView()
		{
			InitializeComponent();

			DataContext = new DoctorsViewModel();
		}
	}
}

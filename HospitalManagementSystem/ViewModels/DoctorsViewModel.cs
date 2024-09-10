using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Views.Dialogs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HospitalManagementSystem.ViewModels
{
	public class DoctorsViewModel : BaseViewModel
	{
		private readonly DoctorsService _doctorsService;
		private readonly SpecializationService _specializationService;

		private string _searchText;
		public string SearchText
		{
			get => _searchText;
			set
			{
				SetProperty(ref _searchText, value);
				FilterDoctors();
			}
		}

		private Specialization _selectedSpecialization;
		public Specialization SelectedSpecialization
		{
			get => _selectedSpecialization;
			set
			{
                SetProperty(ref _selectedSpecialization, value);
				FilterDoctors();
            }
		}

		public ICommand AddCommand { get; }


		private readonly List<Doctor> doctorsList;
		public ObservableCollection<Doctor> Doctors { get; }
		public ObservableCollection<Specialization> Specializations { get; }

		public DoctorsViewModel()
		{
			_doctorsService = new();
			_specializationService = new();

            doctorsList = [];
			Doctors = [];
			Specializations = [];

			AddCommand = new Command(OnAdd);

			Load();
		}

		private void Load()
		{
			var doctors = _doctorsService.GetDoctors();
			var specializations = _specializationService.GetAll();

			foreach (var doctor in doctors)
			{
				Doctors.Add(doctor);
				doctorsList.Add(doctor);
			}

			foreach(var specialization in specializations)
			{
				Specializations.Add(specialization);
			}
		}

		private void FilterDoctors()
		{
			var doctors = _doctorsService.GetDoctors(_searchText, _selectedSpecialization?.Id);

			Doctors.Clear();

			foreach (var doctor in doctors)
			{
				Doctors.Add(doctor);
			}
		}

		private void OnAdd()
		{
			var dialog = new DoctorDialog();
			dialog.ShowDialog();
		}
	}
}

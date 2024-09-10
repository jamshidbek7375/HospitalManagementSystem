using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Views.Dialogs;
using MvvmHelpers.Commands;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace HospitalManagementSystem.ViewModels.Views
{
    public class SpecializationsViewModel : BaseViewModel
    {
        private readonly SpecializationsService _specializationsService;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchSpecializations(value);
            }
        }

        private Patient _selectedSpecialization;
        public Patient SelectedSpecialization
        {
            get => _selectedSpecialization;
            set => SetProperty(ref _selectedSpecialization, value);
        }

        public ICommand AddCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private List<Specialization> specializationsList;
        public ObservableCollection<Specialization> Specializations { get; }

        public SpecializationsViewModel()
        {
            _specializationsService = new();
            Specializations = [];
            specializationsList = [];

            AddCommand = new Command(OnAdd);
            ShowDetailsCommand = new Command(OnShowDetails);
            EditCommand = new Command<Specialization>(OnEdit);
            DeleteCommand = new Command<Specialization>(OnDelete);

            Load();
        }

        private void Load()
        {
            var specializations = _specializationsService.GetAll();

            foreach (var specialization in specializations)
            {
                Specializations.Add(specialization);
                specializationsList.Add(specialization);
            }
        }

        private void SearchSpecializations(string searchText)
        {
            var specializations = _specializationsService.GetAll(searchText);

            Specializations.Clear();
            foreach (var specialization in specializations)
            {
                Specializations.Add(specialization);
            }
        }

        private void OnAdd()
        {
            var dialog = new SpecializationDialog();
            dialog.ShowDialog();
        }

        private void OnShowDetails()
        {
            if (SelectedSpecialization is null)
            {
                return;
            }

            var dialog = new PatientDetailsDialog(SelectedSpecialization);
            dialog.ShowDialog();
        }

        private void OnEdit(Specialization specialization)
        {
            var dialog = new SpecializationDialog(specialization);
            dialog.ShowDialog();
        }

        private void OnDelete(Specialization specialization)
        {
            var result = MessageBoxExtension.ShowConfirmation($"Are you sure you want to delete ?");

            if (result == MessageBoxResult.No)
            {
                return;
            }

            _specializationsService.DeleteSpecialization(specialization.Id);
            MessageBoxExtension.ShowSuccess($"Specialization  was successfully deleted.");
        }
    }

}

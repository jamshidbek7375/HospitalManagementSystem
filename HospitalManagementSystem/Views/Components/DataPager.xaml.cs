using Microsoft.EntityFrameworkCore.Query;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospitalManagementSystem.Views.Components
{
    /// <summary>
    /// Interaction logic for DataPager.xaml
    /// </summary>
    public partial class DataPager : UserControl
    {
        public DataPager()
        {
            InitializeComponent();

            var vm = this.DataContext;

            int g = 0;
        }

        public int ItemsCount { get; set; }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            private set => _currentPage = value;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using CapstoneProject.DAL;
using CapstoneProject.Models;
namespace CapstoneProject.Pages {
    /// <summary>
    /// Interaction logic for ProjectProperties.xaml
    /// </summary>
    
    //By Levi Delezene
    public partial class ProjectProperties : Page {
        Project project;
        Mode _mode;

        public enum Mode { INSERT = 0, UPDATE = 1 }

        public ProjectProperties(Mode mode) {
            _mode = mode;
            InitializeComponent();
            dpStartDate.BlackoutDates.AddDatesInPast();
        }

        public void numberValidation(object sender, TextCompositionEventArgs e) {
            MainWindow.numberValidation(sender, e);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e) {
            try {
                project = createProject();
                NavigationService.Navigate(new Chart(project));
            } catch (Exception excep) {
                MessageBox.Show(excep.ToString());
            }
        }

        private Project createProject() {
            Project project = new Project() {
                Name = tbxName.Text,
                Description = tbxDescription.Text,
                WorkingHours = int.Parse(tbxWorkingHours.Text),
                StartDate = (DateTime)dpStartDate.SelectedDate,
                ProjectOwner = (User)cboOwner.Items[cboOwner.SelectedIndex]
        };

            OProject projectDAL = new OProject();
            if (_mode == Mode.INSERT)
                project = projectDAL.Insert(project);

            if (_mode == Mode.UPDATE)
                projectDAL.Update(project);

            return project;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            OUser userDAL = new OUser();
            List<User> userList = userDAL.Select();
            foreach (User user in userList)
            {
                User userValue = new User(user.Id, user.FirstName,user.LastName);
                cboOwner.Items.Add(userValue);
            }
        }
    }
}

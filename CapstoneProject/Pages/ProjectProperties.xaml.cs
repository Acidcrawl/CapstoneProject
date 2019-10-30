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
        OProject projectDAL = new OProject();
        WindowState prevWindowState = new WindowState();

        public enum Mode { INSERT = 0, UPDATE = 1 }

        public ProjectProperties(Mode mode, Project project) {
            _mode = mode;
            this.project = project;
            InitializeComponent();
        }

        public void numberValidation(object sender, TextCompositionEventArgs e) {
            MainWindow.numberValidation(sender, e);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e) {
            try {
                project = createProject();
                if (_mode == Mode.INSERT)
                    NavigationService.Navigate(new Chart(project));
                else
                {
                    Window.GetWindow(this).WindowState = WindowState.Maximized;
                    Window.GetWindow(this).KeyDown -= Page_KeyDown;
                    NavigationService.GoBack();
                }
            } catch (Exception excep) {
                MessageBox.Show(excep.ToString());
            }
        }

        private Project createProject() {

            project.Name = tbxName.Text;
            project.Description = tbxDescription.Text;
            project.WorkingHours = int.Parse(tbxWorkingHours.Text);
            project.StartDate = (DateTime)dpStartDate.SelectedDate;
            project.ProjectOwner = (User)cboOwner.Items[cboOwner.SelectedIndex];

            if (_mode == Mode.INSERT)
                project = projectDAL.Insert(project);

            if (_mode == Mode.UPDATE)
                projectDAL.Update(project);

            return project;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            prevWindowState = Window.GetWindow(this).WindowState;
            Window.GetWindow(this).WindowState = WindowState.Normal;


            OUser userDAL = new OUser();
            List<User> userList = userDAL.Select();
            foreach (User user in userList)
            {
                User userValue = new User(user.Id, user.FirstName,user.LastName);
                cboOwner.Items.Add(userValue);
            }

            if (_mode == Mode.UPDATE)
            {
                tbxName.Text = project.Name;
                tbxDescription.Text = project.Description;
                tbxWorkingHours.Text = project.WorkingHours.ToString();
                dpStartDate.SelectedDate = project.StartDate.Date;
                cboOwner.SelectedIndex = cboOwner.Items.IndexOf(project.ProjectOwner);
                //Disable startdate for being edited
                dpStartDate.IsEnabled = false;
            }
            else
            {
                dpStartDate.BlackoutDates.AddDatesInPast();
            }

            Window.GetWindow(this).KeyDown += Page_KeyDown;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                NavigationService.GoBack();
                Window.GetWindow(this).WindowState = prevWindowState;
                Window.GetWindow(this).KeyDown -= Page_KeyDown;
            }
        }
    }
}

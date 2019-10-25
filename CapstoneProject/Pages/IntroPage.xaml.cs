using CapstoneProject.DAL;
using CapstoneProject.Models;
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

namespace CapstoneProject.Pages
{
    /// <summary>
    /// Interaction logic for IntroPage.xaml
    /// </summary>
    public partial class IntroPage : Page
    {
        MainWindow parent;
        public IntroPage(MainWindow mainWindow)
        {
            parent = mainWindow;
            InitializeComponent();
        }

        private void CmdOpen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Chart((Project)cboProject.Items[cboProject.SelectedIndex]));
            parent.WindowState = WindowState.Maximized;
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectProperties(ProjectProperties.Mode.INSERT));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OProject projectDAL = new OProject();
            List<Project> projectList = projectDAL.Select();
            foreach (Project project in projectList)
            {
                cboProject.Items.Add(project);
            }
        }
    }
}

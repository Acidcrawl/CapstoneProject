using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CapstoneProject.Models;
using CapstoneProject.Windows;
using CapstoneProject.DAL;
using CapstoneProject.Pages;

namespace CapstoneProject.Controls {
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl {
        Chart _chart;

        public TaskControl(Task task, Chart chart) {
            InitializeComponent();
<<<<<<< HEAD
            this.DataContext = task;
=======
            DataContext = task;
            this._chart = chart;
>>>>>>> 019879c6ba75f35c252766c05c3f3043f6df1491
        }

        private void mi_editTask_Click(object sender, RoutedEventArgs e) {
            Task task = (Task)((MenuItem)sender).DataContext;
            new frmTask(_chart, task).ShowDialog();
            //_chart.DrawGraph(_chart.GetTasksAndDependanciesFromDatabase());
        }
    }
}

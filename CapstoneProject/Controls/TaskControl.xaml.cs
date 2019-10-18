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

namespace CapstoneProject.Controls {
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl {
        public TaskControl(Task task) {
            InitializeComponent();
            DataContext = task;
        }

        private void mi_editTask_Click(object sender, RoutedEventArgs e) {
            Console.Out.WriteLine(tbxTaskId.Text);
            //new frmCreateTask(null, OTask.get(tbxTaskId.Text));
        }
    }
}

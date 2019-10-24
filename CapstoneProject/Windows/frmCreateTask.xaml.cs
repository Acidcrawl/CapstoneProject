using CapstoneProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CapstoneProject.DAL;
using CapstoneProject.Pages;
using System.Globalization;
using System.Data.SqlClient;

namespace CapstoneProject {
    /// <summary>
    /// Interaction logic for frmCreateTask.xaml
    /// </summary>

    //By Levi Delezene
    public partial class frmCreateTask : Window {
        private Chart _chart;
        private Task taskToEdit;

        public frmCreateTask(Chart chart, Task task = null) {
            InitializeComponent();
            DataContext = this;
            Owner = Application.Current.MainWindow;
            _chart = chart;
            if (task != null) {
                taskToEdit = task;
                tbxTaskName.Text = taskToEdit.Name;
                tbxTaskDescription.Text = taskToEdit.Description;
                tbxMaxDuration.Text = taskToEdit.MaxDuration.ToString();
                tbxMinDuration.Text = taskToEdit.MinDuration.ToString();
                cmbPriority.SelectedValue = taskToEdit.Priority;
            }
        }

        public List<User> Users { get; } = new OUser().Select();
        public List<Status> Statuses { get; } = new List<Status> { Status.Completed, Status.In_Progress, Status.Not_Started};
        public List<int> Priorities { get; } = new List<int> { 1,2,3,4,5};

        private bool validateInput() {
            bool ret = true;

            if (tbxTaskName.Text == null || tbxTaskName.Text == "") {
                tbxTaskName.BorderBrush = Brushes.Red;
                ret = false;
            }
            if (tbxTaskDescription.Text == null || tbxTaskDescription.Text == "") {
                tbxTaskDescription.BorderBrush = Brushes.Red;
                ret = false;
            }
            if (tbxMinDuration.Text == null || tbxMinDuration.Text == "") {
                tbxMinDuration.BorderBrush = Brushes.Red;
                ret = false;
            }
            if (tbxMaxDuration.Text == null || tbxMaxDuration.Text == "") {
                tbxMaxDuration.BorderBrush = Brushes.Red;
                ret = false;
            }
            if (cmbPriority.Text == null || cmbPriority.Text == "") {
                cmbPriority.Background = Brushes.Red;
                ret = false;
            }
            if (cmbOwner.SelectedIndex < 0) {
                cmbOwner.BorderBrush = Brushes.Red;
                ret = false;
            }
            if (cmbStatus.Text == null || cmbStatus.Text == "") {
                cmbStatus.BorderBrush = Brushes.Red;
                ret = false;
            }

            return ret;
        }

        //By Levi Delezene
        private Task createTask() {
            if (!validateInput()) return null;

            Task task = new Task {
                Name = tbxTaskName.Text,
                ProjectId = _chart.Project.Id,
                Description = tbxTaskDescription.Text,
                MinDuration = float.Parse(tbxMinDuration.Text),
                MaxDuration = float.Parse(tbxMaxDuration.Text),
                Priority = int.Parse(cmbPriority.Text),
                StartedDate = DateTime.Parse("1/5/2019"),
                Owner = (User)cmbOwner.Items[cmbOwner.SelectedIndex]
            };


            //Maybe find a better way to do this
            switch (cmbStatus.Text) {
                case "Not Started":
                    task.Status = Status.Not_Started;
                    break;
                case "In Progress":
                    task.Status = Status.In_Progress;
                    break;
                case "Completed":
                    task.Status = Status.Completed;
                    break;
            }

            new OTask().Insert(task);

            return task;
        }

        private Task editTask() {
            if (!validateInput()) return null;

            taskToEdit.Name = tbxTaskName.Text;
            taskToEdit.Description = tbxTaskDescription.Text;
            taskToEdit.MinDuration = float.Parse(tbxMinDuration.Text);
            taskToEdit.MaxDuration = float.Parse(tbxMaxDuration.Text);
            taskToEdit.Priority = int.Parse(cmbPriority.Text);
            taskToEdit.Owner = (User)cmbOwner.Items[cmbOwner.SelectedIndex];

            switch (cmbStatus.Text) {
                case "Not Started":
                    taskToEdit.Status = Status.Not_Started;
                    break;
                case "In Progress":
                    taskToEdit.Status = Status.In_Progress;
                    break;
                case "Completed":
                    taskToEdit.Status = Status.Completed;
                    break;
            }

            new OTask().Update(taskToEdit);

            return taskToEdit;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e) {
            try {
                Task task;
                if (taskToEdit != null) {
                    task = editTask();
                } else {
                    task = createTask();
                }

                _chart.Newtask = task;

                if(task != null) {
                    Close();
                }

            } catch (Exception excep) {
                MessageBox.Show(excep.ToString());
            }
        }



        //By Levi Delezene
        private void btnSubmitAndClose_Click(object sender, RoutedEventArgs e) {
            try {
                Task task;
                if (taskToEdit != null) {
                    task = editTask();
                } else {
                    task = createTask();
                }

                _chart.Newtask = task;

            } catch (Exception excep) {
                MessageBox.Show(excep.ToString());
            } finally {
                Close();
            }
        }

        //By Levi Delezene
        private void btnSubmitAndContinue_Click(object sender, RoutedEventArgs e) {
            try {
                Task task;
                if (taskToEdit != null) {
                    task = editTask();
                } else {
                    task = createTask();
                }
            } catch (Exception excep) {
                MessageBox.Show(excep.ToString());
            } finally {
                //There's probably a better way to do this
                Close();
                new frmCreateTask(_chart).Show();
            }
        }

        //By Levi Delezene
        private void numberValidation(object sender, TextCompositionEventArgs e) {
            MainWindow.numberValidation(sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //tbxTaskName.Focus();
            //List<User> users = new OUser().Select();
            //foreach(User u in users) {
            //    cmbOwner.Items.Add(u);
            //}

            //OUser user = new OUser();
            //SqlDataReader sdr = user.Select();
            //while (sdr.Read()) {
            //    User userValue = new User(sdr.GetInt32(0), sdr.GetString(1), sdr.GetString(2));
            //    cmbOwner.Items.Add(userValue);
            //}
        }

        
    }
}

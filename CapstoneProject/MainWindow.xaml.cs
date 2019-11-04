﻿using CapstoneProject.Models;
using CapstoneProject.Pages;
using CapstoneProject.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Task = CapstoneProject.Models.Task;

namespace CapstoneProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Project project;
        public Chart chart;

        public MainWindow()
        {
            InitializeComponent();
            IntroPage introPage = new IntroPage(this);
            frameMain.Content = introPage;
        }

        //By Levi Delezene
        private void mi_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnChart_Click(object sender, RoutedEventArgs e)
        {
            Window window = new frmPERTChart();
            this.Close();
            window.ShowDialog();
        }

        //By Levi Delezene
        private void mi_projectProperties_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Content = new ProjectProperties(ProjectProperties.Mode.UPDATE, project);
        }

        //By Levi Delezene
        private void mi_openProject_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Content = new Chart(project);
        }

        //By Levi Delezene
        public static void numberValidation(object sender, TextCompositionEventArgs e)
        {
            //This allows for multiple '.' in the input. Could probably find something nicer
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new frmPERTChart();
            this.Close();
            window.ShowDialog();
        }

        private void mi_showMin_Click(object sender, RoutedEventArgs e)
        {
            mi_showMin.Header+= "[✓]";
            mi_showMax.Header += "[ ]";
            mi_showMostLikely.Header += "[ ]";
            
            Chart newChart = new Chart(project, "minDuration");
            frameMain.Content = newChart;
        }
        private void mi_showMax_Click(object sender, RoutedEventArgs e)
        {
            mi_showMin.Header += "[ ]";
            mi_showMax.Header += "[✓]";
            mi_showMostLikely.Header += "[ ]";
            Chart newChart = new Chart(project, "maxDuration");
            frameMain.Content = newChart;

        }
        private void mi_showMostLikely_Click(object sender, RoutedEventArgs e)
        {
            mi_showMin.Header += "[ ]";
            mi_showMax.Header += "[ ]";
            mi_showMostLikely.Header += "[✓]";
            Chart newChart = new Chart(project, "mostLikelyDuration");
            frameMain.Content = newChart;
        }

        private void mi_showMax_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

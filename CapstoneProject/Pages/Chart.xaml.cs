﻿using System;
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
using System.Collections;
using CapstoneProject.Models;
using CapstoneProject.Controls;
using CapstoneProject.DAL;

namespace CapstoneProject.Pages
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>

    //By Levi Delezene
    public partial class Chart : Page
    {

        private Point savedMousePosition;
        private TranslateTransform move;
        private ScaleTransform zoom;
        private bool translating = false;
        private Project _project;
        private Task newtask;

        private double dayWidth = Properties.Settings.Default.dayWidth;
        int buttonSpacing = 50;
        int buttonHeight = 25;

        private Dictionary<string, int> dayMonths = new Dictionary<string, int>(); //Dictionary to add months and their respective days

        public Chart(Project project) {
            InitializeComponent();

            Project = project;
            this.PreviewMouseWheel += ZoomCanvas;

            addItemsHashTable();
            addItemsCombo();
        }

        /// <summary>
        /// Written By: Chris Neeser
        /// Date: 10/22/2019
        /// Populates all Tasks and Dependencies for Project from database
        /// </summary>
        /// <returns>List of Root Tasks</returns>
        private List<Task> GetTasksAndDependanciesFromDatabase()
        {
            //Retreive Task List
            OTask oTask = new OTask();
            List<Task> tasks = oTask.Select(_project.Id);


            //Loop through task list and add dependencies
            ODependency oDependency = new ODependency();
            foreach (Task task in tasks)
            {
                List<Dependency> dependencyList = oDependency.Select(task.Id);
                foreach (Dependency dep in dependencyList)
                {
                    Task dependentTask = tasks.Single(s => s.Id == dep.TaskId);
                    task.AddDependentTask(dependentTask);
                }

            }
            return (tasks.Where(s => s.RootNode == true)).ToList();
        }

        // Created by Sandro Pawlidis (10/15/2019)
        private void DrawGraph(List<Task> mainLevel) {
            //TODO: Add support for multiple root nodes.
            int i = DrawSubTasks(mainLevel[0], 50);
        }

        // Created by Sandro Pawlidis (10/15/2019)
        private int DrawSubTasks(Task parent, double topMargin) {
            double newTopMargin = topMargin;
            double minWidth = int.MaxValue;

            int subtaskCount = 0;
            for (int i = 0; i < parent.DependentTasks.Count; i++) {
                subtaskCount += DrawSubTasks(parent.DependentTasks[i], newTopMargin);
                Point start = new Point(((DateTime)parent.StartedDate - _project.StartDate).TotalDays * dayWidth + parent.MinDuration * dayWidth - dayWidth / 4, topMargin + buttonHeight / 2);
                Point end = new Point(((DateTime)parent.DependentTasks[i].StartedDate - _project.StartDate).TotalDays * dayWidth, newTopMargin + buttonHeight / 2);

                double width = start.X + (end.X - start.X) / 2;
                minWidth = (width < minWidth) ? width : minWidth;

                Point midTop = new Point(minWidth, start.Y);
                Point midBot = new Point(minWidth, end.Y);

                List<Point> points = new List<Point>();
                points.Add(start);
                points.Add(midTop);
                points.Add(midBot);
                points.Add(end);

                Path p = getPath(points);
                mainCanvas.Children.Add(p);

                newTopMargin = topMargin + (i + 1) * (buttonHeight + buttonSpacing);
                newTopMargin += subtaskCount * (buttonHeight + buttonSpacing);
            }

            //taskcontrol
            TaskControl t = new TaskControl(parent);
            t.ToolTip = createToolTip(parent);
            Canvas.SetLeft(t, ((DateTime)parent.StartedDate - _project.StartDate).TotalDays * dayWidth);
            Canvas.SetTop(t, topMargin);
            t.Width = parent.MinDuration * dayWidth - dayWidth / 4;
            mainCanvas.Children.Add(t);

            //button
            //Button b = new Button();
            //b.Content = parent.Name;
            //b.ToolTip = createToolTip(parent);
            //Canvas.SetLeft(b, ((DateTime)parent.StartedDate - _project.StartDate).TotalDays * dayWidth);
            //Canvas.SetTop(b, topMargin);
            //b.Width = parent.MinDuration * dayWidth - dayWidth / 4;
            //b.Height = buttonHeight;
            //mainCanvas.Children.Add(b);

            subtaskCount += (parent.DependentTasks.Count > 1) ? parent.DependentTasks.Count - 1 : 0;
            //MessageBox.Show(subtaskCount.ToString());
            return subtaskCount;
        }

        /// <summary>
        /// Written By: Chris Neeser
        /// Date: 10/22/2019
        /// Create the tool tip for a task
        /// </summary>
        /// <param name="task">The task to create the tooltip for</param>
        /// <returns>The tooltip to assign to your controls ToolTip property</returns>
        private TaskToolTip createToolTip(Task task)
        {
            TaskToolTip ttp = new TaskToolTip();
            ttp.Style = (Style)FindResource("AppToolTip");
            ttp.Task = task;
            return ttp;
        }

        // Created by Sandro Pawlidis (10/15/2019)
        private Path getPath(List<Point> points) {
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;

            PathSegmentCollection segments = new PathSegmentCollection();
            for (int i = 1; i < points.Count; i++)
                segments.Add(new LineSegment(points[i], true));

            path.Data = new PathGeometry() {
                Figures = new PathFigureCollection() {
                    new PathFigure() {
                        StartPoint = points[0],
                        Segments = segments
                    }
                }
            };

            return path;
        }

        public Task Newtask
        {
            get
            {
                return newtask;
            }

            set
            {
                newtask = value;
            }
        }

        public Project Project { get => _project; set => _project = value; }

        private void mi_addTask_Click(object sender, RoutedEventArgs e)
        {
            new frmCreateTask(this).ShowDialog();
            AddTask(newtask);
        }

        // Created by Sandro Pawlidis (9/25/2019)
        private void DrawCalendar(int days)
        {
            mainCanvas.Children.Clear();

            int lblCurrentDay = 1;
            int dicIndex = 0;
            for (int i = 0; i < days; i++)
            {
                Line line = new Line();
                line.Stroke = System.Windows.Media.Brushes.Cyan;
                line.StrokeThickness = 2;

                line.X1 = i * dayWidth;
                line.X2 = line.X1;

                line.Y1 = 0;
                line.Y2 = mainCanvas.Height; //Changed .Height to .ActualHeight to make use of auto width and height - Chase

                mainCanvas.Children.Add(line);

                Label lbDay = new Label();
                if (lblCurrentDay > dayMonths.Values.ElementAt(dicIndex))
                {
                    lblCurrentDay = 1;
                    dicIndex++;
                }
                lbDay.Content = lblCurrentDay;

                lblCurrentDay++;

                Canvas.SetLeft(lbDay, line.X1);
                mainCanvas.Children.Add(lbDay);
            }

        }

        public void AddTask(Task task)
        {
            if (task == null) return;
            CalculationService cs = new CalculationService();
            Rect rectVal = cs.dateToChartCoordinate(Project.StartDate,task.StartedDate, task.MaxDuration);
            Grid taskGrid = new Grid();
            Rectangle taskRect = new Rectangle();
            TextBlock taskTextBlock = new TextBlock();
            taskTextBlock.Text = task.Name;

            taskRect.Width = rectVal.Width;
            taskRect.Height = 50;
            taskRect.StrokeThickness = 2;
            taskRect.Stroke = new SolidColorBrush(Colors.Red);

            taskGrid.Children.Add(taskRect);
            taskGrid.Children.Add(taskTextBlock);

            //Canvas.SetTop(taskGrid, 30);
            //Canvas.SetLeft(taskGrid, rectVal.X);
            //mainCanvas.Children.Add(taskGrid);

            TaskControl taskControl = new TaskControl(task);
            taskControl.Width = rectVal.Width;
            taskControl.Height = 50;

            Canvas.SetTop(taskControl, 30);
            Canvas.SetLeft(taskControl, rectVal.X);
            mainCanvas.Children.Add(taskControl);

        }

        // Created by Sandro Pawlidis (9/25/2019)
        private void SetupCanvas()
        {
            //Set the height so that we have a vertical scrollbar
            mainCanvas.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            mainCanvas.Width = 365 * dayWidth; //TO-DO: Necessary for MouseEvents to fire. Discuss. 

            move = new TranslateTransform();
            zoom = new ScaleTransform();

            TransformGroup group = new TransformGroup();
            group.Children.Add(move);
            group.Children.Add(zoom);

            mainCanvas.LayoutTransform = group;
            mainCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        // Created by Sandro Pawlidis (9/25/2019)
        private void ZoomCanvas(object sender, MouseWheelEventArgs e)
        {
            double zoomSpeed = 0.05;

            if (e.Delta > 0)
            {
                zoom.ScaleX += zoomSpeed;
                zoom.ScaleY += zoomSpeed;
            }
            else if (e.Delta < 0)
            {
                zoom.ScaleX -= zoomSpeed;
                zoom.ScaleY -= zoomSpeed;
            }
        }

        // Created by Sandro Pawlidis (9/25/2019)
        private void DragCanvas(object sender, RoutedEventArgs e)
        {
            if (!translating) return;

            Point currentMousePos = Mouse.GetPosition(mainCanvas);
            move.X += currentMousePos.X - savedMousePosition.X;
            move.Y += currentMousePos.Y - savedMousePosition.Y;
        }

        // Created by Sandro Pawlidis (9/25/2019)
        private void SetMouseDrag(object sender, RoutedEventArgs e)
        {
            translating = true;
            savedMousePosition = Mouse.GetPosition(mainCanvas);
        }

        // Created by Sandro Pawlidis (9/25/2019)
        private void ReleaseMouseDrag(object sender, RoutedEventArgs e)
        {
            translating = false;
        }

        // Created by Chris Neeser (10/1/2019)
        Point scrollMousePoint = new Point();
        double hOff = 1;

        private void scrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(scrollViewer);
            hOff = scrollViewer.HorizontalOffset;
            scrollViewer.CaptureMouse();
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (scrollViewer.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(scrollViewer).X));
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.ReleaseMouseCapture();
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            //scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + e.Delta);
        }

        //Added this to handle resizing of the calendar - Chase Torres (9/26/2019)
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //mainCanvas.Children.Clear();
            //DrawCalendar(365);
        }

        //Adds months to the combo box - Chase Torres (9/26/2019)
        private void addItemsCombo()
        {
            foreach (KeyValuePair<string, int> keyEntry in dayMonths)
            {
                comboBoxMonths.Items.Add(keyEntry.Key);
            }
            comboBoxMonths.SelectedIndex = 0;
        }

        // Adds the months and days to the dictionary - Chase Torres (9/26/2019)
        private void addItemsHashTable()
        {
            dayMonths.Add("January", 31);
            dayMonths.Add("February", 28);
            dayMonths.Add("March", 31);
            dayMonths.Add("April", 30);
            dayMonths.Add("May", 31);
            dayMonths.Add("June", 30);
            dayMonths.Add("July", 31);
            dayMonths.Add("August", 31);
            dayMonths.Add("September", 30);
            dayMonths.Add("October", 31);
            dayMonths.Add("November", 30);
            dayMonths.Add("December", 31);
        }

        //Redraws the calendar based off the month selected - Chase Torres(9/26/2019)
        //Modified this to move the scrollbar to the start of the selected month. - Chase Torres(10/7/2019)
        private void ComboBoxMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double monthPosition = 0;
            double totalDays = 0;
            for (int i = 0; i < dayMonths.Keys.ToList().IndexOf((string)comboBoxMonths.SelectedItem); i++)
            {
                totalDays += dayMonths.Values.ElementAt(i);
            }
            monthPosition = totalDays * dayWidth;
            scrollViewer.ScrollToHorizontalOffset(monthPosition);
        }

        //Going to try to use a groupbox as the container to add tasks to the canvas
        private void addGroupBoxCanvas()
        {
            GroupBox taskGroup = new GroupBox();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Maximized;

            SetupCanvas();

            DrawCalendar(365);

            List<Task> taskList = GetTasksAndDependanciesFromDatabase();

            if (taskList.Count > 0)
                DrawGraph(taskList);
        }
    }
}

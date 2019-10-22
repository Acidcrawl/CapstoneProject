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
using System.Windows.Shapes;

namespace CapstoneProject.Windows
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class frmTest : Window
    {
        public frmTest()
        {
            InitializeComponent();
            TaskToolTip ttp = new TaskToolTip();
            ttp.Style = (Style)FindResource("AppToolTip");
            ttp.Content = "Hello";
            ttp.Header = "World!";
            ttp.Task.Name = "Build the calendar canvas.";
            ttp.Task.MostLikelyDuration = 5;
            myButton.ToolTip = ttp;
            //myButton.ToolTip = "Suck it!";
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

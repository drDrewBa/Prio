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

namespace Prio.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void tabIP_Click(object sender, RoutedEventArgs e)
        {
            tabIP.Height = 35;
            tabDue.Height = 30;
            tabComplete.Height = 30;
            tabCancel.Height = 30;
        }

        private void tabDue_Click(object sender, RoutedEventArgs e)
        {

            tabIP.Height = 30;
            tabDue.Height = 35;
            tabComplete.Height = 30;
            tabCancel.Height = 30;
        }

        private void tabComplete_Click(object sender, RoutedEventArgs e)
        {
            tabIP.Height = 30;
            tabDue.Height = 30;
            tabComplete.Height = 35;
            tabCancel.Height = 30;
        }

        private void tabCancel_Click(object sender, RoutedEventArgs e)
        {
            tabIP.Height = 30;
            tabDue.Height = 30;
            tabComplete.Height = 30;
            tabCancel.Height = 35;
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

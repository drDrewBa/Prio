using Prio.Model;
using Prio.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        // Initializing variables ----------------------------------------------------------------------------------------------------------------------------------------
        private int resourcesAvailable, pending, daysDifference;
        private string selectedValue;
        private DateTime selectedDate;
        private ObservableCollection<DataItem> dataItems = new ObservableCollection<DataItem>();

        // Constructor ----------------------------------------------------------------------------------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();

            // Load data from the file
            LoadDataFromFile(@"..\..\Files\IP.txt");

            var viewModel = new MainViewModel();
            DataContext = dataItems;

            DateTime maxSelectableDate = DateTime.Today.AddDays(30);
            datePicker.DisplayDateEnd = maxSelectableDate;
        }

        // Loading datagrids -------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadDataFromFile(string filePath)
        {
            try
            {
                // Read the lines from the file
                var lines = File.ReadAllLines(filePath);
                var data = new List<DataItem>();

                foreach (var line in lines)
                {
                    // Split the line by the comma character
                    var parts = line.Split(',');

                    if (parts.Length == 4)
                    {
                        var item = new DataItem
                        {
                            Task = parts[0],
                            Date = parts[1],
                            Time = parts[2],
                            Priority = parts[3]
                        }; data.Add(item);
                    }
                    else
                    { MessageBox.Show("Invalid data format in the file.");
                        return;
                    }
                }

                // Set the DataGrid's ItemsSource to the list of data
                PrioDataGrid.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data from the file: " + ex.Message);
            }
        }

        // Computing points (basis for ordering elements) --------------------------------------------------------------------------------------------------------------
        public decimal CalculatePoints()
        {
            decimal result = daysDifference - EstimatedTime() + 
                (decimal)(Importance.Value + Complexity.Value + Risk.Value + Mood.Value) +
                pending + resourcesAvailable;

            return result;
        }

        public decimal EstimatedTime()
        {
            if (comboBoxPriority.SelectedItem != null)
            {
                string selectedValue = comboBoxPriority.SelectedItem.ToString();

                // Selection from ComboBox has its own designated value
                switch (selectedValue)
                {
                    case "< 1 hour": return 0.04m;
                    case "1 - 4 hours": return 0.16m;
                    case "Half a day": return 0.5m;
                    case "1 day": return 1m;
                    case "2 - 4 days": return 4m;
                    case "5 days - 1 week": return 7m;
                    case "2 - 3 weeks": return 21m;
                    case "1 month": return 30m;
                    default: return 7m;
                }
            } return 7m;
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = datePicker.SelectedDate.Value;
            TimeSpan difference = selectedDate - DateTime.Today;
            daysDifference = ((int)difference.TotalDays) + 1;
        }

        private void PendingRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            { pending = radioButton.Content.ToString() == "No" ? 1 : 3; }
        }

        private void ResourcesRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            { resourcesAvailable = radioButton.Content.ToString() == "Yes" ? 3 : 1; }
        }

        // Enqueue and dequque tasks --------------------------------------------------------------------------------------------------------------------------------------------------
        private void EnqueueTask()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTask.Text) || txtTask.Text == "Hello!")
                {
                    MessageBox.Show("Enter a valid task description");
                    return;
                }

                if (!datePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select a due date.");
                    return;
                }

                if (comboBoxPriority.SelectedItem == null)
                {
                    MessageBox.Show("Please select a time estimate.");
                    return;
                }

                string newLine = $"{txtTask.Text},{datePicker.SelectedDate.Value:yyyy-MM-dd}," +
                    $"{comboBoxPriority.SelectedItem.ToString()},{CalculatePoints():F2}";
                File.AppendAllLines(@"..\..\Files\IP.txt", new[] { newLine });

                MessageBox.Show("Task added successfully!");

                txtTask.Text = "";
                txtTask.Focus();
                Importance.Value = 1;
                Complexity.Value = 1;
                Risk.Value = 1;
                Mood.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while enqueueing task to the file: " + ex.Message);
            }
        }

        private void DequeueComplete()
        {
            try
            {
                string[] lines = File.ReadAllLines(@"..\..\Files\IP.txt");

                if (lines.Length > 0)
                {
                    string[] parts = lines[0].Split(',');

                    if (parts.Length >= 1)
                    {
                        string firstPart = parts[0];
                        string currentDate = DateTime.Today.ToString("yyyy-MM-dd");
                        string lineToAdd = $"{firstPart},{currentDate}, , ";

                        File.AppendAllLines(@"..\..\Files\Completed.txt", new[] { lineToAdd });

                        var remainingLines = lines.Skip(1).ToArray();

                        File.WriteAllLines(@"..\..\Files\IP.txt", remainingLines);

                        MessageBox.Show("Task completed. Good work!");
                    }
                }
                else
                {
                    MessageBox.Show("No tasks to dequeue.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while dequeuing the task: " + ex.Message);
            }
        }

        private void DequeueCancel()
        {
            try
            {
                string[] lines = File.ReadAllLines(@"..\..\Files\IP.txt");

                if (lines.Length > 0)
                {
                    string[] parts = lines[0].Split(',');

                    if (parts.Length >= 1)
                    {
                        string firstPart = parts[0];
                        string currentDate = DateTime.Today.ToString("yyyy-MM-dd");
                        string lineToAdd = $"{firstPart},{currentDate}, , ";

                        File.AppendAllLines(@"..\..\Files\Canceled.txt", new[] { lineToAdd });

                        var remainingLines = lines.Skip(1).ToArray();

                        File.WriteAllLines(@"..\..\Files\IP.txt", remainingLines);

                        MessageBox.Show("Task canceled.");
                    }
                }
                else
                {
                    MessageBox.Show("No tasks to dequeue.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while dequeuing the task: " + ex.Message);
            }
        }

        // Ordering by Priority -------------------------------------------------------------------------------------------------------------------------------------------------
        private void ReorderFile(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath).ToList(); // Convert to a list
                var orderedLines = OrderByAndSelectLines(lines);

                File.WriteAllLines(filePath, orderedLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while reordering the file: " + ex.Message);
            }
        }

        private List<string> OrderByAndSelectLines(List<string> lines)
        {
            var lineData = new List<(string Line, decimal Value)>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length == 4)
                {
                    if (decimal.TryParse(parts[3], out decimal value))
                    {
                        lineData.Add((line, value));
                    }
                }
            }

            // Custom OrderBy method
            lineData = OrderBy(lineData, ld => ld.Value).ToList();
            var orderedLines = Select(lineData, ld => ld.Line).ToList();

            return orderedLines;
        }

        private IEnumerable<TSource> OrderBy<TSource, TKey>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        {
            var list = source.ToList();
            int n = list.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < n; j++)
                {
                    if (Comparer<TKey>.Default.Compare(keySelector(list[j]), keySelector(list[minIndex])) < 0)
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    // Swap elements
                    TSource temp = list[i];
                    list[i] = list[minIndex];
                    list[minIndex] = temp;
                }
            }

            return list;
        }

        private IEnumerable<TResult> Select<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        // Clicks & window actions -----------------------------------------------------------------------------------------------------------------------------------------------------
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

            btnComplete.IsEnabled = true;
            btnCancel.IsEnabled = true;

            DateColumn.Header = "Due Date";
            TimeColumn.Header = "Est. Time";
            PointsColumn.Header = "Priority";

            LoadDataFromFile(@"..\..\Files\IP.txt");
        }

        private void tabDue_Click(object sender, RoutedEventArgs e)
        {

            tabIP.Height = 30;
            tabDue.Height = 35;
            tabComplete.Height = 30;
            tabCancel.Height = 30;

            btnComplete.IsEnabled = false;
            btnCancel.IsEnabled = false;

            DateColumn.Header = "Due Date";
            TimeColumn.Header = "";
            PointsColumn.Header = "";

            LoadDataFromFile(@"..\..\Files\Due.txt");

        }

        private void tabComplete_Click(object sender, RoutedEventArgs e)
        {
            tabIP.Height = 30;
            tabDue.Height = 30;
            tabComplete.Height = 35;
            tabCancel.Height = 30;

            btnComplete.IsEnabled = false;
            btnCancel.IsEnabled = false;

            DateColumn.Header = "Finished";
            TimeColumn.Header = "";
            PointsColumn.Header = "";

            LoadDataFromFile(@"..\..\Files\Completed.txt");
        }

        private void tabCancel_Click(object sender, RoutedEventArgs e)
        {
            tabIP.Height = 30;
            tabDue.Height = 30;
            tabComplete.Height = 30;
            tabCancel.Height = 35;

            btnComplete.IsEnabled = false;
            btnCancel.IsEnabled = false;

            DateColumn.Header = "Canceled";
            TimeColumn.Header = "";
            PointsColumn.Header = "";

            LoadDataFromFile(@"..\..\Files\Canceled.txt");
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            DequeueComplete();
            LoadDataFromFile(@"..\..\Files\IP.txt");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DequeueCancel();
            LoadDataFromFile(@"..\..\Files\IP.txt");
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EnqueueTask();
            ReorderFile(@"..\..\Files\IP.txt");
            LoadDataFromFile(@"..\..\Files\IP.txt");
        }
    }
}

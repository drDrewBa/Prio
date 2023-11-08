using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

public class Stacks
{
    public static void CompleteTopTask(string entryPath, string exitPath)
    {
        try
        {
            string[] lines = File.ReadAllLines(entryPath);

            if (lines.Length > 0)
            {
                string[] parts = lines[0].Split(',');

                if (parts.Length >= 1)
                {
                    string firstPart = parts[0];
                    string currentDate = DateTime.Today.ToString("yyyy-MM-dd");
                    string lineToAdd = $"{firstPart},{currentDate}, , ";

                    List<string> completedLines = File.ReadAllLines(exitPath).ToList();
                    completedLines.Insert(0, lineToAdd);
                    File.WriteAllLines(exitPath, completedLines);

                    var remainingLines = lines.Skip(1).ToArray();

                    File.WriteAllLines(entryPath, remainingLines);

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

    public static void CancelTopTask(string entryPath, string exitPath)
    {
        try
        {
            string[] lines = File.ReadAllLines(entryPath);

            if (lines.Length > 0)
            {
                string[] parts = lines[0].Split(',');

                if (parts.Length >= 1)
                {
                    string firstPart = parts[0];
                    string currentDate = DateTime.Today.ToString("yyyy-MM-dd");
                    string lineToAdd = $"{firstPart},{currentDate}, , ";

                    List<string> canceledLines = File.ReadAllLines(exitPath).ToList();
                    canceledLines.Insert(0, lineToAdd);
                    File.WriteAllLines(exitPath, canceledLines);

                    var remainingLines = lines.Skip(1).ToArray();
                    File.WriteAllLines(entryPath, remainingLines);

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

    public static void StackDueTasks(string entryPath, string exitPath)
    {
        try
        {
            string[] lines = File.ReadAllLines(entryPath);
            List<string> dueLines = new List<string>();
            List<string> remainingLines = new List<string>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length == 4)
                {
                    string taskDate = parts[1];

                    if (DateTime.TryParse(taskDate, out DateTime dueDate) && dueDate < DateTime.Today)
                    {
                        dueLines.Add($"{parts[0]},{parts[1]}, , ");
                    }
                    else
                    {
                        remainingLines.Add(line);
                    }
                }
            }

            if (dueLines.Count > 0)
            {
                var existingDueLines = File.ReadAllLines(exitPath).ToList();
                existingDueLines.InsertRange(0, dueLines);
                File.WriteAllLines(exitPath, existingDueLines);
                File.WriteAllLines(entryPath, remainingLines);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while stacking due tasks: " + ex.Message);
        }
    }
}

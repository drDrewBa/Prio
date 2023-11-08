using System;
using System.Collections.Generic;
using System.Linq;

public class BinaryHeap
{
    public static List<string> Sort(List<string> lines)
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

        BuildMaxHeap(lineData);

        int n = lineData.Count;
        for (int i = n - 1; i > 0; i--)
        {
            Swap(lineData, 0, i);
            MaxHeapify(lineData, 0, i);
        }

        var orderedLines = lineData.Select(ld => ld.Line).ToList();
        return orderedLines;
    }

    private static void BuildMaxHeap(List<(string Line, decimal Value)> arr)
    {
        int n = arr.Count;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            MaxHeapify(arr, i, n);
        }
    }

    private static void MaxHeapify(List<(string Line, decimal Value)> arr, int i, int n)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n && arr[left].Value > arr[largest].Value)
        {
            largest = left;
        }

        if (right < n && arr[right].Value > arr[largest].Value)
        {
            largest = right;
        }

        if (largest != i)
        {
            Swap(arr, i, largest);
            MaxHeapify(arr, largest, n);
        }
    }

    private static void Swap(List<(string Line, decimal Value)> arr, int i, int j)
    {
        var temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}

using Prio.Model;
using System;

namespace Prio.Model
{
    public class PrioCalculator
    {
        public static decimal CalculatePoints(int daysDifference, decimal estTime, int pending, 
            int resourcesAvailable, int importance, int complexity, int risk, int mood)
        {
            decimal result = daysDifference - estTime +
                (decimal)(importance + complexity + risk + mood) +
                pending + resourcesAvailable;

            return result;
        }
    }
}

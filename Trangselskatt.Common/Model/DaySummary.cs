using System;

namespace Trangselskatt.Common.Model
{
    public class DaySummary
    {
        public DaySummary()
        {
            Dates = new DateTime[] { };
        }

        public string Date { get; set; }
        public int TotalCost { get; set; }
        public DateTime[] Dates { get; set; }
    }

}
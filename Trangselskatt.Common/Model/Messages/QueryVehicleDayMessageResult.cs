using System;
using System.Collections.Generic;

namespace Trangselskatt.Common.Model.Messages
{
    public class QueryVehicleDayMessageResult
    {
        public int TotalTax { get; set; }
        public string Date { get; set; }
        public List<DateTime> History { get; set; }
    }
}

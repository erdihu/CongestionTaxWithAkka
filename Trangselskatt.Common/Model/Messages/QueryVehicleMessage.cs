using System;

namespace Trangselskatt.Common.Model.Messages
{
    public class QueryVehicleMessage
    {
        public QueryVehicleMessage(string regnr, DateTime dateToQuery)
        {
            RegNr = regnr;
            DateToQuery = dateToQuery;
        }

        public string RegNr { get; }
        public DateTime DateToQuery { get; }
    }
}

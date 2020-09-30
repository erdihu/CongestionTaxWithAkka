using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trangselskatt.Common.Model.Messages
{
    public class QueryVehiclesResult
    {
        public IReadOnlyList<Vehicle> Vehicles { get; set; }
    }
}

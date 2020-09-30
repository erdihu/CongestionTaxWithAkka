using System;
using System.Collections.Generic;
using Trangselskatt.Common.Model;

namespace Trangselskatt.Common.Contracts
{
    public interface ITrangselskattCalculator
    {
        int CalculateDayPrice(DateTime dateToCalculatePrice, Vehicle vehicle, IReadOnlyList<DateTime> passageHistory);
    }
}
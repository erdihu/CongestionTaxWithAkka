using System;
using System.Collections.Generic;

namespace Trangselskatt.Common.Contracts
{
    public interface IRedDayProvider
    {
        IReadOnlyList<DateTime> RedDays { get; }
    }
}

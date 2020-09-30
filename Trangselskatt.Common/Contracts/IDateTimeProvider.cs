using System;

namespace Trangselskatt.Common.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}

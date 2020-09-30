using System;

namespace Trangselskatt.Common.Contracts
{
    public interface IPassagePriceProvider
    {
        byte GetDayIndifferentPassagePrice(DateTime dateTime);
    }
}

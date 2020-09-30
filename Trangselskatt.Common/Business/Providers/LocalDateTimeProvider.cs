using System;
using Trangselskatt.Common.Contracts;

namespace Trangselskatt.Common.Business.Providers
{
    public class LocalDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}

using System;
using Trangselskatt.Common.Contracts;

namespace Trangselskatt.Common.Business.Providers
{
    /// <summary>
    /// Hämtar priset beroende på tid
    /// </summary>
    //todo: Data should ideally come from an external system such as database, cache etc. in form of some suitable data structure such as interval tree
    public class PassagePriceProvider : IPassagePriceProvider
    {
        public byte GetDayIndifferentPassagePrice(DateTime dateTime)
        {
            //00:00–05:59
            var interval1Start = GetTimeOfDay(0, 0, 0);
            var interval1Stop = GetTimeOfDay(5, 59, 59);

            //06:00–06:29
            var interval2Start = GetTimeOfDay(6, 0, 0);
            var interval2Stop = GetTimeOfDay(6, 29, 59);

            //06:30–06:59
            var interval3Start = GetTimeOfDay(6, 30, 0);
            var interval3Stop = GetTimeOfDay(6, 59, 59);

            //07:00–07:59
            var interval4Start = GetTimeOfDay(7, 0, 0);
            var interval4Stop = GetTimeOfDay(7, 59, 59);

            //08:00–08:29
            var interval5Start = GetTimeOfDay(8, 0, 0);
            var interval5Stop = GetTimeOfDay(8, 29, 59);

            //08:30–14:59
            var interval6Start = GetTimeOfDay(8, 30, 0);
            var interval6Stop = GetTimeOfDay(14, 59, 59);

            //15:00–15:29
            var interval7Start = GetTimeOfDay(15, 0, 0);
            var interval7Stop = GetTimeOfDay(15, 29, 59);

            //15:30–16:59
            var interval8Start = GetTimeOfDay(15, 30, 0);
            var interval8Stop = GetTimeOfDay(16, 59, 59);

            //17:00–17:59
            var interval9Start = GetTimeOfDay(17, 0, 0);
            var interval9Stop = GetTimeOfDay(17, 59, 59);

            //18:00–18:29
            var interval10Start = GetTimeOfDay(18, 0, 0);
            var interval10Stop = GetTimeOfDay(18, 29, 59);

            //18:30–23:59
            var interval11Start = GetTimeOfDay(18, 30, 0);
            var interval11Stop = GetTimeOfDay(23, 59, 59);


            var passageTimespan = dateTime.TimeOfDay;

            if (interval1Start <= passageTimespan && passageTimespan <= interval1Stop)
            {
                return 0;
            }
            if (interval2Start <= passageTimespan && passageTimespan <= interval2Stop)
            {
                return 9;
            }
            if (interval3Start <= passageTimespan && passageTimespan <= interval3Stop)
            {
                return 16;
            }
            if (interval4Start <= passageTimespan && passageTimespan <= interval4Stop)
            {
                return 22;
            }
            if (interval5Start <= passageTimespan && passageTimespan <= interval5Stop)
            {
                return 16;
            }
            if (interval6Start <= passageTimespan && passageTimespan <= interval6Stop)
            {
                return 9;
            }
            if (interval7Start <= passageTimespan && passageTimespan <= interval7Stop)
            {
                return 16;
            }
            if (interval8Start <= passageTimespan && passageTimespan <= interval8Stop)
            {
                return 22;
            }
            if (interval9Start <= passageTimespan && passageTimespan <= interval9Stop)
            {
                return 16;
            }
            if (interval10Start <= passageTimespan && passageTimespan <= interval10Stop)
            {
                return 9;
            }
            if (interval11Start <= passageTimespan && passageTimespan <= interval11Stop)
            {
                return 0;
            }

            //todo: Log
            throw new Exception("Cannot find price for given date, this should not happen");
        }

        private static TimeSpan GetTimeOfDay(int hour, int minute, int second)
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, today.Day, hour, minute, second).TimeOfDay;
        }
    }
}

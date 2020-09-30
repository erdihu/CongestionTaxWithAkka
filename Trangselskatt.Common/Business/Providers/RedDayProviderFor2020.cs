using System;
using System.Collections.Generic;
using System.Linq;
using Trangselskatt.Common.Contracts;

namespace Trangselskatt.Common.Business.Providers
{
    /// <summary>
    /// Returenrar skattefria dagar för året 2020
    /// </summary>
    //todo: Det är möjligt att använda generisk 'Svenska röda dagar' provider här men detta är utanför omfattningen
    public class RedDayProviderFor2020 : IRedDayProvider
    {
        public IReadOnlyList<DateTime> RedDays => GetRedDays();

        private static IReadOnlyList<DateTime> GetRedDays()
        {
            var weekends = GetDaysBetween(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31))
                .Where(date => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

            var taxfreeDays = new List<DateTime>(
                GetDaysBetween(new DateTime(2020, 7, 1), new DateTime(2020, 7, 31)))
            {
                new DateTime(2020,1,1),
                new DateTime(2020,1,6),
                new DateTime(2020,4,9),
                new DateTime(2020,4,10),
                new DateTime(2020,4,13),
                new DateTime(2020,4,30),
                new DateTime(2020,5,1),
                new DateTime(2020,5,20),
                new DateTime(2020,5,21),
                new DateTime(2020,6,5),
                new DateTime(2020,6,19),
                new DateTime(2020,10,30),
                new DateTime(2020,12,24),
                new DateTime(2020,12,25),
                new DateTime(2020,12,31)
            };

            return weekends.Concat(taxfreeDays).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returnerar dagar mellan två angivna datum
        /// </summary>
        /// <param name="start">Inklusivt</param>
        /// <param name="end">Inklusivt</param>
        private static IEnumerable<DateTime> GetDaysBetween(DateTime start, DateTime end)
        {
            for (var i = start; i <= end; i = i.AddDays(1))
            {
                yield return i;
            }
        }

    }
}

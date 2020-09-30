using System;
using System.Collections.Generic;
using System.Linq;
using Trangselskatt.Common.Contracts;
using Trangselskatt.Common.Model;

namespace Trangselskatt.Common.Business.Services
{
    /// <summary>
    /// Calculates trängselskatt for a given passage time and vehicle type
    /// </summary>
    public class GoteborgTrangselskattCalculator : ITrangselskattCalculator
    {
        private readonly IRedDayProvider _redDayProvider;
        private readonly IPassagePriceProvider _passagePriceProvider;

        public GoteborgTrangselskattCalculator(IRedDayProvider redDayProvider, IPassagePriceProvider passagePriceProvider)
        {
            _redDayProvider = redDayProvider;
            _passagePriceProvider = passagePriceProvider;
        }

        /// <summary>
        /// Beräknar totala beloppet för en angiven dag utifrån historiken.
        /// Historiken kan innehålla dagar som inte är samma med <paramref name="dateToCalculatePrice"/>; de filtreras bort
        /// </summary>
        /// <param name="dateToCalculatePrice">Dagen som beloppet ska beräknas till</param>
        /// <param name="vehicle">Fordonsobjekt</param>
        /// <param name="passageHistory">Passagehistoriken</param>
        /// <returns></returns>
        public int CalculateDayPrice(DateTime dateToCalculatePrice, Vehicle vehicle, IReadOnlyList<DateTime> passageHistory)
        {
            //Bättre att returnera tidigt om fordonet inte är skattepliktigt.
            if (!vehicle.PaysCongestionTax) return 0;

            //Flerpassagerregeln:
            //Den innebär att om man passerar flera betalstationer inom ett tidsintervall på 60 minuter,
            //så betalar man endast en gång. Man beskattas då för passagen med det högsta beloppet.
            //Detta gäller endast för passager i Göteborg.
            //https://www.transportstyrelsen.se/sv/vagtrafik/Trangselskatt/Trangselskatt-i-goteborg/Fragor-och-svar-for-Goteborg/Fragor-svar-om-trangselskatt-i-goteborg1112/


            //Det finns en otydlighet i ett specifikt fall med 60 minuters intervallet. 
            //t.ex. Vad ska hända om 60 minuters intervallet överskrider till nästa dag?
            //Fallet hanteras inte här då det inte kan inträffa enligt nuvarande intervall-belopp trädet.
            //Det är dock en viktig grej att tänka på om trängselskatten tas ut dygnet runt.
            var todaysPassages = passageHistory
                .Where(x => x.Date == dateToCalculatePrice.Date)
                .OrderBy(x => x)
                .ToArray();

            var intervalGroups = CreateGroups(todaysPassages);
            var daySum = intervalGroups.Sum(x => x.intervalPrice);

            //Det maximala beloppet per dag och fordon är 60 kronor.
            //todo: bättre att läsa in maximala beloppet från ett externt system.
            return Math.Min(daySum, 60);
        }


        /// <summary>
        /// Skapar grupperna utifrån dagens passage historiken
        /// </summary>
        /// <param name="todaysPassages"></param>
        /// <returns>Grupperad lista av passager med intervalbeloppet</returns>
        private List<(List<DateTime> passageTimes, int intervalPrice)> CreateGroups(DateTime[] todaysPassages)
        {
            var group = new List<(List<DateTime> passageTimes, int intervalPrice)>();

            if (!todaysPassages.Any()) return group;

            for (var i = 0; i < todaysPassages.Length; i++)
            {
                //Början av en ny grupp
                var groupStart = todaysPassages[i];
                var groupEnd = groupStart.AddMinutes(60);
                var intervalGroup = new List<DateTime> { todaysPassages[i] };

                //Det finns ett nästa elemement i listan
                while (i + 1 >= 0 && todaysPassages.Length > i + 1)
                {
                    var next = todaysPassages[i + 1];
                    //Om nästa elementet större än slutdatumet behöver vi starta en ny grupp utan att inkrementa i:n
                    if (next > groupEnd)
                    {
                        break;
                    }

                    intervalGroup.Add(next);
                    i++;
                }

                //Nu är intervallgruppen klar och vi kan beräkna dess lokala maximum
                group.Add((intervalGroup, GetGroupPrice(intervalGroup)));
            }

            return group;
        }

        /// <summary>
        /// Hämntar maximala beloppet som ska betalas för en intervallgrupp.
        /// </summary>
        /// <param name="intervalGroup"></param>
        /// <returns></returns>
        private int GetGroupPrice(IReadOnlyCollection<DateTime> intervalGroup)
        {
            if (!intervalGroup.Any()) return 0;

            var redDays = _redDayProvider.RedDays;

            return intervalGroup.Max(date => redDays.Contains(date.Date)
                ? 0
                : _passagePriceProvider.GetDayIndifferentPassagePrice(date)
                );
        }
    }
}

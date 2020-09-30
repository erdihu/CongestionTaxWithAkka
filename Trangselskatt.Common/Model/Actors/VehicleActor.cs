using Akka.Actor;
using Akka.Event;
using Akka.Util.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Trangselskatt.Common.Contracts;
using Trangselskatt.Common.Model.Messages;

namespace Trangselskatt.Common.Model.Actors
{
    public class VehicleActor : ReceiveActor
    {
        private readonly Vehicle _vehicle;
        private readonly ITrangselskattCalculator _trangselskattCalculator;
        private readonly Dictionary<DateTime, int> _daySumMap;

        private readonly List<DateTime> _passageTimes;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public VehicleActor(Vehicle vehicle, ITrangselskattCalculator trangselskattCalculator)
        {
            _vehicle = vehicle;
            _trangselskattCalculator = trangselskattCalculator;
            _passageTimes = new List<DateTime>();
            _daySumMap = new Dictionary<DateTime, int>();

            Receive<VehiclePassedPaymentStation>(RegisterPassage);
            Receive<QueryVehicleMessage>(QueryVehiclePassages);
            Receive<UpdatePreliminaryPriceMessage>(UpdatePreliminaryPrice);
        }

        private void QueryVehiclePassages(QueryVehicleMessage message)
        {
            Log.Info($"[{message.RegNr} - {message.DateToQuery.ToShortDateString()}] Vehicle passage history query received");
            var history = new List<DateTime>(_passageTimes.Where(x => x.Date == message.DateToQuery.Date));

            if (!_daySumMap.ContainsKey(message.DateToQuery.Date))
            {
                Sender.Tell(
                    new MessageResult(false,
                        $"No records found for {message.DateToQuery.ToShortDateString()}."));
                Log.Info(
                    $"[{message.RegNr} - {message.DateToQuery.ToShortDateString()} No records found");
            }
            else
            {
                Sender.Tell(new QueryVehicleDayMessageResult
                {
                    History = history,
                    Date = message.DateToQuery.ToShortDateString(),
                    TotalTax = _daySumMap[message.DateToQuery.Date]
                });
                Log.Info(
                    $"[{message.RegNr} - {message.DateToQuery.ToShortDateString()} History returned. Total tax: {_daySumMap[message.DateToQuery.Date]}");
            }
        }

        private void RegisterPassage(VehiclePassedPaymentStation message)
        {
            _passageTimes.Add(message.Time);
            Self.Tell(new UpdatePreliminaryPriceMessage { DateToUpdate = message.Time });
            Log.Info($"[{message.RegistrationNumber} - {message.Time}] Adding passage");
        }

        private void UpdatePreliminaryPrice(UpdatePreliminaryPriceMessage message)
        {
            var currentDayPrice = _trangselskattCalculator.CalculateDayPrice(message.DateToUpdate, _vehicle, _passageTimes.AsReadOnly());
            _daySumMap.AddOrSet(message.DateToUpdate.Date, currentDayPrice);
            Log.Info($"[{_vehicle.RegistrationNumber} - {message.DateToUpdate}] New day total: {currentDayPrice}");
        }

    }
}

using Akka.Actor;
using Akka.Event;
using System.Collections.Generic;
using Trangselskatt.Common.Contracts;
using Trangselskatt.Common.Model.Messages;

namespace Trangselskatt.Common.Model.Actors
{
    public class VehicleCoordinatorActor : ReceiveActor
    {
        private readonly ITrangselskattCalculator _trtTrangselskattCalculator;
        private readonly Dictionary<Vehicle, IActorRef> _vehicleToActorMap;
        private readonly Dictionary<string, IActorRef> _regnrToActorMap;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public VehicleCoordinatorActor(ITrangselskattCalculator trtTrangselskattCalculator)
        {
            _trtTrangselskattCalculator = trtTrangselskattCalculator;
            _vehicleToActorMap = new Dictionary<Vehicle, IActorRef>();
            _regnrToActorMap = new Dictionary<string, IActorRef>();

            Receive<RegisterVehicleMessage>(HandleRegisterVehicle);
            Receive<QueryExistingVehiclesMessage>(HandleQueryVehicles);
            Receive<VehiclePassedPaymentStation>(ForwardVehiclePassedPaymentStation);
            Receive<QueryVehicleMessage>(ForwardQueryVehicleMessage);
        }

        private void ForwardQueryVehicleMessage(QueryVehicleMessage message)
        {
            if (!_regnrToActorMap.ContainsKey(message.RegNr))
            {
                Sender.Tell(new MessageResult(false, "All vehicles must be registered first."));
            }
            else
            {
                _regnrToActorMap[message.RegNr].Ask(message).PipeTo(Sender);
            }
        }

        private void ForwardVehiclePassedPaymentStation(VehiclePassedPaymentStation message)
        {
            if (!_regnrToActorMap.ContainsKey(message.RegistrationNumber))
            {
                Sender.Tell(new MessageResult(false, "All vehicles must be registered first."));
                Log.Info($"Unregisted vehicle ({message.RegistrationNumber})!");
            }
            else
            {
                _regnrToActorMap[message.RegistrationNumber].Tell(
                    new VehiclePassedPaymentStation(message.RegistrationNumber, message.Time));
            }

        }

        private void HandleQueryVehicles(QueryExistingVehiclesMessage message)
        {
            var list = new List<Vehicle>(_vehicleToActorMap.Count);
            foreach (var vehicle in _vehicleToActorMap.Keys)
            {
                list.Add(vehicle);
            }

            Sender.Tell(new QueryVehiclesResult { Vehicles = list.AsReadOnly() });
            Log.Info($"Returning vehicle list. Count {list.Count}");
        }

        private void HandleRegisterVehicle(RegisterVehicleMessage message)
        {
            if (!_vehicleToActorMap.ContainsKey(message.Vehicle))
            {
                var actor = Context.ActorOf(Props.Create(() =>
                    new VehicleActor(message.Vehicle, _trtTrangselskattCalculator)));
                _vehicleToActorMap.Add(message.Vehicle, actor);
                _regnrToActorMap.Add(message.Vehicle.RegistrationNumber, actor);

                Sender.Tell(new MessageResult(true));
                Log.Info($"{message.Vehicle} added.");
            }
            else
            {
                Sender.Tell(new MessageResult(false, "Vehicle already exists"));
                Log.Info($"{message.Vehicle} already exists.");
            }
        }

    }
}

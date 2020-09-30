using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.StructureMap;
using System;
using Trangselskatt.Common.IoC;
using Trangselskatt.Common.Model.Actors;

namespace Trangselskatt.Service
{
    class Program
    {
        private static ActorSystem ActorSystem;

        static void Main(string[] args)
        {
            var container = DependencyContainer.Build();
            ActorSystem = Akka.Actor.ActorSystem.Create("TrangselskattActorSystem");
            var resolver = new StructureMapDependencyResolver(container, ActorSystem);
            var _ = ActorSystem.ActorOf(ActorSystem.DI().Props<VehicleCoordinatorActor>(), "VehicleCoordinator");

            Console.ReadLine();
        }
    }
}

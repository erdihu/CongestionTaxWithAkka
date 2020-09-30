using System;

namespace Trangselskatt.Web.Models
{
    /// <summary>
    /// Det enda actorsystemet som ska leva i IIS:en. Ifall IIS:en startas om ska data leva i en annan applikation
    /// </summary>
    public static class TrangselskattActorSystem
    {
        private static Akka.Actor.ActorSystem ActorSystem;

        public static void Create()
        {
            ActorSystem = Akka.Actor.ActorSystem.Create("TrangselskattActorSystem");
            ActorReferences.FordonApiController =
                ActorSystem.ActorSelection("akka.tcp://TrangselskattActorSystem@127.0.0.1:8091/user/VehicleCoordinator")
                    .ResolveOne(TimeSpan.FromSeconds(10))
                    .Result;

        }

        public static void Shutdown()
        {
            ActorSystem.Terminate().Wait();
            ActorSystem.WhenTerminated.Wait();
        }

        public static class ActorReferences
        {
            public static Akka.Actor.IActorRef FordonApiController { get; set; }
        }
    }
}

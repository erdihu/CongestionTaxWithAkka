using Akka.Actor;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Trangselskatt.Common.Model;
using Trangselskatt.Common.Model.Messages;
using Trangselskatt.Web.Models;

namespace Trangselskatt.Web.Controllers.ApiControllers
{
    /// <summary>
    /// Huvudinterface för användare som står utanför systemet.
    /// </summary>
    public class FordonController : ApiController
    {

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var result = await TrangselskattActorSystem.ActorReferences
                .FordonApiController.Ask(new QueryExistingVehiclesMessage());

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSummary(string regnr, DateTime date)
        {
            var queryVehicleMessage = new QueryVehicleMessage(regnr, date);
            var result = await TrangselskattActorSystem.ActorReferences
                .FordonApiController.Ask(queryVehicleMessage);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IHttpActionResult> Register(string regnr, VehicleType type)
        {
            var vehicle = new Vehicle(regnr, type);

            var result = await TrangselskattActorSystem.ActorReferences
                .FordonApiController.Ask(new RegisterVehicleMessage(vehicle));

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult RegisterPass(string regnr, DateTime passTime)
        {
            var message = new VehiclePassedPaymentStation(regnr, passTime);

            TrangselskattActorSystem.ActorReferences
                .FordonApiController.Tell(message);

            return Ok(new MessageResult(true, "Query received"));
        }
    }
}

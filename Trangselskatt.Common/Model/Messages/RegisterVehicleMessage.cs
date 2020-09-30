namespace Trangselskatt.Common.Model.Messages
{
    public class RegisterVehicleMessage
    {
        public RegisterVehicleMessage(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public Vehicle Vehicle { get; }
    }
}

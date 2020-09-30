using System;

namespace Trangselskatt.Common.Model.Messages
{
    public class VehiclePassedPaymentStation
    {
        public VehiclePassedPaymentStation(string registrationNumber, DateTime time)
        {
            RegistrationNumber = registrationNumber;
            Time = time;
        }

        public string RegistrationNumber { get; }
        public DateTime Time { get; }
    }
}

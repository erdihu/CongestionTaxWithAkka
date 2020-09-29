using System;

namespace Trangselskatt.Common.Model
{
    /// <summary>
    /// Definierar minsta möjliga egenskaper som måste finnas i ett fordonsobjekt.
    /// </summary>
    public class Vehicle : IEquatable<Vehicle>
    {
        /// <summary>
        /// Registreringsnummer
        /// </summary>
        public string RegistrationNumber { get; }

        /// <summary>
        /// Fordonstyp
        /// </summary>
        public VehicleType Type { get; }

        public Vehicle(string registrationNumber, VehicleType type = VehicleType.Other)
        {
            RegistrationNumber = registrationNumber;
            Type = type;
        }

        /// <summary>
        /// Definierar regler huruvida en fordon är trängselskattepliktigt
        /// </summary>
        /// <returns>true om fordonet är trängselskattepliktigt, false annars</returns>
        public bool IsTrangselskattepliktigt()
        {
            return Type == VehicleType.Other;
        }

        public bool Equals(Vehicle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return RegistrationNumber == other.RegistrationNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vehicle)obj);
        }

        public override int GetHashCode()
        {
            return (RegistrationNumber != null ? RegistrationNumber.GetHashCode() : 0);
        }

        public static bool operator ==(Vehicle left, Vehicle right)
        {
            return left != null && left.Equals(right);
        }

        public static bool operator !=(Vehicle left, Vehicle right)
        {
            return !(left == right);
        }
    }
}

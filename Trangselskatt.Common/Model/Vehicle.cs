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

        /// <summary>
        /// Returnerar huruvida ett fordon är trängselskattepliktigt
        /// </summary>
        public bool PaysCongestionTax => IsTrangselskattepliktigt();

        public Vehicle(string registrationNumber, VehicleType type = VehicleType.Other)
        {
            RegistrationNumber = registrationNumber ?? throw new ArgumentNullException(nameof(registrationNumber));
            Type = type;
        }

        /// <summary>
        /// Definierar regler huruvida ett fordon är trängselskattepliktigt
        /// </summary>
        /// <returns>true om fordonet är trängselskattepliktigt, false annars</returns>
        public bool IsTrangselskattepliktigt()
        {
            return Type == VehicleType.Other;
        }

        #region IEquatable
        public bool Equals(Vehicle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;

            return RegistrationNumber == other.RegistrationNumber;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Vehicle);
        }

        public override int GetHashCode()
        {
            return RegistrationNumber != null ? RegistrationNumber.GetHashCode() : 0;
        }

        public static bool operator ==(Vehicle left, Vehicle right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Vehicle left, Vehicle right)
        {
            return !(left == right);
        }
        #endregion

        public override string ToString()
        {
            return $"Vehicle: Registreringsnummer:{RegistrationNumber}, Type: {Type.ToString()}";
        }
    }
}

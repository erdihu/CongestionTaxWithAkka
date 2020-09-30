using System.Runtime.Serialization;

namespace Trangselskatt.Common.Model
{
    /// <summary>
    /// Vissa fordon är undtagna från trängselskatt, allt annat är trängselskattepliktigt.
    /// </summary>
    public enum VehicleType
    {
        [EnumMember(Value = "Utryckningsfordon")]
        Utryckningsfordon = 1,

        [EnumMember(Value = "Bussar med totalvikt av minst 14 ton")]
        BussarMedTotalViktAvMinst14Ton = 2,

        [EnumMember(Value = "Diplomatregistrerade fordon")]
        DiplomatregistreradeFordon = 3,

        [EnumMember(Value = "Motorcyklar")]
        Motorcyklar = 4,

        [EnumMember(Value = "Militära fordon")]
        MilitaraFordon = 5,

        [EnumMember(Value = "Annat")]
        Other = 999
    }
}
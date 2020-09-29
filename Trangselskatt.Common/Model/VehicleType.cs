namespace Trangselskatt.Common.Model
{
    /// <summary>
    /// Vissa fordon är undtagna från trängselskatt, allt annat är trängselskattepliktigt.
    /// </summary>
    public enum VehicleType
    {
        Utryckningsfordon = 1,
        BussarMedTotalViktAvMinst14Ton = 2,
        DiplomatregistreradeFordon = 3,
        Motorcyklar = 4,
        MilitaraFordon = 5,
        Other = 999
    }
}
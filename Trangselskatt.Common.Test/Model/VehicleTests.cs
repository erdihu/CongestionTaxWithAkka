using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Trangselskatt.Common.Model;

namespace Trangselskatt.Common.Test.Model
{
    [TestClass]
    public class VehicleTests
    {
        [TestMethod]
        public void Vehicle_Should_Be_Trangselskattepliktigt_If_Type_Is_Other()
        {
            //Setup
            var car1 = new Vehicle("ABC001");
            var car2 = new Vehicle("ABC002", VehicleType.Other);

            //Act

            //Assert
            Assert.IsTrue(car1.IsTrangselskattepliktigt());
            Assert.IsTrue(car2.IsTrangselskattepliktigt());
        }

        [TestMethod]
        public void Vehicle_Should_Not_Be_Trangselskattepliktigt_If_Type_Is_Not_Other()
        {
            //Setup
            var car1 = new Vehicle("ABC001", VehicleType.BussarMedTotalViktAvMinst14Ton);
            var car2 = new Vehicle("ABC002", VehicleType.DiplomatregistreradeFordon);
            var car3 = new Vehicle("ABC003", VehicleType.MilitaraFordon);
            var car4 = new Vehicle("ABC004", VehicleType.Motorcyklar);
            var car5 = new Vehicle("ABC005", VehicleType.Utryckningsfordon);

            //Act

            //Assert
            Assert.IsFalse(car1.IsTrangselskattepliktigt());
            Assert.IsFalse(car2.IsTrangselskattepliktigt());
            Assert.IsFalse(car3.IsTrangselskattepliktigt());
            Assert.IsFalse(car4.IsTrangselskattepliktigt());
            Assert.IsFalse(car5.IsTrangselskattepliktigt());
        }

        [TestMethod]
        public void Vehicles_Should_Be_Equal_If_They_Have_Same_RegNr()
        {
            //Setup
            var left = new Vehicle("ABC123");
            var right = new Vehicle("ABC123");

            //Act
            var areEqual = left == right;

            //Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Vehicles_Cannot_Be_Created_With_Null_RegNr()
        {
            //Setup & Act
            var _ = new Vehicle(null);
        }
    }
}

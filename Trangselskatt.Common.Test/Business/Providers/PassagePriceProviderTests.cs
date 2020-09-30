using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Trangselskatt.Common.Business.Providers;
using Trangselskatt.Common.Contracts;

namespace Trangselskatt.Common.Test.Business.Providers
{
    [TestClass]
    public class PassagePriceProviderTests
    {
        private IPassagePriceProvider _passagePriceProvider;

        [TestInitialize]
        public void Initialize()
        {
            _passagePriceProvider = new PassagePriceProvider();
        }

        [TestMethod]
        public void GetDayIndifferentPassagePrice_Should_Show_Respect_To_Time_Of_Day()
        {
            //Setup

            var redDay = new DateTime(2020, 1, 1, 9, 0, 0);
            var freeTime1 = new DateTime(2020, 1, 2, 5, 59, 0);
            var freeTime2 = new DateTime(2020, 1, 2, 18, 30, 0);
            var nineSekTime1 = new DateTime(2020, 1, 2, 6, 0, 0);
            var nineSekTime2 = new DateTime(2020, 1, 2, 6, 29, 0);
            var twentytwoSekTime1 = new DateTime(2020, 1, 2, 15, 30, 0);
            var twentytwoSekTime2 = new DateTime(2020, 1, 2, 16, 59, 0);

            //Act
            var redDayIndifferentPrice = _passagePriceProvider.GetDayIndifferentPassagePrice(redDay);
            var freeTime1Price = _passagePriceProvider.GetDayIndifferentPassagePrice(freeTime1);
            var freeTime2Price = _passagePriceProvider.GetDayIndifferentPassagePrice(freeTime2);
            var nineSekTime1Price = _passagePriceProvider.GetDayIndifferentPassagePrice(nineSekTime1);
            var nineSekTime2Price = _passagePriceProvider.GetDayIndifferentPassagePrice(nineSekTime2);
            var twentytwoSekTime1Price = _passagePriceProvider.GetDayIndifferentPassagePrice(twentytwoSekTime1);
            var twentytwoSekTime2Price = _passagePriceProvider.GetDayIndifferentPassagePrice(twentytwoSekTime2);

            //Assert
            Assert.AreEqual(9, redDayIndifferentPrice);
            Assert.AreEqual(0, freeTime1Price);
            Assert.AreEqual(0, freeTime2Price);
            Assert.AreEqual(9, nineSekTime1Price);
            Assert.AreEqual(9, nineSekTime2Price);
            Assert.AreEqual(22, twentytwoSekTime1Price);
            Assert.AreEqual(22, twentytwoSekTime2Price);

        }
    }
}

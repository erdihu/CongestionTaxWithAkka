using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Trangselskatt.Common.Business.Providers;
using Trangselskatt.Common.Business.Services;
using Trangselskatt.Common.Contracts;
using Trangselskatt.Common.Model;

namespace Trangselskatt.Common.Test.Business.Services
{
    [TestClass]
    public class GoteborgTrangselskattCalculatorTests
    {
        private ITrangselskattCalculator _trangselskattCalculator;
        private IPassagePriceProvider _passagePriceProvider;

        private Mock<IRedDayProvider> _redDayProviderMock;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        [TestInitialize]
        public void Initialize()
        {
            _redDayProviderMock = new Mock<IRedDayProvider>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _passagePriceProvider = new PassagePriceProvider();

            _trangselskattCalculator = new GoteborgTrangselskattCalculator(_redDayProviderMock.Object, _passagePriceProvider);
        }

        [TestMethod]
        public void Non_Other_Vehicle_Types_Should_Not_Pay_Tax()
        {
            //Setup
            _dateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2020, 1, 2, 8, 30, 0));
            var redDayList = new List<DateTime> { new DateTime(2020, 1, 1) };
            _redDayProviderMock.Setup(x => x.RedDays).Returns(redDayList);

            var nonOtherVehicles = new List<Vehicle>();
            for (var i = 1; i <= 5; i++)
            {
                nonOtherVehicles.Add(new Vehicle("ABC101", (VehicleType)i));
            }

            var history = new List<DateTime>
            {
                new DateTime(2020, 1, 2, 7, 0, 0), //group 1 - 22
                new DateTime(2020, 1, 2, 7, 59, 0), //group 1 - 0
                new DateTime(2020, 1, 2, 8, 29, 0), //group 2 - 16
                new DateTime(2020, 1, 2, 8, 59, 0), //group 2 - 0
                new DateTime(2020, 1, 2, 9, 19, 0), //group 2 - 0
                new DateTime(2020, 1, 2, 9, 30, 0), //group 3 - 9
            }.AsReadOnly();

            //Act
            var prices = new List<int>();
            foreach (var nonOtherVehicle in nonOtherVehicles)
            {
                var totalPrice = _trangselskattCalculator.CalculateDayPrice(history[0], nonOtherVehicle, history);
                prices.Add(totalPrice);
            }


            //Assert
            for (var i = 0; i < prices.Count; i++)
            {
                var price = prices[i];
                Assert.AreEqual(0, price, $"Error in index {i}");
            }
        }

        [TestMethod]
        public void Multiple_Passages_Should_Be_Calculated_Correctly()
        {
            //Setup
            _dateTimeProvider.Setup(x => x.Now).Returns(new DateTime(2020, 1, 2, 8, 30, 0));
            var redDayList = new List<DateTime> { new DateTime(2020, 1, 1) };
            _redDayProviderMock.Setup(x => x.RedDays).Returns(redDayList);

            var car = new Vehicle("ABC101", VehicleType.Other);
            var history = new List<DateTime>
            {
                new DateTime(2020, 1, 2, 7, 0, 0), //group 1 - 22
                new DateTime(2020, 1, 2, 7, 59, 0), //group 1 - 0
                new DateTime(2020, 1, 2, 8, 29, 0), //group 2 - 16
                new DateTime(2020, 1, 2, 8, 59, 0), //group 2 - 0
                new DateTime(2020, 1, 2, 9, 19, 0), //group 2 - 0
                new DateTime(2020, 1, 2, 9, 30, 0), //group 3 - 9
            }.AsReadOnly();

            //Act
            var totalPrice = _trangselskattCalculator.CalculateDayPrice(history[0], car, history);

            //Assert
            Assert.AreEqual(22 + 16 + 9, totalPrice);

        }

        private IReadOnlyList<(DateTime passageTime, byte price)> CreateHistoryFor(IEnumerable<DateTime> dates)
        {
            var history = new List<(DateTime passageTime, byte price)>();
            foreach (var date in dates)
            {
                var price = _passagePriceProvider.GetDayIndifferentPassagePrice(date);
                history.Add((date, price));
            }

            return history.AsReadOnly();
        }
    }
}

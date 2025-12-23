using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Tyuiu.RadchenkoAV.Sprint7.ProjectV10.Lib;

namespace Tyuiu.RadchenkoAV.Sprint7.ProjectV10.Test
{
    [TestClass]
    public class DataServiceTest
    {
        [TestMethod]
        public void ValidGetTotalCost()
        {
            DataService ds = new DataService();

            List<DataService.Order> orders = new List<DataService.Order>
            {
                new DataService.Order { OrderCost = 1000 },
                new DataService.Order { OrderCost = 2000 },
                new DataService.Order { OrderCost = 3000 }
            };

            decimal result = ds.GetTotalCost(orders);
            decimal wait = 6000;

            Assert.AreEqual(wait, result);
        }

        [TestMethod]
        public void ValidGetAverageCost()
        {
            DataService ds = new DataService();

            List<DataService.Order> orders = new List<DataService.Order>
            {
                new DataService.Order { OrderCost = 1000 },
                new DataService.Order { OrderCost = 2000 },
                new DataService.Order { OrderCost = 3000 }
            };

            decimal result = ds.GetAverageCost(orders);
            decimal wait = 2000;

            Assert.AreEqual(wait, result);
        }

        [TestMethod]
        public void ValidGetOrderCount()
        {
            DataService ds = new DataService();

            List<DataService.Order> orders = new List<DataService.Order>
            {
                new DataService.Order(),
                new DataService.Order(),
                new DataService.Order()
            };

            int result = ds.GetOrderCount(orders);
            int wait = 3;

            Assert.AreEqual(wait, result);
        }

        [TestMethod]
        public void ValidSearchByProductName()
        {
            DataService ds = new DataService();

            List<DataService.Order> orders = new List<DataService.Order>
            {
                new DataService.Order { ProductName = "Ноутбук" },
                new DataService.Order { ProductName = "Телефон" },
                new DataService.Order { ProductName = "Планшет" }
            };

            var result = ds.SearchByProductName(orders, "Ноут");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Ноутбук", result[0].ProductName);
        }
    }
}
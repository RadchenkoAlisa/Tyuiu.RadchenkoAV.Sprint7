using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tyuiu.RadchenkoAV.Sprint7.ProjectV10.Lib
{
    public class DataService
    {
        public class Order
        {
            public int OrderNumber { get; set; }
            public DateTime ExecutionDate { get; set; }
            public decimal OrderCost { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
            public int Quantity { get; set; }
            public string ClientLastName { get; set; }
            public string ClientFirstName { get; set; }
            public string ClientMiddleName { get; set; }
            public string AccountNumber { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }

            // Конструктор для инициализации значений по умолчанию
            public Order()
            {
                ProductName = "";
                ClientLastName = "";
                ClientFirstName = "";
                ClientMiddleName = "";
                AccountNumber = "";
                Address = "";
                Phone = "";
                OrderNumber = 0;
                OrderCost = 0;
                ProductPrice = 0;
                Quantity = 0;
                ExecutionDate = DateTime.Now;
            }
        }

        public List<Order> LoadFromCsv(string filePath)
        {
            List<Order> orders = new List<Order>();

            if (!File.Exists(filePath))
                return orders;

            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++) // Пропускаем заголовок
            {
                var values = lines[i].Split(',');
                if (values.Length >= 12)
                {
                    Order order = new Order
                    {
                        OrderNumber = int.TryParse(values[0], out int orderNum) ? orderNum : 0,
                        ExecutionDate = DateTime.TryParse(values[1], out DateTime execDate) ? execDate : DateTime.Now,
                        OrderCost = decimal.TryParse(values[2], out decimal cost) ? cost : 0,
                        ProductName = values.Length > 3 ? values[3] : "",
                        ProductPrice = decimal.TryParse(values[4], out decimal price) ? price : 0,
                        Quantity = int.TryParse(values[5], out int qty) ? qty : 0,
                        ClientLastName = values.Length > 6 ? values[6] : "",
                        ClientFirstName = values.Length > 7 ? values[7] : "",
                        ClientMiddleName = values.Length > 8 ? values[8] : "",
                        AccountNumber = values.Length > 9 ? values[9] : "",
                        Address = values.Length > 10 ? values[10] : "",
                        Phone = values.Length > 11 ? values[11] : ""
                    };
                    orders.Add(order);
                }
            }

            return orders;
        }

        public void SaveToCsv(string filePath, List<Order> orders)
        {
            List<string> lines = new List<string>
            {
                "OrderNumber,ExecutionDate,OrderCost,ProductName,ProductPrice,Quantity,ClientLastName,ClientFirstName,ClientMiddleName,AccountNumber,Address,Phone"
            };

            foreach (var order in orders)
            {
                lines.Add($"{order.OrderNumber},{order.ExecutionDate:yyyy-MM-dd},{order.OrderCost}," +
                         $"{(string.IsNullOrEmpty(order.ProductName) ? "" : order.ProductName)}," +
                         $"{order.ProductPrice},{order.Quantity}," +
                         $"{(string.IsNullOrEmpty(order.ClientLastName) ? "" : order.ClientLastName)}," +
                         $"{(string.IsNullOrEmpty(order.ClientFirstName) ? "" : order.ClientFirstName)}," +
                         $"{(string.IsNullOrEmpty(order.ClientMiddleName) ? "" : order.ClientMiddleName)}," +
                         $"{(string.IsNullOrEmpty(order.AccountNumber) ? "" : order.AccountNumber)}," +
                         $"{(string.IsNullOrEmpty(order.Address) ? "" : order.Address)}," +
                         $"{(string.IsNullOrEmpty(order.Phone) ? "" : order.Phone)}");
            }

            File.WriteAllLines(filePath, lines);
        }

        public List<Order> CreateSampleData()
        {
            return new List<Order>
            {
                new Order
                {
                    OrderNumber = 1001,
                    ExecutionDate = DateTime.Now.AddDays(7),
                    OrderCost = 15000,
                    ProductName = "Ноутбук",
                    ProductPrice = 15000,
                    Quantity = 1,
                    ClientLastName = "Иванов",
                    ClientFirstName = "Иван",
                    ClientMiddleName = "Иванович",
                    AccountNumber = "40817810099910004312",
                    Address = "г. Тюмень, ул. Ленина, д. 10",
                    Phone = "+79224561234"
                },
                new Order
                {
                    OrderNumber = 1002,
                    ExecutionDate = DateTime.Now.AddDays(5),
                    OrderCost = 30000,
                    ProductName = "Телевизор",
                    ProductPrice = 30000,
                    Quantity = 1,
                    ClientLastName = "Петров",
                    ClientFirstName = "Петр",
                    ClientMiddleName = "Петрович",
                    AccountNumber = "40817810099910004313",
                    Address = "г. Тюмень, ул. Республики, д. 20",
                    Phone = "+79224561235"
                },
                new Order
                {
                    OrderNumber = 1003,
                    ExecutionDate = DateTime.Now.AddDays(10),
                    OrderCost = 25000,
                    ProductName = "Холодильник",
                    ProductPrice = 25000,
                    Quantity = 1,
                    ClientLastName = "Сидорова",
                    ClientFirstName = "Анна",
                    ClientMiddleName = "Владимировна",
                    AccountNumber = "40817810099910004314",
                    Address = "г. Тюмень, ул. Мельникайте, д. 15",
                    Phone = "+79224561236"
                }
            };
        }

        public List<Order> SearchByProductName(List<Order> orders, string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return orders;

            return orders.Where(o =>
                !string.IsNullOrEmpty(o.ProductName) &&
                o.ProductName.ToLower().Contains(productName.ToLower())).ToList();
        }

        public List<Order> SearchByClientLastName(List<Order> orders, string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return orders;

            return orders.Where(o =>
                !string.IsNullOrEmpty(o.ClientLastName) &&
                o.ClientLastName.ToLower().Contains(lastName.ToLower())).ToList();
        }

        public List<Order> SortByDate(List<Order> orders, bool ascending = true)
        {
            return ascending
                ? orders.OrderBy(o => o.ExecutionDate).ToList()
                : orders.OrderByDescending(o => o.ExecutionDate).ToList();
        }

        public List<Order> SortByCost(List<Order> orders, bool ascending = true)
        {
            return ascending
                ? orders.OrderBy(o => o.OrderCost).ToList()
                : orders.OrderByDescending(o => o.OrderCost).ToList();
        }

        public decimal GetTotalCost(List<Order> orders)
        {
            return orders.Sum(o => o.OrderCost);
        }

        public decimal GetAverageCost(List<Order> orders)
        {
            return orders.Any() ? orders.Average(o => o.OrderCost) : 0;
        }

        public int GetOrderCount(List<Order> orders)
        {
            return orders.Count;
        }

        public decimal GetMaxCost(List<Order> orders)
        {
            return orders.Any() ? orders.Max(o => o.OrderCost) : 0;
        }

        public decimal GetMinCost(List<Order> orders)
        {
            return orders.Any() ? orders.Min(o => o.OrderCost) : 0;
        }
    }
}
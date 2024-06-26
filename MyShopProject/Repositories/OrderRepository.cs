﻿using Microsoft.EntityFrameworkCore;
using MyShopProject.Model;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MyShopProject.Repositories
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalPrice { get; set; }

        public byte Status { get; set; }
    }

    public class OrderRepository
    {
        public ObservableCollection<Order> GetAllOrders()
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            using (var context = new MyShopContext())
            {
                orders = new ObservableCollection<Order>(context.Orders
                                       .Include(o => o.User).ToList());
            }
            return orders;
        }

        public ObservableCollection<Order> GetOrdersByUserId(int userId)
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            using (var context = new MyShopContext())
            {
                orders = new ObservableCollection<Order>(context.Orders
                    .Include(o => o.User)
                    .Where(o => o.UserId == userId).ToList());
            }
            return orders;
        }

        public Order GetOrderById(int orderId)
        {
            Order order = new Order();
            using (var context = new MyShopContext())
            {
                order = context.Orders
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.Id == orderId)!;
            }
            return order;
        }

        public (ObservableCollection<OrderDetail>, int) GetAllOrderDetailsByPagination(DateTime? startDate, DateTime? endDate, int Page, int ItemsPerPage)
        {
            ObservableCollection<OrderDetail> orderDetails = new ObservableCollection<OrderDetail>();
            int totalCount = 0;

            endDate = endDate?.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (var context = new MyShopContext())
            {
                var orders = context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Promotion)
                    .Where(o => (!startDate.HasValue || o.CreatedAt >= startDate.Value) && (!endDate.HasValue || o.CreatedAt <= endDate.Value))
                    .ToList();
                totalCount = orders.Count;
                orders = orders.Skip((Page - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
                if (orders == null || orders.Count == 0)
                {
                    return (orderDetails, 0);
                }

                foreach (var order in orders)
                {
                    int totalPrice = order.OrderProducts.Sum(op =>
                    {
                        int productPrice = op.Product.Price;
                        if (op.Promotion != null)
                        {
                            if (op.Promotion.ByPercent != null)
                            {
                                productPrice -= productPrice * op.Promotion.ByPercent.Value / 100;
                            }
                            else if (op.Promotion.ByCash != null)
                            {
                                productPrice -= op.Promotion.ByCash.Value;
                            }
                        }
                        return productPrice * op.Amount;
                    });

                    orderDetails.Add(new OrderDetail
                    {
                        Id = order.Id,
                        UserName = order.User.Name,
                        CreatedAt = order.CreatedAt,
                        UpdatedAt = order.UpdatedAt,
                        TotalPrice = totalPrice,
                        Status = order.Status
                    }); ;
                }
            }
            return (orderDetails, totalCount);
        }

        public int GetNumberOfOrders()
        {
            int count = 0;
            using (var context = new MyShopContext())
            {
                count = context.Orders.Count();
            }
            return count;
        }
        public (Order?, int, int) GetNumberOfOrderProductByOrderId(int orderId)
        {
            using (var context = new MyShopContext())
            {
                var order = context.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .ThenInclude(p => p.Brand)
                    .FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    return (null, 0, 0);
                }

                int numberOfOrderProducts = order.OrderProducts.Count;

                int numberOfDistinctBrands = order.OrderProducts
                    .Select(op => op.Product.BrandId)
                    .Distinct()
                    .Count();

                return (order, numberOfOrderProducts, numberOfDistinctBrands);
            }
        }

        public void DeleteOrder(OrderDetail order)
        {
            using (var context = new MyShopContext())
            {
                var orderToDelete = context.Orders.FirstOrDefault(o => o.Id == order.Id);
                if (orderToDelete != null)
                {
                    context.Orders.Remove(orderToDelete);
                    context.SaveChanges();
                }
            }
        }
        public int AddOrder(Order order)
        {
            using (var context = new MyShopContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
                return order.Id;
            }
        }
        public bool UpdateOrderByStatus(int orderId, byte status)
        {
            using (var context = new MyShopContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    return false;
                }
                order.Status = status;
                context.SaveChanges();
                return true;
            }
        }
        public bool UpdateEditOrderDate(int orderId, DateTime date)
        {
            using (var context = new MyShopContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    return false;
                }
                order.UpdatedAt = date;
                context.SaveChanges();
                return true;
            }
        }
        public (List<int>, List<int>, List<string>) GetRevenueAndProfitByDateToDate(DateTime startDate, DateTime endDate)
        {
            var revenues = new List<int>();
            var profits = new List<int>();
            var labels = new List<string>();

            using (var _context = new MyShopContext())
            {
                endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                var orders = _context.Orders
                    .Where(o => o.Status == 2 && o.UpdatedAt >= startDate && o.UpdatedAt <= endDate)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Promotion)
                    .ToList();

                for (var day = startDate; day.Date <= endDate.Date; day = day.AddDays(1))
                {
                    var dailyOrders = orders.Where(o => o.UpdatedAt.Date == day.Date).ToList();

                    var dailyRevenue = dailyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        return op.Amount * (op.Product.Price - discount);
                    }));
                    revenues.Add(dailyRevenue);

                    var dailyProfit = dailyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        var revenue = op.Amount * (op.Product.Price - discount);
                        return revenue - (op.Product.PriceOriginal * op.Amount);
                    }));
                    profits.Add(dailyProfit);

                    labels.Add(day.ToString("dd/MM/yyyy"));
                }
            }
            return (revenues, profits, labels);
        }

        public (List<int>, List<int>, List<string>) GetRevenueAndProfitByYear(int Year)
        {
            var revenues = new List<int>();
            var profits = new List<int>();
            var labels = new List<string>();
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            using (var _context = new MyShopContext())
            {
                var orders = _context.Orders
            .Where(o => o.Status == 2 && o.UpdatedAt.Year == Year)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Promotion)
            .ToList();

                for (int month = 1; month <= 12; month++)
                {
                    var monthlyOrders = orders.Where(o => o.UpdatedAt.Month == month).ToList();

                    var monthlyRevenue = monthlyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        return op.Amount * (op.Product.Price - discount);
                    }));
                    revenues.Add(monthlyRevenue);

                    var monthlyProfit = monthlyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        var revenue = op.Amount * (op.Product.Price - discount);
                        return revenue - (op.Product.PriceOriginal * op.Amount);
                    }));
                    profits.Add(monthlyProfit);

                    labels.Add(cultureInfo.DateTimeFormat.GetMonthName(month));
                }
            }
            return (revenues, profits, labels);
        }
        public (List<int>, List<int>, List<string>) GetRevenueAndProfitByMonth(int Year, int Month)
        {
            var revenues = new List<int>();
            var profits = new List<int>();
            var labels = new List<string>();
            using (var _context = new MyShopContext())
            {
                var orders = _context.Orders
            .Where(o => o.Status == 2 && o.UpdatedAt.Year == Year && o.UpdatedAt.Month == Month)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Promotion)
            .ToList();

                var startDate = new DateTime(Year, Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                for (var day = startDate; day.Date <= endDate.Date; day = day.AddDays(7))
                {
                    var weekEnd = day.AddDays(6) > endDate ? endDate : day.AddDays(6);
                    var weeklyOrders = orders.Where(o => o.UpdatedAt.Date >= day.Date && o.UpdatedAt.Date <= weekEnd.Date).ToList();

                    var weeklyRevenue = weeklyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        return op.Amount * (op.Product.Price - discount);
                    }));
                    revenues.Add(weeklyRevenue);

                    var weeklyProfit = weeklyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        var revenue = op.Amount * (op.Product.Price - discount);
                        return revenue - (op.Product.PriceOriginal * op.Amount);
                    }));
                    profits.Add(weeklyProfit);

                    labels.Add($"{day:dd/MM/yyyy} - {weekEnd:dd/MM/yyyy}");
                }
            }
            return (revenues, profits, labels);
        }
        public (List<int>, List<int>, List<string>) GetRevenueAndProfitByWeek(int Year, int Month, int Week)
        {
            var revenues = new List<int>();
            var profits = new List<int>();
            var labels = new List<string>();
            using (var _context = new MyShopContext())
            {
                var firstDayOfMonth = new DateTime(Year, Month, 1);
                var firstDayOfWeek = firstDayOfMonth.AddDays((Week - 1) * 7);
                var lastDayOfWeek = firstDayOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

                var orders = _context.Orders
                    .Where(o => o.Status == 2 && o.UpdatedAt >= firstDayOfWeek && o.UpdatedAt <= lastDayOfWeek)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Promotion)
                    .ToList();

                for (var day = firstDayOfWeek; day.Date <= lastDayOfWeek.Date; day = day.AddDays(1))
                {
                    var dailyOrders = orders.Where(o => o.UpdatedAt.Date == day.Date).ToList();

                    var dailyRevenue = dailyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        return op.Amount * (op.Product.Price - discount);
                    }));
                    revenues.Add(dailyRevenue);

                    var dailyProfit = dailyOrders.Sum(o => o.OrderProducts.Sum(op =>
                    {
                        var discount = op.Promotion?.ByCash ?? (op.Promotion?.ByPercent ?? 0) * op.Product.Price / 100;
                        var revenue = op.Amount * (op.Product.Price - discount);
                        return revenue - (op.Product.PriceOriginal * op.Amount);
                    }));
                    profits.Add(dailyProfit);

                    labels.Add(day.ToString("dd/MM/yyyy"));
                }
            }
            return (revenues, profits, labels);
        }
        public (List<Tuple<string, List<int>>>, List<string>) GetNumberOfProductsSoldByDateToDate(DateTime startDate, DateTime endDate)
        {
            var hashMap = new List<Tuple<string, List<int>>>();
            var labels = new List<string>();

            using (var _context = new MyShopContext())
            {
                endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                var orders = _context.Orders
                    .Where(o => o.Status == 2 && o.UpdatedAt >= startDate && o.UpdatedAt <= endDate)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .ToList();

                var products = _context.Products.ToList();

                foreach (var product in products)
                {
                    var productSales = new List<int>();
                    for (var day = startDate; day.Date <= endDate; day = day.AddDays(1))
                    {
                        var dailyOrders = orders.Where(o => o.UpdatedAt.Date == day.Date).ToList();
                        var dailySales = dailyOrders.Sum(o => o.OrderProducts.Where(op => op.ProductId == product.Id).Sum(op => op.Amount));
                        productSales.Add(dailySales);
                        labels.Add(day.ToString("dd/MM/yyyy"));
                    }
                    hashMap.Add(new Tuple<string, List<int>>(product.Name, productSales));
                }
            }

            return (hashMap, labels);
        }
        public (List<Tuple<string, List<int>>>, List<string>) GetNumberOfProductsSoldByYear(int Year)
        {
            var hashMap = new List<Tuple<string, List<int>>>();
            var labels = new List<string>();
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            using (var _context = new MyShopContext())
            {
                var orders = _context.Orders
                        .Where(o => o.Status == 2 && o.UpdatedAt.Year == Year)
                        .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                        .ToList();

                var products = _context.Products.ToList();

                foreach (var product in products)
                {
                    var productSales = new List<int>();
                    for (var month = 1; month <= 12; month++)
                    {
                        var monthlyOrders = orders.Where(o => o.UpdatedAt.Month == month).ToList();
                        var monthlySales = monthlyOrders.Sum(o => o.OrderProducts.Where(op => op.ProductId == product.Id).Sum(op => op.Amount));
                        productSales.Add(monthlySales);
                        labels.Add(cultureInfo.DateTimeFormat.GetMonthName(month));
                    }
                    hashMap.Add(new Tuple<string, List<int>>(product.Name, productSales));
                }
            }
            return (hashMap, labels);
        }
        public (List<Tuple<string, List<int>>>, List<string>) GetNumberOfProductsSoldByMonth(int Year, int Month)
        {
            var hashMap = new List<Tuple<string, List<int>>>();
            var labels = new List<string>();
            using (var _context = new MyShopContext())
            {
                var orders = _context.Orders
                        .Where(o => o.Status == 2 && o.UpdatedAt.Year == Year && o.UpdatedAt.Month == Month)
                        .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                        .ToList();
                var products = _context.Products.ToList();

                var startDate = new DateTime(Year, Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                foreach (var product in products)
                {
                    var productSales = new List<int>();
                    for (var day = startDate; day.Date <= endDate.Date; day = day.AddDays(7))
                    {
                        var weekEnd = day.AddDays(6) > endDate ? endDate : day.AddDays(6);
                        var weeklyOrders = orders.Where(o => o.UpdatedAt.Date >= day.Date && o.UpdatedAt.Date <= weekEnd.Date).ToList();
                        var weeklySales = weeklyOrders.Sum(o => o.OrderProducts.Where(op => op.ProductId == product.Id).Sum(op => op.Amount));
                        productSales.Add(weeklySales);

                        labels.Add($"{day:dd/MM/yyyy} - {weekEnd:dd/MM/yyyy}");
                    }
                    hashMap.Add(new Tuple<string, List<int>>(product.Name, productSales));
                }
            }
            return (hashMap, labels);
        }
        public (List<Tuple<string, List<int>>>, List<string>) GetNumberOfProductsSoldByWeek(int Year, int Month, int Week)
        {
            var hashMap = new List<Tuple<string, List<int>>>();
            var labels = new List<string>();
            using (var _context = new MyShopContext())
            {
                var firstDayOfMonth = new DateTime(Year, Month, 1);
                var firstDayOfWeek = firstDayOfMonth.AddDays((Week - 1) * 7);
                var lastDayOfWeek = firstDayOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

                var orders = _context.Orders
                    .Where(o => o.Status == 2 && o.UpdatedAt >= firstDayOfWeek && o.UpdatedAt <= lastDayOfWeek)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .ToList();

                var products = _context.Products.ToList();

                foreach (var product in products)
                {
                    var productSales = new List<int>();
                    for (var day = firstDayOfWeek; day.Date <= lastDayOfWeek.Date; day = day.AddDays(1))
                    {
                        var dailyOrders = orders.Where(o => o.UpdatedAt.Date == day.Date).ToList();
                        var dailySales = dailyOrders.Sum(o => o.OrderProducts.Where(op => op.ProductId == product.Id).Sum(op => op.Amount));
                        productSales.Add(dailySales);
                        labels.Add(day.ToString("dd/MM/yyyy"));
                    }
                    hashMap.Add(new Tuple<string, List<int>>(product.Name, productSales));
                }
            }
            return (hashMap, labels);
        }
        public Dictionary<string, int> GetTop5BestSalersProductByYear(int Year)
        {
            var hashMap = new Dictionary<string, int>();
            using (var _context = new MyShopContext())
            {
                var orders = _context.Orders
            .Where(o => o.Status == 2 && o.UpdatedAt.Year == Year)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ToList();

                var productSales = orders
                    .SelectMany(o => o.OrderProducts)
                    .GroupBy(op => op.Product.Name)
                    .Select(g => new { ProductName = g.Key, Sales = g.Sum(op => op.Amount) })
                    .OrderByDescending(g => g.Sales)
                    .Take(5)
                    .ToList();

                foreach (var productSale in productSales)
                {
                    hashMap.Add(productSale.ProductName, productSale.Sales);
                }
            }
            return hashMap;
        }
        public Dictionary<string, int> GetTop5BestSalersProductByMonth(int Year, int Month)
        {
            var hashMap = new Dictionary<string, int>();
            using (var _context = new MyShopContext())
            {
                var orders = _context.Orders
                        .Where(o => o.Status == 2 && o.UpdatedAt.Year == Year && o.UpdatedAt.Month == Month)
                        .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                        .ToList();

                var productSales = orders
                    .SelectMany(o => o.OrderProducts)
                    .GroupBy(op => op.Product.Name)
                    .Select(g => new { ProductName = g.Key, Sales = g.Sum(op => op.Amount) })
                    .OrderByDescending(g => g.Sales)
                    .Take(5)
                    .ToList();

                foreach (var productSale in productSales)
                {
                    hashMap.Add(productSale.ProductName, productSale.Sales);
                }
            }
            return hashMap;
        }
        public (Dictionary<string, int>, string) GetTop5BestSalersProductByWeek(int Year, int Month, int Week)
        {
            var hashMap = new Dictionary<string, int>();
            string date = "";
            using (var _context = new MyShopContext())
            {
                var firstDayOfMonth = new DateTime(Year, Month, 1);
                var firstDayOfWeek = firstDayOfMonth.AddDays((Week - 1) * 7);
                var lastDayOfWeek = firstDayOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

                var orders = _context.Orders
                    .Where(o => o.Status == 2 && o.UpdatedAt >= firstDayOfWeek && o.UpdatedAt <= lastDayOfWeek)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .ToList();

                var productSales = orders
                    .SelectMany(o => o.OrderProducts)
                    .GroupBy(op => op.Product.Name)
                    .Select(g => new { ProductName = g.Key, Sales = g.Sum(op => op.Amount) })
                    .OrderByDescending(g => g.Sales)
                    .Take(5)
                    .ToList();

                foreach (var productSale in productSales)
                {
                    hashMap.Add(productSale.ProductName, productSale.Sales);
                }

                date = $"{firstDayOfWeek:dd/MM/yyyy} - {lastDayOfWeek:dd/MM/yyyy}";
            }
            return (hashMap, date);
        }

        static List<DateTime> GetWeekDays(DateTime givenDate)
        {

            // Xác định ngày đầu tuần (Thứ Hai)
            int delta = DayOfWeek.Monday - givenDate.DayOfWeek;
            DateTime monday = givenDate.AddDays(delta);

            // Trường hợp ngày đã cho là Chủ Nhật
            if (givenDate.DayOfWeek == DayOfWeek.Sunday)
            {
                monday = givenDate.AddDays(-6);
            }

            // Tạo danh sách các ngày trong tuần
            List<DateTime> weekDays = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                weekDays.Add(monday.AddDays(i).Date);
            }

            return weekDays;
        }

        public int getNumOfNewPurchaseInWeek(DateTime dateTime)
        {
            using (var context = new MyShopContext())
            {
                var orders = context.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .ToList();

                var weekDays = GetWeekDays(dateTime);

                var weeklyOrders = orders
                    .Where(o => o.Status == 1)
                    .Where(o => weekDays.Contains(o.UpdatedAt.Date))
                    .ToList();

                return weeklyOrders.Count;
            }

        }

        public int getNumOfNewPurchaseInMonth(DateTime dateTime)
        {
            using (var context = new MyShopContext())
            {
                var orders = context.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .ToList();

                var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var monthlyOrders = orders
                    .Where(o => o.Status == 1)
                    .Where(o => o.UpdatedAt.Date >= firstDayOfMonth.Date && o.UpdatedAt.Date <= lastDayOfMonth.Date)
                    .ToList();

                return monthlyOrders.Count;
            }
        }

    }

}

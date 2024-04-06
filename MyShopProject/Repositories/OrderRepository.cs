using Microsoft.EntityFrameworkCore;
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

                    labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));
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
                var lastDayOfWeek = firstDayOfWeek.AddDays(6);

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
    }
}

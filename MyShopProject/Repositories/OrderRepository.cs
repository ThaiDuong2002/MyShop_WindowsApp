using Microsoft.EntityFrameworkCore;
using MyShopProject.Model;
using System.Collections.ObjectModel;

namespace MyShopProject.Repositories
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

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
                    decimal totalPrice = order.OrderProducts.Sum(op =>
                    {
                        decimal productPrice = op.Product.Price;
                        if (op.Promotion != null)
                        {
                            if (op.Promotion.ByPercent != null)
                            {
                                productPrice -= productPrice * (decimal)op.Promotion.ByPercent / 100;
                            }
                            else if (op.Promotion.ByCash != null)
                            {
                                productPrice -= (decimal)op.Promotion.ByCash;
                            }
                        }
                        return productPrice * op.Amount;
                    });

                    orderDetails.Add(new OrderDetail
                    {
                        Id = order.Id,
                        UserName = order.User.Name,
                        CreatedAt = order.CreatedAt,
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
    }
}

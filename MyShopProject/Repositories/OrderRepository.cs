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

        public ObservableCollection<Order> GetOrderBtId(int orderId)
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            using (var context = new MyShopContext())
            {
                orders = new ObservableCollection<Order>(context.Orders
                    .Include(o => o.User)
                    .Where(o => o.Id == orderId).ToList());
            }
            return orders;
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
    }
}

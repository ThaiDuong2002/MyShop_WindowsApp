using Microsoft.EntityFrameworkCore;
using MyShopProject.Model;
using System.Collections.ObjectModel;

namespace MyShopProject.Repositories
{
    public class OrderProductRepository
    {
        public ObservableCollection<OrderProduct> GetOrderProductsByOrderId(int OrderId)
        {
            ObservableCollection<OrderProduct> orderProducts = new ObservableCollection<OrderProduct>();
            using (var context = new MyShopContext())
            {
                orderProducts = new ObservableCollection<OrderProduct>(context.OrderProducts
                    .Include(op => op.Product)
                    .Include(op => op.Promotion)
                    .Where(op => op.OrderId == OrderId)
                    .ToList());
            }
            return orderProducts;
        }
        public int GetTotalPriceByOrderId(int OrderId)
        {
            int totalPrice = 0;
            using (var context = new MyShopContext())
            {
                var orderProducts = context.OrderProducts
                    .Include(op => op.Product)
                    .Include(op => op.Promotion)
                    .Where(op => op.OrderId == OrderId)
                    .ToList();

                foreach (var orderProduct in orderProducts)
                {
                    int price = orderProduct.Product.Price;
                    if (orderProduct.Promotion != null)
                    {
                        if (orderProduct.Promotion.ByPercent != null)
                        {
                            price -= price * orderProduct.Promotion.ByPercent.Value / 100;
                        }
                        else if (orderProduct.Promotion.ByCash != null)
                        {
                            price -= orderProduct.Promotion.ByCash.Value;
                        }
                    }
                    totalPrice += price * orderProduct.Amount;
                }
            }
            return totalPrice;
        }
    }
}

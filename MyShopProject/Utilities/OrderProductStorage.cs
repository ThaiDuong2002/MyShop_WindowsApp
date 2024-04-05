using MyShopProject.Model;
using System.Collections.ObjectModel;

namespace MyShopProject.Utilities
{
    public class OrderProductStore
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public Promotion? Promotion { get; set; }
    }
    public class OrderProductStorage
    {
        private static ObservableCollection<OrderProductStore> _orderProductStores;
        public static ObservableCollection<OrderProductStore> OrderProductStores
        {
            get
            {
                if (_orderProductStores == null)
                {
                    _orderProductStores = new ObservableCollection<OrderProductStore>();
                }
                return _orderProductStores;
            }
            set
            {
                _orderProductStores = value;
            }
        }
        public static bool AddOrderProductStore(OrderProductStore orderProductStore)
        {
            try
            {
                OrderProductStorage.OrderProductStores.Add(orderProductStore);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool RemoveOrderProductStore(OrderProductStore orderProductStore)
        {
            try
            {
                OrderProductStorage.OrderProductStores.Remove(orderProductStore);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool EditOrderProductByProductId(int productId, OrderProductStore orderProductStore)
        {
            try
            {
                var orderProduct = OrderProductStorage.OrderProductStores.FirstOrDefault(x => x.ProductId == productId);
                if (orderProduct != null)
                {
                    orderProduct.ProductId = orderProductStore.ProductId;
                    orderProduct.ProductName = orderProductStore.ProductName;
                    orderProduct.Price = orderProductStore.Price;
                    orderProduct.Quantity = orderProductStore.Quantity;
                    orderProduct.Promotion = orderProductStore.Promotion;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ClearOrderProductStore()
        {
            try
            {
                OrderProductStorage.OrderProductStores.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int TotalPrice()
        {
            int totalPrice = 0;
            foreach (var orderProduct in OrderProductStores)
            {
                int price = orderProduct.Price;
                if (orderProduct.Promotion != null)
                {
                    if (orderProduct.Promotion.ByCash != null)
                    {
                        price -= orderProduct.Promotion.ByCash.Value;
                    }
                    else if (orderProduct.Promotion.ByPercent != null)
                    {
                        price -= price * orderProduct.Promotion.ByPercent.Value / 100;
                    }
                }
                totalPrice += price * orderProduct.Quantity;
            }
            return totalPrice;
        }
    }
}

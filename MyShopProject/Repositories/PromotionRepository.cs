using MyShopProject.Model;
using System.Collections.ObjectModel;

namespace MyShopProject.Repositories
{
    public class PromotionRepository
    {
        public (int, ObservableCollection<Promotion>) GetPromotions(int Page, int ItemsPerPage, string? search)
        {
            ObservableCollection<Promotion> promotions = new ObservableCollection<Promotion>();
            using (var context = new MyShopContext())
            {
                promotions = new ObservableCollection<Promotion>(context.Promotions.ToList());
                if (!string.IsNullOrEmpty(search))
                {
                    promotions = new ObservableCollection<Promotion>(promotions
                        .Where(p => p.Name.Contains(search)).ToList());
                }
                var totalPromotions = promotions.Count();

                promotions = new ObservableCollection<Promotion>(promotions
                                       .Skip((Page - 1) * ItemsPerPage)
                                       .Take(ItemsPerPage)
                                       .ToList());


                return (totalPromotions, promotions);
            }
        }

        public ObservableCollection<Promotion> GetAllPromotions()
        {
            ObservableCollection<Promotion> promotions = new ObservableCollection<Promotion>();
            using (var context = new MyShopContext())
            {
                promotions = new ObservableCollection<Promotion>(context.Promotions.ToList());
            }
            return promotions;
        }

        public ObservableCollection<Promotion> GetPromotionById(int promotionId)
        {
            ObservableCollection<Promotion> promotions = new ObservableCollection<Promotion>();
            using (var context = new MyShopContext())
            {
                promotions = new ObservableCollection<Promotion>(context.Promotions
                                       .Where(p => p.Id == promotionId).ToList());
            }
            return promotions;
        }

        public void DeletePromotion(int promotionId)
        {
            using (var context = new MyShopContext())
            {
                var promotion = context.Promotions.Find(promotionId);
                context.Promotions.Remove(promotion!);
                context.SaveChanges();
            }
        }

        public void AddPromotion(Promotion promotion)
        {
            using (var context = new MyShopContext())
            {
                context.Promotions.Add(promotion);
                context.SaveChanges();
            }
        }
    }
}

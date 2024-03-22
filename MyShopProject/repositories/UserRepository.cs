using MyShopProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.repositories
{
    public class UserRepository
    {
        public ObservableCollection<User> GetAllUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            using (var context = new MyShopContext())
            {
                users = new ObservableCollection<User>(context.Users.ToList());
            }
            return users;
        }
        public int getNumOfUsers()
        {
            int count = 0;
            using (var context = new MyShopContext())
            {
                count = context.Users.Count();
            }
            return count;
        }
        public ObservableCollection<User> GetUsersByPagination(int page, int itemPerPage)
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            using (var context = new MyShopContext())
            {
                users = new ObservableCollection<User>(context.Users.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList());
            }
            return users;
        }
        public ObservableCollection<User> GetUsersByName(string name)
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            using (var context = new MyShopContext())
            {
                users = new ObservableCollection<User>(context.Users.Where(u => u.Name.Contains(name)).ToList());
            }
            return users;
        }
        public bool deleteUser(User user)
        {
            using (var context = new MyShopContext())
            {
                var userToDelete = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}

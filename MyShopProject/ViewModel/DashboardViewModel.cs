using MaterialDesignThemes.Wpf;
using MyShopProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.ViewModel
{
    class DashboardViewModel:BaseViewModel
    {

        public DashboardViewModel() { 

            Global.SaveScreen("Dashboard");
        }
    }
}

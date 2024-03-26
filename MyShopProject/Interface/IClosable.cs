using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.Interface
{
    public interface IClosable
    {
        bool? DialogResult { get; set; }
        void Close();
    }
}

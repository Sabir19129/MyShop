using MyShop.Common;
using MyShop.Models;
using MyShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    public class ProductListViewModel : TabViewModel
    {
        public ProductListViewModel()
        {
            TabHeading = "Product List";
        }
    }
}
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModels
{
    public class SalesListViewModel : TabViewModel
    {
        public SalesListViewModel()
        {
            new ObservableCollection<Sales>(Sales.FetchSales());
            TabHeading = "Sales List";
        }
    }
}

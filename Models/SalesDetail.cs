using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyShop.Models
{
    public class SaleDetail : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        public event EventHandler QuantityChanged;
        private int _Id;
        private int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        private Product _Product;
        public Product Product
        {
            get { return _Product; }
            set
            {
                if (_Product != value)
                {
                    _Product = value;
                    OnPropertyChanged(nameof(Product));
                    if (Product != null)
                    {
                        Price = Product.Price;
                    }

                }
            }
        }

        private int _TotalSale;
        public int TotalSale
        {
            get { return _TotalSale; }
            set
            {
                if (_TotalSale != value)
                {
                    _TotalSale = value;
                    OnPropertyChanged(nameof(TotalSale));
                    //CalculateTotalSale();
                }
            }
        }

        private int _Price; // Changed to decimal as price typically has decimal values
        public int Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged(nameof(Price));
                   
                }
            }
        }

        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                if (_TotalPrice != value)
                {
                    _TotalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

      

    }
}

using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    internal class PurchaseDetail: BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";

        public event EventHandler QuantityChanged;


        private int _Id;
        public int Id
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
      

        private int _Quantity;
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    CalculateTotalPrice();
                    QuantityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private int _Price;
        public int Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged(nameof(Price));
                    CalculateTotalPrice();
                }
            }
        }
         
        
        private int _TotalPrice;

        public int TotalPrice

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
        public void CalculateTotalPrice()
        {
            TotalPrice = Quantity * Price;
        }
       
        public override string ToString()
        {
            return Product.Name; // This will show the name when displaying a User object
        }

    }
}

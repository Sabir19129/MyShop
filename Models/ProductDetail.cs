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
    public class ProductDetail: BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";

        public event EventHandler QuantityChanged;
       

        private string _Madein;
        public string Madein
        {
            get { return _Madein; }
            set
            {
                if (_Madein != value)
                {
                    _Madein = value;
                    OnPropertyChanged(nameof(Madein)); // Fixed here
                }
            }
        }
        private int _Stock;
        public int Stock
        {
            get => _Stock;
            // get { return _Stock; } , both can be used Lambda and the return method
            set
            {
                if (_Stock != value)
                {
                    _Stock = value;
                    OnPropertyChanged(nameof(Stock)); // Fixed here
                }
            }
        }


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
                    //CalculateTotalPrice();
                }
            }
        }                 
            
    }

}

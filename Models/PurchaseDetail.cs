using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models
{
    internal class PurchaseDetail: BindableBase
    {

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
                    CalculateTotalAmount();
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
                    CalculateTotalAmount();
                }
            }
        }
         
        
        private int _TotalAmount;

        public int TotalAmount

        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                    
                }
            }
        }
        public void CalculateTotalAmount()
        {
            TotalAmount = Quantity * Price;
        }
    }
}

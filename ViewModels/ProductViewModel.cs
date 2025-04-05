
using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    internal class ProductViewModel : TabViewModel
    {
        #region Property
        #region Const
        public ProductViewModel()
        {
            Product = new Product();
            Products = new List<Product>();
           // Product p1 = new Product();
           // Product p2 = new Product();
            //p1.Name = "iPhone";
            //p2.Name = "Samsung";
        }
        #endregion
        // Property to hold the list of products

        // Property to hold the current Product object
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
                }
            }
        }
        // 1. Add this method to update stock quantities
        private List<Product> _Products;
        // 2. In the Products property setter, add a call to UpdateStockQuantities
        public List<Product> Products
        {
            get { return _Products; }
            set
            {
                if (_Products != value)
                {
                    _Products = value;
                    OnPropertyChanged(nameof(Products)); 
                }
            }
        }

        // 3. Modify ExecuteFetchCommand to update stock quantities after fetching products
       

        #endregion

        #region Command
        // SaveCommand to insert a new product
        RelayCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand(p => ExecuteSaveCommand());
                }
                return _SaveCommand;
            }
        }

        private void ExecuteSaveCommand()
        {
            Product.Insert(); // Insert the product
             Products =  Product.FetchProducts();
           // Product = new Product(); // Reset the Product object
        }

        RelayCommand _UpdateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                {
                    _UpdateCommand = new RelayCommand(p => ExecuteUpdateCommand());
                }
                return _UpdateCommand;
            }
        }

        private void ExecuteUpdateCommand()
        {
            Product.Update(); // update the product            
            Product = new Product(); // Reset the Product object
            Products = Product.FetchProducts();
        }
        RelayCommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new RelayCommand(p => ExecuteDeleteCommand());
                }
                return _DeleteCommand;
            }
        }

        private void ExecuteDeleteCommand()
        {
            Product.Delete(Product.Id);
            Product = new Product();
            Product.FetchProducts();

            //Products.Remove(Product);

            // Products = Product.FetchProducts();
        }





        // FetchCommand to get products based on search criteria
        RelayCommand _FetchCommand;
       

        public ICommand FetchCommand
        {
            get
            {
                if (_FetchCommand == null)
                {
                    _FetchCommand = new RelayCommand(p => ExecuteFetchCommand());
                }
                return _FetchCommand;
            }
        }
        private void ExecuteFetchCommand()
        {
            Products = Product.FetchProducts(); // Fetch products 
            Product = new Product();
        }
        // Fetch data from the Product model and update the Products list
       
#endregion
    }
}


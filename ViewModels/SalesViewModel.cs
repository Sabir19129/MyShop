
using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MyShop.ViewModels
{

    internal class SaleViewModel : TabViewModel

    {
        #region Property    
        public SaleViewModel()
        {
            Sale = new Sale();
            Sales = new List<Sale>();
            Products = new Product().FetchProducts();

            PaymentMethods = new ObservableCollection<string>();

        }
        private List<Sale> _sales;
        public List<Sale> Sales
        {
            get => _sales;
            set
            {
                if (_sales != value)
                {
                    _sales = value;
                    OnPropertyChanged(nameof(Sales));
                
                }
            }
        }



        private ObservableCollection<string> _paymentMethods;
        public ObservableCollection<string> PaymentMethods
        {
            get { return _paymentMethods; }
            set
            {
                if (_paymentMethods != value)
                {
                    _paymentMethods = value;
                    OnPropertyChanged(nameof(PaymentMethods)); // Correct property name
                }
            }
        }


        private string _SelectedPaymentMethod;
        public string SelectedPaymentMethod
        {
            get { return _SelectedPaymentMethod; }
            set
            {
                if (_SelectedPaymentMethod != value)
                {
                    _SelectedPaymentMethod = value;

                    OnPropertyChanged(nameof(SelectedPaymentMethod));

                    //Sales.PaymentMethod = SelectedPaymentMethod;
                }
            }
        }

        private int _PurchasePrice;
        public int PurchasePrice
        {
            get { return _PurchasePrice; }
            set
            {
                if (_PurchasePrice != value)
                {
                    _PurchasePrice = value;
                    OnPropertyChanged(nameof(PurchasePrice));
                }
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));

                    // Set IsSaveMode to True when a product is selected
                    //IsSaveMode = true;
                    // IsEditMode = false;  // You might also want to set IsEditMode to false if you're not in edit mode

                   // Sales.Product.Id = SelectedProduct.Id;
                }
            }
        }

        private Sale _Sale;
        public Sale Sale
        {
            get { return _Sale; }
            set
            {
                if (_Sale != value)
                {
                    _Sale = value;
                    OnPropertyChanged(nameof(Sale));
                }
            }
        }

        private ObservableCollection<Sale> _SaleList;
        public ObservableCollection<Sale> SaleList
        {
            get { return _SaleList; }
            set
            {
                if (_SaleList != value)
                {
                    _SaleList = value;
                    OnPropertyChanged(nameof(SaleList));
                }
            }
        }


        private List<Product> _Products;
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

      

        #endregion

        #region Command

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
            Sale.Insert(); // Insert the Sales
            Sale = new Sale(); // Reset the Sales object 
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
           // Sales.Update(); // Update the Sales
            Sale = new Sale(); // Reset the Sales object 
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
            SaleList.Remove(Sale); // Remove the selected Sale 
        }

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
            var fetchedSales = Sale.FetchSales(); // Fetch sales from the database
            SaleList.Clear(); // Clear the existing list
            foreach (var sale in fetchedSales)
            {
                SaleList.Add((Sale)sale); // Add each fetched sale to the SalesList
            }
        }


        #endregion
    }
}

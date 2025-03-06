
using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MyShop.ViewModels
{

    internal class SalesViewModel : BindableBase 

    {
        #region Property    
        public SalesViewModel()
        {
            Sales = new Sales();
            SalesList = new ObservableCollection();
            Products = new Product().FetchProducts();

            PaymentMethods = new ObservableCollection<string>
            {
            "Cash",
            "Card",
            "Online",
            "UPI",
            "Cheque"
            };

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

                    Sales.PaymentMethod = SelectedPaymentMethod;
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

                    Sales.Product.Id = SelectedProduct.Id;
                }
            }
        }

        private Sales _Sales;
        public Sales Sales
        {
            get { return _Sales; }
            set
            {
                if (_Sales != value)
                {
                    _Sales = value;
                    OnPropertyChanged(nameof(Sales));
                }
            }
        }

        private ObservableCollection<Sales> _SalesList;
        public ObservableCollection<Sales> SalesList
        {
            get { return _SalesList; }
            set
            {
                if (_SalesList != value)
                {
                    _SalesList = value;
                    OnPropertyChanged(nameof(SalesList));
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
            Sales.Insert(); // Insert the Sales
            Sales = new Sales(); // Reset the Sales object 
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
            Sales.Update(); // Update the Sales
            Sales = new Sales(); // Reset the Sales object 
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
            SalesList.Remove(Sales); // Remove the selected Sale 
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
            var fetchedSales = Sales.FetchSales(); // Fetch sales from the database
            SalesList.Clear(); // Clear the existing list
            foreach (var sale in fetchedSales)
            {
                SalesList.Add((Sales)sale); // Add each fetched sale to the SalesList
            }
        }


        #endregion
    }
}

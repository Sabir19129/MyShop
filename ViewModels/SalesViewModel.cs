
using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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
            SaleDetail = new SaleDetail();
            Payments = Payment.FetchPayments();
            Suppliers = Supplier.FetchSuppliers();
            Users = User.FetchUsers();

        }
        private void SaleDetails_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Sale.TotalPrice = Sale.SaleDetails.Sum(x => x.TotalPrice);
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

        private SaleDetail _SaleDetail = new SaleDetail();
        public SaleDetail SaleDetail
        {
            get { return _SaleDetail; }
            set
            {
                if (_SaleDetail != value)
                {
                    _SaleDetail = value;
                    OnPropertyChanged(nameof(SaleDetail));
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
        private List<Supplier> _Suppliers;
        public List<Supplier> Suppliers
        {
            get { return _Suppliers; }
            set
            {
                if (_Suppliers != value)
                {
                    _Suppliers = value;
                    OnPropertyChanged(nameof(Suppliers));
                }
            }
        }

        private List<User> _Users;
        public List<User> Users
        {
            get { return _Users; }
            set
            {
                if (_Users != value)
                {
                    _Users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }


        //private string _SelectedPaymentMethod;
        //public string SelectedPaymentMethod
        //{
        //    get { return _SelectedPaymentMethod; }
        //    set
        //    {
        //        if (_SelectedPaymentMethod != value)
        //        {
        //            _SelectedPaymentMethod = value;

        //            OnPropertyChanged(nameof(SelectedPaymentMethod));

        //            //Sales.PaymentMethod = SelectedPaymentMethod;
        //        }
        //    }
        //}

        private int _SalePrice;
        public int SalePrice
        {
            get { return _SalePrice; }
            set
            {
                if (_SalePrice != value)
                {
                    _SalePrice = value;
                    OnPropertyChanged(nameof(SalePrice));
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
        private List<Payment> _Payments;
        public List<Payment> Payments
        {
            get { return _Payments; }
            set
            {
                if (_Payments != value)
                {
                    _Payments = value;
                    OnPropertyChanged(nameof(Payments));
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
        private RelayCommand _AddDetailCommand;
        public ICommand AddDetailCommand
        {
            get
            {
                if (_AddDetailCommand == null)
                {
                    _AddDetailCommand = new RelayCommand(p => ExecuteAddDetailCommand());
                }
                return _AddDetailCommand;
            }
        }
        private void ExecuteAddDetailCommand()
        {
            if (SaleDetail.Product == null || SaleDetail.Quantity <= 0 || SaleDetail.Price <= 0)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            Sale.SaleDetails.Add(new SaleDetail
            {
                Product = SaleDetail.Product,
                Quantity = SaleDetail.Quantity,
                Price = SaleDetail.Price,
                TotalPrice = SaleDetail.TotalPrice,

            });
            OnPropertyChanged(nameof(SaleDetail)); // Notify UI

            SaleDetail = new SaleDetail(); // Reset for new entry
                                                   // Reset for the next entry
        }



        #endregion
    }
}


using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    internal class PurchaseViewModel : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Const
        public PurchaseViewModel()
        {
            Purchase = new Purchase();
            Purchases = new List<Purchase>();
            Products = new Product().FetchProducts();
            IsEditMode = false;
            IsSaveMode = false;
            Suppliers = Supplier.FetchSuppliers();
            Payments = Payment.FetchPayments();

        }
        #endregion
        #region Properties

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
                }
            }
        }


        private Purchase _purchase;
        public Purchase Purchase
        {
            get { return _purchase; }
            set
            {
                if (_purchase != value)
                {
                    _purchase = value;
                    OnPropertyChanged(nameof(Purchase));
                    IsEditMode = true;
                    IsSaveMode = false;

                    Purchase.PurchaseDetails.CollectionChanged += PurchaseDetails_CollectionChanged;
                }
            }
        }

        private void PurchaseDetails_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Purchase.TotalAmount = Purchase.PurchaseDetails.Sum(x=> x.TotalAmount);
        }

        //private List<Purchase> _purchaselist;
        //public List<Purchase> purchaselist
        //{
        //    get { return _purchaselist; }
        //    set
        //    {
        //       if (_purchaselist != value)
        //        {
        //            _purchaselist = value;
        //            OnPropertyChanged(nameof(purchaselist));
        //        }
        //    }
        //}

        private List<Purchase> _purchases;
        public List<Purchase> Purchases
        {
            get { return _purchases; }
            set
            {
                if (_purchases != value)
                {
                    _purchases = value;
                    OnPropertyChanged(nameof(Purchases));
                }
            }
        }

        private List<Product> _products;
        public List<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

         private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(nameof(IsEditMode));  // Notify UI to refresh visibility
                }
            }
        }
     

        private bool _IsSaveMode;
        public bool IsSaveMode
        {
            get { return _IsSaveMode; }
            set
            {
                if (_IsSaveMode != value)
                {
                    _IsSaveMode = value;
                    OnPropertyChanged(nameof(IsSaveMode));  // Notify UI to refresh visibility
                }
            }
        }

        // Add a method to set IsEditMode depending on context (e.g., when selecting a product or editing one)
        public void StartNewPurchase()
        {
            IsEditMode = false; // For new purchases (Save button visible)
        }

        public void StartEditingPurchase()
        {
            IsEditMode = true; // For editing purchases (Update button visible)
        }
        private PurchaseDetail _PurchaseDetail = new PurchaseDetail();
        public PurchaseDetail PurchaseDetail
        {
            get { return _PurchaseDetail; }
            set
            {
                if (_PurchaseDetail != value)
                {
                    _PurchaseDetail = value;
                    OnPropertyChanged(nameof(PurchaseDetail)); 
                }
            }
        }

        private void PurchaseDetail_ProductChanged(object? sender, EventArgs e)
        {
            
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
        #region ICmd Members
        // SaveCommand to get Purchases
        private RelayCommand _SaveCommand;
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
            // Check if required fields are filled
            if (PurchaseDetail.Product == null )
            {
                MessageBox.Show("Please select Product.");
                return;
            }
            Purchase.Insert();
          
            Purchase = new Purchase();  // Reset after save
            Purchases = Purchase.FetchPurchases();

            // After saving, reset IsEditMode for creating mode
            IsEditMode = false;  // Hide the Save button after saving
        }  
        private RelayCommand _UpdateCommand;
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
            // Check if required fields are filled
            if (PurchaseDetail.Product == null || PurchaseDetail.Product.Id == 0 || PurchaseDetail.Quantity <= 0 || PurchaseDetail.Price <= 0)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            // Insert the updated purchase
            Purchase.Update();
            Purchase = new Purchase();  // Reset after update
            Purchases = Purchase.FetchPurchases();

            // After updating, set IsEditMode to true (show Update button)
            IsEditMode = false;
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
            if (PurchaseDetail.Product == null || PurchaseDetail.Quantity <= 0 || PurchaseDetail.Price <= 0)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            Purchase.PurchaseDetails.Add(new PurchaseDetail
            {
                Product = PurchaseDetail.Product,
                Quantity = PurchaseDetail.Quantity,
                Price = PurchaseDetail.Price,
                TotalAmount = PurchaseDetail.TotalAmount,
                
            });
            OnPropertyChanged(nameof(PurchaseDetail)); // Notify UI
           
            PurchaseDetail = new PurchaseDetail(); // Reset for new entry
                                                   // Reset for the next entry
        }

         

        // DeleteCommand to delete a Purchase
        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(p => ExecuteDeleteCommand(p));
                }
                return _deleteCommand;
            }
        }

        private void ExecuteDeleteCommand(object p)
        {
            if (PurchaseDetail.Product == null || PurchaseDetail.Product.Id == 0 || PurchaseDetail.Quantity <= 0 || PurchaseDetail.Price <= 0)
            {
                MessageBox.Show("Please select a Purchase to delete.");
                return;
            }

            var purchase = p as Purchase;
            if (purchase == null) return;

            var result = MessageBox.Show($"This will delete {PurchaseDetail.Product.Id} permanently. Do you want to proceed?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Purchase.Delete(PurchaseDetail.Product.Id); // Delete the Purchase
                Purchases = Purchase.FetchPurchases(); // Refresh Purchases
                Purchase = new Purchase(); // Reset Purchase
            }
        }

        // FetchCommand to get Purchases
        private RelayCommand _FetchCommand;
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
            Purchases = Purchase.FetchPurchases(); // Fetch the list of purchases
        }
        private RelayCommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new RelayCommand(p => ExecuteCancelCommand());
                }
                return _CancelCommand;
            }
        }

        private void ExecuteCancelCommand()
        {
            var result = MessageBox.Show("This will cancel all your entries of Purchase. Do you want to continue?",
                                         "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Purchase = new Purchase(); // This will now reflect in the UI
            }
            else
            {  
            }
        }
        private RelayCommand _PaymentCommand;
        public ICommand PaymentCommand
        {
            get
            {
                if (_PaymentCommand == null)
                {
                    _PaymentCommand = new RelayCommand(p => ExecutePaymentCommand());
                }
                return _PaymentCommand;
            }
        }

        private void ExecutePaymentCommand()
        {
            var result = MessageBox.Show("Do you want to confirm your Purchases?",
                                         "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Please pay the bill");
                Purchase = new Purchase(); // This will now reflect in the UI
            }
            else
            {
            }
        }

        #endregion
    }
}

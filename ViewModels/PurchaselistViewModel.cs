using MyShop.Common;
using MyShop.Models;
using MyShop.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    public class PurchaseListViewModel : TabViewModel
    {

        public PurchaseListViewModel()
        {
            Purchases = Purchase.FetchPurchases();
            TabHeading = "Purchase List"; 
            SelectedPurchase = new Purchase();
        }
        private Purchase _SelectedPurchase;
        public Purchase SelectedPurchase
        {
            get{ return _SelectedPurchase; }
            set
            {
                if (_SelectedPurchase != value)
                {
                    _SelectedPurchase = value;
                    OnPropertyChanged(nameof(SelectedPurchase));
                }
            }
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
        private List<int> DeletedIds { get; set; } = new List<int>();

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

            SelectedPurchase.PurchaseDetails.Add(new PurchaseDetail
            {
                Product = PurchaseDetail.Product,
                Quantity = PurchaseDetail.Quantity,
                Price = PurchaseDetail.Price,
                TotalPrice = PurchaseDetail.TotalPrice,

            });
            OnPropertyChanged(nameof(PurchaseDetail)); // Notify UI

            PurchaseDetail = new PurchaseDetail(); // Reset for new entry
                                                   // Reset for the next entry
        }
        private RelayCommand _DeletePurchaseDetailCommand;
        public ICommand DeletePurchaseDetailCommand
        {
            get
            {
                if (_DeletePurchaseDetailCommand == null)
                {
                    _DeletePurchaseDetailCommand = new RelayCommand(p => ExecuteDeletePurchaseDetailCommand(p));
                }
                return _DeletePurchaseDetailCommand;
            }
        }

        private void ExecuteDeletePurchaseDetailCommand(object p)
        {
            // if (PurchaseDetail.Product == null || PurchaseDetail.Product.Id == 0 || PurchaseDetail.TotalPrice ==0 || PurchaseDetail.Quantity <= 0 
            //   || PurchaseDetail.Price <= 0)
            //{
            //    MessageBox.Show("Please select a Purchase to delete.");
            //    return;
            //}

            var purchaseDetail = p as PurchaseDetail;
            if (purchaseDetail == null) return;

            var result = MessageBox.Show($"This will delete {purchaseDetail.Product.Id} permanently. Do you want to proceed?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DeletedIds.Add(purchaseDetail.Id);

                SelectedPurchase.PurchaseDetails.Remove(purchaseDetail);
                //Purchases = Purchase.FetchPurchases(); // Refresh Purchases
                //Purchase = new Purchase(); // Reset Purchase
            }
        }
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
            Purchases = Purchase.FetchPurchases();
            //Purchases = Purchase.FetchPurchaseDetails();// Fetch the list of purchases
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
            //PurchaseDetail.Update();
            Purchases = Purchase.FetchPurchases();
            //Purchases = Purchase.FetchPurchaseDetails();// Fetch the list of purchases
        }


    }
}

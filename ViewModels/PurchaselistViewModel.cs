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
                TotalPrice = PurchaseDetail.TotalPrice,

            });
            OnPropertyChanged(nameof(PurchaseDetail)); // Notify UI

            PurchaseDetail = new PurchaseDetail(); // Reset for new entry
                                                   // Reset for the next entry
        }


    }
}

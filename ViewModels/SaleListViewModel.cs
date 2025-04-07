using MyShop.Common;
using MyShop.Models;
using MyShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    public class SaleListViewModel : TabViewModel
    {
        public SaleListViewModel()
        {
            Sales = Sale.FetchSales();
            TabHeading = "Sale List";
            SelectedSale = new Sale();
        }
        #region Property   

        private Sale _SelectedSale;
        public Sale SelectedSale
        {
            get { return _SelectedSale; }
            set
            {
                if (_SelectedSale != value)
                {
                    _SelectedSale = value;
                    OnPropertyChanged(nameof(SelectedSale));
                }
            }
        }
        private List<Sale> _Sales;
        public List<Sale> Sales
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
        #endregion
        #region Command
        private RelayCommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                {
                    _AddCommand = new RelayCommand(p => ExecuteAddCommand());
                }
                return _AddCommand;
            }
        }
        private void ExecuteAddCommand()
        {
            SaleView SaleView = new SaleView();
            SaleViewModel SaleViewModel = new SaleViewModel();
            SaleView.DataContext = SaleViewModel;
            SaleView.ShowDialog();
            // Reset for the next entry
        }
        private RelayCommand _DeleteSaleCommand;
        public ICommand DeleteSaleCommand
        {
            get
            {
                if (_DeleteSaleCommand == null)
                {
                    _DeleteSaleCommand = new RelayCommand(p => ExecuteDeleteSaleCommand(p));
                }
                return _DeleteSaleCommand;
            }
        }

        private void ExecuteDeleteSaleCommand(object p)
        {
            SelectedSale.Delete(SelectedSale.Id);
            Sales = Sale.FetchSales();

        }
        private RelayCommand _SaleFetchCommand;
        public ICommand SaleFetchCommand
        {
            get
            {
                if (_SaleFetchCommand == null)
                {
                    _SaleFetchCommand = new RelayCommand(p => ExecuteSaleFetchCommand());
                }
                return _SaleFetchCommand;
            }
        }

        private void ExecuteSaleFetchCommand()
        {
            Sales = Sale.FetchSales();
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
            SaleView SaleView = new SaleView();
            SaleViewModel SaleViewModel = new SaleViewModel();
            SaleView.DataContext = SaleViewModel;
            SaleViewModel.Sale = SelectedSale;
            SaleView.ShowDialog();
            Sales = Sale.FetchSales();
        }


    }
}



#endregion

//using Microsoft.Reporting.WinForms;
using MyShop.Common;
using MyShop.Models;
using MyShop.ViewModels;
using MyShop.Views;
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
            PurchaseView PurchaseView = new PurchaseView();
            PurchaseViewModel PurchaseViewModel = new PurchaseViewModel();
            PurchaseView.DataContext = PurchaseViewModel;
            PurchaseView.ShowDialog();
            // Reset for the next entry
        }
        private RelayCommand _DeletePurchaseCommand;
        public ICommand DeletePurchaseCommand
        {
            get
            {
                if (_DeletePurchaseCommand == null)
                {
                    _DeletePurchaseCommand = new RelayCommand(p => ExecuteDeletePurchaseCommand(p));
                }
                return _DeletePurchaseCommand;
            }
        }

        private void ExecuteDeletePurchaseCommand(object p)
        {
            SelectedPurchase.Delete();
            Purchases = Purchase.FetchPurchases();

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
            PurchaseView PurchaseView = new PurchaseView();
            PurchaseViewModel PurchaseViewModel = new PurchaseViewModel();
            PurchaseView.DataContext = PurchaseViewModel;
            PurchaseViewModel.Purchase = SelectedPurchase;
            PurchaseView.ShowDialog();
            Purchases = Purchase.FetchPurchases();
        }

        private RelayCommand _PrintCommand;
        public ICommand PrintCommand
        {
            get
            {
                if (_PrintCommand == null)
                {
                    _PrintCommand = new RelayCommand(p => ExecutePrintCommand());
                }
                return _PrintCommand;
            }

        }
        private void ExecutePrintCommand()
        {
            //// Create a report viewer instance
            //ReportViewer reportViewer = new ReportViewer();

            //// Set processing mode (Local or Remote)
            //reportViewer.ProcessingMode = ProcessingMode.Local;

            //// Set the report path (without .rdlc extension if embedded)
            //reportViewer.LocalReport.ReportPath = "Report.rdlc";
            //// Or for embedded resources:
            //// reportViewer.LocalReport.ReportEmbeddedResource = "YourNamespace.YourReport.rdlc";

            //// Add data sources if needed
            //// reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetName", yourData));

            //// Refresh the report
            //reportViewer.RefreshReport();

            //// Print the report
            //reportViewer.PrintDialog();
        }
    }

}

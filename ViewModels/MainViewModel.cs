using MyShop.Common;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.ViewModels;  // Ensure you have this using directive
using MyShop.Views;       // Ensure you have this using directive
using System.Windows;
using System.Windows.Input;
namespace MyShop.ViewModels
{
   
    public class MainViewModel : BindableBase
    {
        public MainViewModel() 
        {
            TabViewModels = new ObservableCollection<TabViewModel>();
            TabViewModels.Add(new PurchaseListViewModel()) ;
           TabViewModels.Add(new SaleListViewModel());
            TabViewModels.Add(new BranchListViewModel());


        }

        private ObservableCollection<TabViewModel> _TabViewModels;
        public ObservableCollection<TabViewModel> TabViewModels
        {
            get { return _TabViewModels; }
            set
            {
                if (_TabViewModels != value)
                {
                    _TabViewModels = value;
                    OnPropertyChanged(nameof(TabViewModels));
                }
            }
        }
        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged(nameof(SelectedTab));
                }
            }

        }
        private RelayCommand _ProductCommand;
        public ICommand ProductCommand
        {
            get
            {
                if (_ProductCommand == null)
                {
                    _ProductCommand = new RelayCommand(p => ExecuteProductCommand());
                }
                return _ProductCommand;
            }
        }
        private void ExecuteProductCommand()
        {
            ProductView productView = new ProductView();
            ProductViewModel productViewModel = new ProductViewModel();
            productView.DataContext = productViewModel;
            productView.ShowDialog();

            // Products = Product.FetchProducts();
        }
        private RelayCommand _SaleCommand;
        public ICommand SaleCommand
        {
            get
            {
                if (_SaleCommand == null)
                {
                    _SaleCommand = new RelayCommand(p => ExecuteSaleCommand());
                }
                return _ProductCommand;
            }
        }
        private void ExecuteSaleCommand()
        {
           
        }
    }
}

using MyShop.Common;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    public class SalesListViewModel : TabViewModel
    {
        public SalesListViewModel()
        {
            new ObservableCollection<Sales>(Sales.FetchSales());
            TabHeading = "Sales List";
        }
        #region Property   

        private Sales _SelectedSales;
        public Sales SelectedSales
        {
            get { return _SelectedSales; }
            set
            {
                if (_SelectedSales != value)
                {
                    _SelectedSales = value;
                    OnPropertyChanged(nameof(SelectedSales));
                }
            }
        }
        #endregion
        #region Command

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
            SelectedSales = new Sales(); // Reset the Sales object 
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
    }
}


        #endregion

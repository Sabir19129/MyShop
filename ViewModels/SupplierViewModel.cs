
using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    internal class SupplierViewModel : BindableBase
    {
        #region Properties
        public SupplierViewModel()
        {
            Supplier = new Supplier();
            Suppliers = new List<Supplier>();
        }

        // Property to hold the list of Suppliers
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

        // Property to hold the current Supplier object
        private Supplier _Supplier;
        public Supplier Supplier
        {
            get { return _Supplier; }
            set
            {
                if (_Supplier != value)
                {
                    _Supplier = value;
                    OnPropertyChanged(nameof(Supplier));
                }
            }
        }
        #endregion
        // SaveCommand to insert a new Supplier
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
            Supplier.Insert(); // Insert the Supplier
            Supplier = new Supplier(); // Reset the Supplier object
            Suppliers = Supplier.FetchSuppliers();
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
            Supplier.Update(); // update the Supplier
            Supplier = new Supplier(); // Reset the Supplier object

            Suppliers = Supplier.FetchSuppliers(); // Call the Get method from the Supplier model

        }

        RelayCommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new RelayCommand(p => ExecuteDeleteCommand(p));
                }
                return _DeleteCommand;
            }
        }

        private void ExecuteDeleteCommand(object p)
        {
            var Supplier = p as Supplier;
            if (Supplier == null) return;

            // Show confirmation dialog
            var result = MessageBox.Show($"This will delete {Supplier.Name} permanently. Do you want to proceed?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            // Check Supplier's choice
            if (result == MessageBoxResult.Yes)
            {
                Supplier.Delete(Supplier.Id); // Proceed with deletion

                Suppliers = Supplier.FetchSuppliers(); // Call the Get method from the Supplier model
            }
        }





        // FetchCommand to get Suppliers based on search criteria
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

        // Fetch data from the Supplier model and update the Suppliers list
        private void ExecuteFetchCommand()
        {
            Suppliers = Supplier.FetchSuppliers(); // Call the Get method from the Supplier model
        }
    }
    #endregion
}


using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    internal class CustomerViewModel : TabViewModel
    {
        #region Properties
        public CustomerViewModel()
        {
            Customer = new Customer();
            Customers = new List<Customer>();
        }

        // Property to hold the list of Customers
        private List<Customer> _Customers;
        public List<Customer> Customers
        {
            get { return _Customers; }
            set
            {
                if (_Customers != value)
                {
                    _Customers = value;
                    OnPropertyChanged(nameof(Customers));
                }
            }
        }

        // Property to hold the current Customer object
        private Customer _Customer;
        public Customer Customer
        {
            get { return _Customer; }
            set
            {
                if (_Customer != value)
                {
                    _Customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }
        #endregion
        // SaveCommand to insert a new Customer
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
            Customer.Insert(); // Insert the Customer
            Customer = new Customer(); // Reset the Customer object
            Customers = Customer.FetchCustomers();
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
            Customer.Update(); // update the Customer
            Customer = new Customer(); // Reset the Customer object

            Customers = Customer.FetchCustomers(); // Call the Get method from the Customer model

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
            var Customer = p as Customer;
            if (Customer == null) return;

            // Show confirmation dialog
            var result = MessageBox.Show($"This will delete {Customer.Name} permanently. Do you want to proceed?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            // Check Customer's choice
            if (result == MessageBoxResult.Yes)
            {
                if (Customer != null) // Check if Customer is not null
                {
                    Customer.Delete(Customer.Id); // Proceed with deletion

                    var Customers = Customer.FetchCustomers(); // Fetch Customers
                    if (Customers != null) // Check if Customers list is not null
                    {
                        Customers = Customers; // Assign to Customers
                    }
                    else
                    {
                        // Handle the case where FetchCustomer returns null
                        Customers = new List<Customer>(); // Assign an empty list or handle accordingly
                    }
                }
            }
            else
            {
                // Handle the case where Customer is null
                throw new ArgumentNullException(nameof(Customer), "Customer object cannot be null.");
            }
        }





        // FetchCommand to get Customers based on search criteria
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

        // Fetch data from the Customer model and update the Customers list
        private void ExecuteFetchCommand()
        {
            Customers = Customer.FetchCustomers(); // Call the Get method from the Customer model
        }
    }
    #endregion
}

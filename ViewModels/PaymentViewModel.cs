
using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    internal class PaymentViewModel : TabViewModel
    {
        #region Properties
        public PaymentViewModel()
        {
            Payment = new Payment();
            Payments = new List<Payment>();

        }

        // Property to hold the list of Payments
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

        // Property to hold the current Payment object
        private Payment _Payment;
        public Payment Payment
        {
            get { return _Payment; }
            set
            {
                if (_Payment != value)
                {
                    _Payment = value;
                    OnPropertyChanged(nameof(Payment));
                }
            }
        }
        #endregion
        // SaveCommand to insert a new Payment
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
            Payment.Insert(); // Insert the Payment
            Payment = new Payment(); // Reset the Payment object
            Payments = Payment.FetchPayments();
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
            Payment.Update(); // update the Payment
            Payment = new Payment(); // Reset the Payment object

            Payments = Payment.FetchPayments(); // Call the Get method from the Payment model

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
            var Payment = p as Payment;
            if (Payment == null) return;

            // Show confirmation dialog
            var result = MessageBox.Show($"This will delete {Payment.Name} permanently. Do you want to proceed?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            // Check Payment's choice
            if (result == MessageBoxResult.Yes)
            {
                Payment.Delete(Payment.Id); // Proceed with deletion

                Payments = Payment.FetchPayments(); // Call the Get method from the Payment model
            }
        }





        // FetchCommand to get Payments based on search criteria
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

        // Fetch data from the Payment model and update the Payments list
        private void ExecuteFetchCommand()
        {
            Payments = Payment.FetchPayments(); // Call the Get method from the Payment model
        }
    }
    #endregion
}

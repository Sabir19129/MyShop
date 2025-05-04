
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
    internal class BranchViewModel : TabViewModel
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Constructor

        public BranchViewModel()
        {
            Branch = new Branch();
            Branchs = new List<Branch>();
            IsEditMode = false;
            IsSaveMode = false;
            BranchDetail = new BranchDetail();
            //Branchs = Branch.FetchBranchs();



        }

        #endregion

        #region Properties
        public List<string> TimeOptions { get; set; } = new List<string>
    {
        "08:00 AM", "09:00 AM", "10:00 AM", "11:00 AM",
        "12:00 PM", "01:00 PM", "02:00 PM", "03:00 PM",
        "04:00 PM", "05:00 PM", "06:00 PM", "07:00 PM",
        "08:00 PM", "09:00 PM", "10:00 PM", "11:00 PM"
    };

        private List<int> DeletedIds { get; set; } = new List<int>();

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




        private Branch _Branch;
        public Branch Branch
        {
            get { return _Branch; }
            set
            {
                if (_Branch != value)
                {
                    _Branch = value;
                    OnPropertyChanged(nameof(Branch));
                    IsEditMode = true;
                    IsSaveMode = false;

                   // Branch.BranchDetails.CollectionChanged += BranchDetails_CollectionChanged;
                }
            }
        }

        //private void BranchDetails_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    Branch.TotalPrice = Branch.BranchDetails.Sum(x => x.TotalPrice);
        //}


        //private List<Branch> _Branchlist;
        //public List<Branch> Branchlist
        //{
        //    get { return _Branchlist; }
        //    set
        //    {
        //       if (_Branchlist != value)
        //        {
        //            _Branchlist = value;
        //            OnPropertyChanged(nameof(Branchlist));
        //        }
        //    }
        //}

        private List<Branch> _Branchs;
        public List<Branch> Branchs
        {
            get { return _Branchs; }
            set
            {
                if (_Branchs != value)
                {
                    _Branchs = value;
                    OnPropertyChanged(nameof(Branchs));
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
        public void StartNewBranch()
        {
            IsEditMode = false; // For new Branchs (Save button visible)
        }

        public void StartEditingBranch()
        {
            IsEditMode = true; // For editing Branchs (Update button visible)
        }
        private BranchDetail _BranchDetail = new BranchDetail();
        public BranchDetail BranchDetail
        {
            get { return _BranchDetail; }
            set
            {
                if (_BranchDetail != value)
                {
                    _BranchDetail = value;
                    OnPropertyChanged(nameof(BranchDetail));
                }
            }
        }

        private void BranchDetail_ProductChanged(object? sender, EventArgs e)
        {

        }

        private List<BranchDetail> _BranchDetails;
        public List<BranchDetail> BranchDetails
        {
            get { return _BranchDetails; }
            set
            {
                if (_BranchDetails != value)
                {
                    _BranchDetails = value;
                    OnPropertyChanged(nameof(BranchDetails));
                }
            }
        }

        #endregion

        #region ICmd Members
        // SaveCommand to get Branchs
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
            //if (BranchDetail.Product == null )
            //{
            //    MessageBox.Show("Please select Product.");
            //    return;
            //}

            if (Branch.Id == 0)//Id is zero means it's new Branch
            {
                Branch.Insert();//Insert Branch
            }
            else//Branch needs to be updated
            {
                Branch.Update(DeletedIds);//Update Branch
            }

            Branch = new Branch();  // Reset after save
            Branchs = Branch.FetchBranchs();

            // After saving, reset IsEditMode for creating mode
            IsEditMode = false;  // Hide the Save button after saving
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
            

            Branch.BranchDetails.Add(new BranchDetail
            {
                Id = BranchDetail.Id,
                NoOfEmployee = BranchDetail.NoOfEmployee,
                StartTime = BranchDetail.StartTime,
                EndTime = BranchDetail.EndTime,
                Feedback = BranchDetail.Feedback,

            });
            OnPropertyChanged(nameof(BranchDetail)); // Notify UI

            BranchDetail = new BranchDetail(); // Reset for new entry
                                                   // Reset for the next entry
        }






        // DeleteCommand to delete a Branch
        private RelayCommand _DeleteBranchDetailCommand;
        public ICommand DeleteBranchDetailCommand
        {
            get
            {
                if (_DeleteBranchDetailCommand == null)
                {
                    _DeleteBranchDetailCommand = new RelayCommand(p => ExecuteDeleteBranchDetailCommand(p));
                }
                return _DeleteBranchDetailCommand;
            }
        }

        private void ExecuteDeleteBranchDetailCommand(object p)
        {
            // if (BranchDetail.Product == null || BranchDetail.Product.Id == 0 || BranchDetail.TotalPrice ==0 || BranchDetail.Quantity <= 0 
            //   || BranchDetail.Price <= 0)
            //{
            //    MessageBox.Show("Please select a Branch to delete.");
            //    return;
            //}

            //var BranchDetail = p as BranchDetail;
            //if (BranchDetail == null) return;

            //var result = MessageBox.Show($"This will delete {BranchDetail.Product.Id} permanently. Do you want to proceed?",
            //                              "Confirm Delete",
            //                              MessageBoxButton.YesNo,
            //                              MessageBoxImage.Warning);

            //if (result == MessageBoxResult.Yes)
            //{
            //    DeletedIds.Add(BranchDetail.Id);

            //    Branch.BranchDetails.Remove(BranchDetail);
            //    //Branchs = Branch.FetchBranchs(); // Refresh Branchs
            //    //Branch = new Branch(); // Reset Branch
            //}
        }

        // FetchCommand to get Branchs
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
            Branchs = Branch.FetchBranchs();
            //Branchs = Branch.FetchBranchDetails();// Fetch the list of Branchs
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
            var result = MessageBox.Show("This will cancel all your entries of Branch. Do you want to continue?",
                                         "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Branch = new Branch(); // This will now reflect in the UI
            }
            else
            {
            }
        }
        private RelayCommand _CheckOutCommand;
        public ICommand CheckOutCommand
        {
            get
            {
                if (_CheckOutCommand == null)
                {
                    _CheckOutCommand = new RelayCommand(p => ExecuteCheckOutCommand());
                }
                return _CheckOutCommand;
            }
        }


        private void ExecuteCheckOutCommand()
        {
            var result = MessageBox.Show("You want to check Out or Continue Shopping?",
                                         "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Branch = new Branch(); // This will now reflect in the UI
            }
            else
            {
            }
        }
        //private RelayCommand _PaymentCommand;
        //public ICommand PaymentCommand
        //{
        //    get
        //    {
        //        if (_PaymentCommand == null)
        //        {
        //            _PaymentCommand = new RelayCommand(p => ExecutePaymentCommand());
        //        }
        //        return _PaymentCommand;
        //    }
        //}

        //private void ExecutePaymentCommand()
        //{
        //    var result = MessageBox.Show("Do you want to confirm your Branchs?",
        //                                 "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        //    if (result == MessageBoxResult.Yes)
        //    {
        //        MessageBox.Show("Please pay the bill");
        //        Branch = new Branch(); // This will now reflect in the UI
        //    }
        //    else
        //    {
        //    }
        //}

        #endregion
    }
}

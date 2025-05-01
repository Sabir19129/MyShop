using MyShop.Common;
using MyShop.Models;
using MyShop.ViewModels;
using MyShop.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    public class BranchListViewModel : TabViewModel
    {

        public BranchListViewModel()
        {
            Branchs = Branch.FetchBranchs();
            TabHeading = "Branch List";
            SelectedBranch = new Branch();
        }
        private Branch _SelectedBranch;
        public Branch SelectedBranch
        {
            get { return _SelectedBranch; }
            set
            {
                if (_SelectedBranch != value)
                {
                    _SelectedBranch = value;
                    OnPropertyChanged(nameof(SelectedBranch));
                }
            }
        }


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
            BranchView BranchView = new BranchView();
            BranchViewModel BranchViewModel = new BranchViewModel();
            BranchView.DataContext = BranchViewModel;
            BranchView.ShowDialog();
            // Reset for the next entry
        }
        private RelayCommand _DeleteBranchCommand;
        public ICommand DeleteBranchCommand
        {
            get
            {
                if (_DeleteBranchCommand == null)
                {
                    _DeleteBranchCommand = new RelayCommand(p => ExecuteDeleteBranchCommand(p));
                }
                return _DeleteBranchCommand;
            }
        }

        private void ExecuteDeleteBranchCommand(object p)
        {
            SelectedBranch.Delete();
            Branchs = Branch.FetchBranchs();

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
            Branchs = Branch.FetchBranchs();
            //Branchs = Branch.FetchBranchDetails();// Fetch the list of Branchs
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
            BranchView BranchView = new BranchView();
            BranchViewModel BranchViewModel = new BranchViewModel();
            BranchView.DataContext = BranchViewModel;
            BranchViewModel.Branch = SelectedBranch;
            BranchView.ShowDialog();
            Branchs = Branch.FetchBranchs();
        }


    }
}


using MyShop.Common;
using MyShop.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MyShop.ViewModels
{
    internal class UserViewModel : BindableBase
    {
        #region Properties
        public UserViewModel()
        {
            User = new User();
            Users = new List<User>();
        }

        // Property to hold the list of Users
        private List<User> _Users;
        public List<User> Users
        {
            get { return _Users; }
            set
            {
                if (_Users != value)
                {
                    _Users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }

        // Property to hold the current User object
        private User _User;
        public User User
        {
            get { return _User; }
            set
            {
                if (_User != value)
                {
                    _User = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }
        #endregion
        // SaveCommand to insert a new User
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
            User.Insert(); // Insert the User
            User = new User(); // Reset the User object
            Users = User.FetchUsers();
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
            User.Update(); // update the User
            User = new User(); // Reset the User object
          
            Users = User.FetchUsers(); // Call the Get method from the User model
            
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
            var user = p as User;
            if (user == null) return;

            // Show confirmation dialog
            var result = MessageBox.Show($"This will delete {user.Name} permanently. Do you want to proceed?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            // Check user's choice
            if (result == MessageBoxResult.Yes)
            {
                if (user != null) // Check if user is not null
                {
                    User.Delete(user.Id); // Proceed with deletion

                    var users = User.FetchUsers(); // Fetch users
                    if (users != null) // Check if users list is not null
                    {
                        Users = users; // Assign to Users
                    }
                    else
                    {
                        // Handle the case where FetchUsers returns null
                        Users = new List<User>(); // Assign an empty list or handle accordingly
                    }
                }
                }
                else
                {
                    // Handle the case where user is null
                    throw new ArgumentNullException(nameof(user), "User object cannot be null.");
                }
        }





        // FetchCommand to get Users based on search criteria
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

        // Fetch data from the User model and update the Users list
        private void ExecuteFetchCommand()
        {
            Users = User.FetchUsers(); // Call the Get method from the User model
        }
    }
    #endregion
}

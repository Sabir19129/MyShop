using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    internal class User : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Properties
        private int _ProductId;
        public int Id
        {
            get { return _ProductId; }
            set
            {
                if (_ProductId != value)
                {
                    _ProductId = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        #endregion
        #region Functions
        public void Insert()
        {
            // Check if the User with the same ID already exists
            string checkQuery = "SELECT COUNT(1) FROM Users WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        connection.Open();
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("A user with the same ID already exists. Please enter a unique ID.");
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while checking for duplicate user: " + ex.Message);
                        return;
                    }
                }

                // Insert the new User if no duplicate is found
                string query = "INSERT INTO Users ( Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(query, connection))
                { 
                    command.Parameters.AddWithValue("@Name", Name);

                    try
                    {
                      
                        command.ExecuteNonQuery();
                        MessageBox.Show("User inserted successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the user: " + ex.Message);
                    }                 
                }
            }
        }

        // Method to update an existing user
        public void Update()
        {
            // Ensure that ID is not empty or invalid
            if (Id == 0)
            {
                MessageBox.Show("Please select a User to update.");
                return;
            }

            string query = "UPDATE Users SET Name = @Name WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", Name);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("User updated successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        // Method to delete a user
        public void Delete(int id)
        {
            // Ensure the ID is valid before attempting to delete
            if (id <= 0)
            {
                MessageBox.Show("Invalid ID. Please select a valid user.");
                return;
            }

            string query = "DELETE FROM Users WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("User not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting the user: " + ex.Message);
                    }
                }
            }
        }

        // Method to fetch all users
        public List<User> FetchUsers()
        {
            string query = "SELECT Id, Name FROM Users";
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString()
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching users: " + ex.Message);
                    }
                }
            }

            return users;
        }

        // Override ToString method for better display in UI
        public override string ToString()
        {
            return Name; // This will show the name when displaying a User object
        }
        #endregion
      
    }
}

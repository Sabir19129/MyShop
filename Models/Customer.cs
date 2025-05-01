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
    public class Customer : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Properties
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
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
            // Check if the Customer with the same ID already exists
            string checkQuery = "SELECT COUNT(1) FROM Customer WHERE Id = @Id";

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
                            MessageBox.Show("A Customer with the same ID already exists. Please enter a unique ID.");
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while checking for duplicate Customer: " + ex.Message);
                        return;
                    }
                }

                // Insert the new Customer if no duplicate is found
                string query = "INSERT INTO Customer ( Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);

                    try
                    {

                        command.ExecuteNonQuery();
                        MessageBox.Show("Customer inserted successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the Customer: " + ex.Message);
                    }
                }
            }
        }

        // Method to update an existing Customer
        public void Update()
        {
            // Ensure that ID is not empty or invalid
            if (Id == 0)
            {
                MessageBox.Show("Please select a Customer to update.");
                return;
            }

            string query = "UPDATE Customer SET Name = @Name WHERE Id = @Id";

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
                        MessageBox.Show("Customer updated successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        // Method to delete a Customer
        public void Delete(int id)
        {
            // Ensure the ID is valid before attempting to delete
            if (id <= 0)
            {
                MessageBox.Show("Invalid ID. Please select a valid Customer.");
                return;
            }

            string query = "DELETE FROM Customer WHERE Id = @Id";

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
                            MessageBox.Show("Customer deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Customer not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting the Customer: " + ex.Message);
                    }
                }
            }
        }

        // Method to fetch all Customer
        public static List<Customer> FetchCustomers()
        {
            string query = "SELECT Id, Name FROM Customer";
            List<Customer> Customer = new List<Customer>();

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
                                Customer.Add(new Customer()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString()
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Customer: " + ex.Message);
                    }
                }
            }

            return Customer;
        }

        // Override ToString method for better display in UI
        public override string ToString()
        {
            return Name; // This will show the name when displaying a Customer object
        }
        #endregion

    }
}

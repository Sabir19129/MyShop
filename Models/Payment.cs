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
    internal class Payment : BindableBase, IEquatable<Payment>
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
            // Check if the User with the same ID already exists
            string checkQuery = "SELECT COUNT(1) FROM Payment WHERE Id = @Id";

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
                            MessageBox.Show("A Payment with the same ID already exists. Please enter a unique Id.");
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while checking for duplicate Payment: " + ex.Message);
                        return;
                    }
                }

                // Insert the new User if no duplicate is found
                string query = "INSERT INTO Payment ( Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);

                    try
                    {

                        command.ExecuteNonQuery();
                        MessageBox.Show("Payment inserted successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the Payment: " + ex.Message);
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
                MessageBox.Show("Please select a Payment to update.");
                return;
            }

            string query = "UPDATE Payment SET Name = @Name WHERE Id = @Id";

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
                        MessageBox.Show("Payment updated successfully.");
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
                MessageBox.Show("Invalid ID. Please select a valid Payment.");
                return;
            }

            string query = "DELETE FROM Payment WHERE Id = @Id";

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
                            MessageBox.Show("Payment deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Payment not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting the Payment: " + ex.Message);
                    }
                }
            }
        }

        // Method to fetch all Payment
        public static List<Payment> FetchPayments()
        {
            string query = "SELECT Id, Name FROM Payment";
            List<Payment> Payments = new List<Payment>();

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
                                Payments.Add(new Payment()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString()
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Payments: " + ex.Message);
                    }
                }
            }

            return Payments;
        }

        // Override ToString method for better display in UI
        public override string ToString()
        {
            return Name; // This will show the name when displaying a User object
        }
        #endregion
        public bool Equals(Payment? other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }

    }
}

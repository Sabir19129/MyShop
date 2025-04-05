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
    public class Supplier : BindableBase, IEquatable<Supplier>
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

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }
        private string _Contact;
        public string Contact
        {
            get { return _Contact; }
            set
            {
                if (_Contact != value)
                {
                    _Contact = value;
                    OnPropertyChanged(nameof(Contact));
                }
            }
        }
        #endregion
        #region Functions
        public void Insert()
        {
            // Check if the User with the same ID already exists
            string checkQuery = "SELECT COUNT(1) FROM Supplier WHERE Id = @Id";

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
                            MessageBox.Show("A Supplier with the same ID already exists. Please enter a unique ID.");
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while checking for duplicate Supplier: " + ex.Message);
                        return;
                    }
                }

                // Insert the new User if no duplicate is found
                string query = "INSERT INTO Supplier ( Name, Address, Contact) VALUES (@Name, @Address, @Contact)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Address", Address); 
                    command.Parameters.AddWithValue("@Contact", Contact);

                    try
                    {

                        command.ExecuteNonQuery();
                        MessageBox.Show("Supplier inserted successfully.");
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
                MessageBox.Show("Please select a Supplier to update.");
                return;
            }

            string query = "UPDATE Supplier SET Name = @Name, Address = @Address, Contact = @Contact WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Contact", Contact);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Supplier updated successfully.");
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
                MessageBox.Show("Invalid ID. Please select a valid Supplier.");
                return;
            }

            string query = "DELETE FROM Supplier WHERE Id = @Id";

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
                            MessageBox.Show("Supplier deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Supplier not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting the Supplier: " + ex.Message);
                    }
                }
            }
        }

        // Method to fetch all Supplier
        public static List<Supplier> FetchSuppliers()
        {
            string query = "SELECT Id, Name,Contact,Address FROM Supplier";
            List<Supplier> Suppliers = new List<Supplier>();

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
                                Suppliers.Add(new Supplier()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Contact = reader["Contact"].ToString(),
                                    Address = reader["Address"].ToString()
                                });

                                
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Suppliers: " + ex.Message);
                    }
                }
            }

            return Suppliers;
        }

        // Override ToString method for better display in UI
        public override string ToString()
        {
            return Name; // This will show the name when displaying a User object
        }
        #endregion

      
        public bool Equals(Supplier? other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }
    }
}

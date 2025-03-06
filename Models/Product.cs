using MyShop.Common;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace MyShop.Models
{
    internal class Product : BindableBase// The BindableBase class implements INotifyPropertyChanged to enable property change notifications
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";

        #region Properties

        private int _Id;
        public int Id
        {
            get => _Id;
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

        private int _Price;
        public int Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged(nameof(Price)); // Fixed here
                }
            }
        }

        private string _Madein;
        public string Madein
        {
            get { return _Madein; }
            set
            {
                if (_Madein != value)
                {
                    _Madein = value;
                    OnPropertyChanged(nameof(Madein)); // Fixed here
                }
            }
        }
        private int _Stock;
        public int Stock
        {
            get =>_Stock;
           // get { return _Stock; } , both can be used Lambda and the return method
            set
            {
                if (_Stock != value)
                {
                    _Stock = value;
                    OnPropertyChanged(nameof(Stock)); // Fixed here
                }
            }
        }

        #endregion

        #region Function
        public void Insert()
        {
            //This function is used to check for duplicate records
            //in the Product table in a database. It ensures that a product with the same Id does not already exist before proceeding
            //with any operation 
            string checkQuery = "SELECT COUNT(1) FROM Product WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        connection.Open();
                        int count = (int)checkCommand.ExecuteScalar(); // Get the number of rows matching the ID

                        if (count > 0)
                        {
                            MessageBox.Show("A product with the same ID already exists. Please enter a unique ID.");
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while checking for duplicate Product: " + ex.Message);
                        return;
                    }
                }

                // Insert the new Product if no duplicate is found
                string query = "INSERT INTO Product (Name, Price, Madein) VALUES (@Name, @Price, @Madein)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Price", Price);
                    command.Parameters.AddWithValue("@Madein", Madein);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Product inserted successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the Product: " + ex.Message);
                    }
                }
            }
        }
        public List<Product> FetchProducts()
        {
            string query = "SELECT Id, Name, Madein, Price,Stock FROM Product";
            List<Product> products = new List<Product>();

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
                                products.Add(new Product()
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    Madein = reader["Madein"].ToString(),
                                    Stock = Convert.ToInt32(reader["Stock"]),
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Products: " + ex.Message);
                    }
                }
            }

            return products;
        }
        public void Update()
        {
            // Ensure that ID is not empty or invalid
            if (Id == 0)
            {
                MessageBox.Show("Please select a product to update."); // Fixed message
                return;
            }

            string query = "UPDATE Product SET Name = @name, Price = @Price, Madein = @Madein WHERE ID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Price", Price);
                    command.Parameters.AddWithValue("@Madein", Madein);

                    try
                    {
                        connection.Open(); // Open the connection before executing
                        command.ExecuteNonQuery();//Executes a SQL command (like INSERT, UPDATE, DELETE) that does not return any data.
                                                  //It returns the number of rows affected.
                        MessageBox.Show("Product updated successfully."); // Fixed message
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }
        public static void UpdateStock(int ProductId, int s_Quantity)
        {

            // The query now allows both increases (positive Quantity) and decreases (negative Quantity)
            string query = "UPDATE Product SET Stock = Stock + @Quantity WHERE Id = @ProductId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adding parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.Parameters.AddWithValue("@Quantity", s_Quantity); // Quantity can be positive (purchase) or negative (sale)
                   

                    try
                    {
                        // Open the database connection
                        connection.Open();
                        // Execute the command
                        var result = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // If an error occurs, show a message box
                        MessageBox.Show("An error occurred while updating stock: " + ex.Message);
                    }
                }
            }
        }
        public void Delete(int Id)
        {
            // Ensure the ID is valid before attempting to delete
            if (Id <= 0)
            {
                MessageBox.Show("Invalid ID. Please select a valid product.");
                return;
            }

            string query = "DELETE FROM product WHERE ID = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", Id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("product deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("product not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting the product: " + ex.Message);
                    }
                }
            }
        }
       
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object? that)
        {
            if (that == null)
            {
                return base.Equals(that);
            }
            else
            {
                if (that is Product product)
                {
                    return this.Id == product.Id;
                }
                else
                {
                    return base.Equals(that);
                }
            }
        }

        #endregion
    }
}

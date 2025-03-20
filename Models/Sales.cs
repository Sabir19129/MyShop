using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace MyShop.Models
{
    internal class Sales : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";

        #region Properties


        public Sales()
        {
           Product = new Product();
        }




        private Product _Product;
        public Product Product
        {
            get { return _Product; }
            set
            {
                if (_Product != value)
                {
                    _Product = value;
                    OnPropertyChanged(nameof(Product));
                }
            }
        }
        private DateTime _SaleDate = DateTime.Now;
        public DateTime SaleDate
        {
            get { return _SaleDate; }
            set
            {
                if (_SaleDate != value)
                {
                    _SaleDate = value;
                    OnPropertyChanged(nameof(SaleDate));
                }
            }
        }

        private int _Quantity;
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    //CalculateTotalSale();
                }
            }
        }

        private decimal _Price; // Changed to decimal as price typically has decimal values
        public decimal Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged(nameof(Price));
                    CalculateTotalSale();
                }
            }
        }

        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                if (_TotalPrice != value)
                {
                    _TotalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        private string _PaymentMethod; // Ensure that PaymentMethod matches your table definition
        public string PaymentMethod
        {
            get { return _PaymentMethod; }
            set
            {
                if (_PaymentMethod != value)
                {
                    _PaymentMethod = value;
                    OnPropertyChanged(nameof(PaymentMethod));
                }
            }
        }

        private void CalculateTotalSale()
        {
            TotalPrice = Quantity * Price;
        }

        #endregion

        #region Functions
        public void Insert()

        {
            string query = "INSERT INTO Sales (ProductId, Quantity, PaymentMethod, TotalPrice, SaleDate, Price) " +
                           "VALUES (@ProductId, @Quantity, @PaymentMethod, @TotalPrice, @SaleDate, @Price)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@ProductId", Product.Id); // Ensure Product.Id is valid
                    command.Parameters.AddWithValue("@Quantity", Quantity);   // Ensure Quantity is not null
                    command.Parameters.AddWithValue("@PaymentMethod", PaymentMethod ?? (object)DBNull.Value); // Handle null values
                    command.Parameters.AddWithValue("@TotalPrice", TotalPrice); // Ensure TotalPrice is a valid decimal
                    command.Parameters.AddWithValue("@SaleDate", SaleDate);   // Ensure SaleDate is a valid DateTime
                    command.Parameters.AddWithValue("@Price", Price);         // Ensure Price is a valid decimal

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();


                        // Optionally update stock after updating the sale
                        Product.UpdateStock(Product.Id, Quantity * -1);

                        MessageBox.Show("Sale recorded successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL Error: " + ex.Message);
                    }
                }
            }
        }

        // Method to reduce the quantity in Purchase based on the quantity sold
        internal void Update()
        {
            // Make sure SalesId is available
            if (this.Product.Id <= 0)
            {
                MessageBox.Show("Invalid ProductId");
                return;
            }

            string updateQuery = "UPDATE Sales SET Quantity = @Quantity, PaymentMethod = @PaymentMethod, TotalPrice = @TotalPrice,SaleDate = @SaleDate, Price = @Price WHERE SaleId = @SaleId";  // Assuming ProductId is used for identifying the sales entry

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@ProductId", Product.Id);  // Ensure Product.Id is valid
                    command.Parameters.AddWithValue("@Price", Price); // Ensure Price is a valid decimal
                    command.Parameters.AddWithValue("@Quantity", Quantity);  // Ensure Quantity is not null
                    command.Parameters.AddWithValue("@TotalPrice", TotalPrice); // Ensure TotalPrice is a valid decimal
                    command.Parameters.AddWithValue("@PaymentMethod", PaymentMethod ?? (object)DBNull.Value); // Handle null values
                    command.Parameters.AddWithValue("@SaleDate", SaleDate); // Ensure SaleDate is a valid DateTime
                   

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();


                        MessageBox.Show("Sale updated successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL Error: " + ex.Message);
                    }
                }
            }
        }

        public void Delete(int SalesId)  // Ensure you are passing the correct SalesId
        {
            if (SalesId <= 0)
            {
                MessageBox.Show("Invalid SalesId. Please select a valid sale.");
                return;
            }

            string query = "DELETE FROM Sales WHERE SalesId = @SalesId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SalesId", SalesId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Sales deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Sales not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error deleting sale: " + ex.Message);
                    }
                }
            }
        }

        internal IEnumerable<Sales> FetchSales()
        {
            List<Sales> salesList = new List<Sales>();
            string query = "SELECT pr.Id AS ProductId, s.Quantity, s.PaymentMethod, s.Price, s.TotalPrice, s.SaleDate, pr.Name AS ProductName " +
                "FROM Product pr  INNER JOIN Sales s ON s.ProductId = pr.Id";

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
                                salesList.Add(new Sales
                                {
                                    Product = new Product()
                                    {
                                        Id = (int)reader["ProductId"],  // Correct column for ProductId
                                        Name = reader["ProductName"].ToString()  // Correct column for ProductName
                                    },
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    PaymentMethod = reader["PaymentMethod"].ToString(),
                                    TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                                    SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                                    Price = Convert.ToDecimal(reader["Price"])
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error fetching sales: " + ex.Message);
                    }
                }
            }

            return salesList;
        }

        #endregion
    }
}

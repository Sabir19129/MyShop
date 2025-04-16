using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    public class Sale : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        public event EventHandler QuantityChanged;

        #region Properties
        public Sale()
        {
            SaleDetails = new ObservableCollection<SaleDetail>();
            Payment = new Payment(); // Initialize empty payment
            User = new User();       // Initialize empty user
        }
        private ObservableCollection<SaleDetail> _SaleDetails;
        public ObservableCollection<SaleDetail> SaleDetails
        {
            get { return _SaleDetails; }
            set
            {
                if (_SaleDetails != value)
                {
                    _SaleDetails = value;
                    OnPropertyChanged(nameof(SaleDetails));
                }
            }
        }
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
                    CalculateTotalPrice();
                    QuantityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private List<Supplier> _Suppliers;
        public List<Supplier> Suppliers
        {
            get { return _Suppliers; }
            set
            {
                if (_Suppliers != value)
                {
                    _Suppliers = value;
                    OnPropertyChanged(nameof(Suppliers));
                }
            }
        }
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
        private int _Price; // Changed to decimal as price typically has decimal values
        public int Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged(nameof(Price));
                    CalculateTotalPrice();
                }
            }
        }
        public void CalculateTotalPrice()
        {
            TotalPrice = Quantity * Price;
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
        private Supplier _Supplier;
        public Supplier Supplier
        {
            get { return _Supplier; }
            set
            {
                if (_Supplier != value)
                {
                    _Supplier = value;
                    OnPropertyChanged(nameof(Supplier));
                }
            }
        }

        #endregion

        #region Functions
        public void Insert()

        {
            string query = "INSERT INTO Sale (PaymentId, TotalPrice, SaleDate, UserId) " +
                "VALUES (@PaymentId, @TotalPrice, @SaleDate, @UserId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {


                    command.Parameters.AddWithValue("@UserId", User.Id);
                    command.Parameters.AddWithValue("@PaymentId", Payment.Id);
                    command.Parameters.AddWithValue("@SaleDate", SaleDate);
                    command.Parameters.AddWithValue("@TotalPrice", TotalPrice);
                    // Ensure Price is a valid decimal

                    try
                    {
                        connection.Open();
                        var newId = command.ExecuteScalar(); // Get the newly inserted Purchase ID

                        if (newId != null)
                        {
                            foreach (var SaleDetail in SaleDetails)
                            {
                                string queryDetail = "INSERT INTO SaleDetail (SaleId, ProductId, Quantity, Price) VALUES (@SaleId, @ProductId, @Quantity, @Price)";
                                using (SqlCommand command1 = new SqlCommand(queryDetail, connection))
                                {
                                    command1.Parameters.AddWithValue("@SaleId", Id);
                                    command1.Parameters.AddWithValue("@ProductId", SaleDetail.Product.Id);
                                    command1.Parameters.AddWithValue("@Quantity", SaleDetail.Quantity);
                                    command1.Parameters.AddWithValue("@Price", SaleDetail.Price);
                                    // command1.Parameters.AddWithValue("@TotalPrice", SaleDetail.Quantity*purchaseDetail.Price);
                                    command1.ExecuteNonQuery();
                                }
                                Product.UpdateStock(SaleDetail.Product.Id, Quantity); // Update stock
                            }
                            MessageBox.Show("Sale inserted successfully.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the Sales: " + ex.Message);
                    }
                }
            }
        }

        // Method to reduce the quantity in Purchase based on the quantity sold
        //internal void Update()
        //{
        //    // Make sure SalesId is available
        //    if (this.Product.Id <= 0)
        //    {
        //        MessageBox.Show("Invalid ProductId");
        //        return;
        //    }

        //    string updateQuery = "UPDATE Sales SET Quantity = @Quantity, PaymentMethod = @PaymentMethod, TotalPrice = @TotalPrice,SaleDate = @SaleDate, Price = @Price WHERE SaleId = @SaleId";  // Assuming ProductId is used for identifying the sales entry

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(updateQuery, connection))
        //        {
        //            // Add parameters
        //            command.Parameters.AddWithValue("@ProductId", Product.Id);  // Ensure Product.Id is valid
        //            command.Parameters.AddWithValue("@Price", Price); // Ensure Price is a valid decimal
        //            command.Parameters.AddWithValue("@Quantity", Quantity);  // Ensure Quantity is not null
        //            command.Parameters.AddWithValue("@TotalPrice", TotalPrice); // Ensure TotalPrice is a valid decimal
        //            command.Parameters.AddWithValue("@PaymentMethod", PaymentMethod ?? (object)DBNull.Value); // Handle null values
        //            command.Parameters.AddWithValue("@SaleDate", SaleDate); // Ensure SaleDate is a valid DateTime


        //            try
        //            {
        //                connection.Open();
        //                command.ExecuteNonQuery();


        //                MessageBox.Show("Sale updated successfully.");
        //            }
        //            catch (SqlException ex)
        //            {
        //                MessageBox.Show("SQL Error: " + ex.Message);
        //            }
        //        }
        //    }
        //}

        public void Delete(int SalesId)  // Ensure you are passing the correct SalesId
        {
            if (SalesId <= 0)
            {
                MessageBox.Show("Invalid SalesId. Please select a valid sale.");
                return;
            }

            string query = "DELETE FROM Sale WHERE Id = @SalesId";

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

        public static List<Sale> FetchSales()
        {
            string query = @"SELECT s.Id AS SaleId, s.SaleDate,  pm.Id AS PaymentId, pm.Name AS PaymentMethod FROM Sale s INNER JOIN Payment pm ON pm.Id = s.PaymentId";

            List<Sale> sales = new List<Sale>();
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
                                var Sale = new Sale()
                                {        
                                    Id = Convert.ToInt32(reader["SaleId"]),
                                    SaleDate = reader["SaleDate"] != DBNull.Value ? (DateTime)reader["SaleDate"] : default(DateTime),
                                    Payment = new Payment()
                                    {
                                        Id = Convert.ToInt32(reader["PaymentId"]),
                                        Name = reader["PaymentMethod"].ToString()
                                    }
                                };
                                sales.Add(Sale);
                                // FillSalesDetail(Sales);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error fetching sale: " + ex.Message);
                    }
                }
            }
            return sales;
        }
       
        #endregion
    }
}
//    public static void FillSalesDetail(Purchase purchase)
//    {
//        string query = @"SELECT pd.Id, PurchaseId, pd.ProductId, P.Name as ProductName, pd.Quantity, 
//                pd.Price, pd.TotalPrice From PurchaseDetail pd Inner Join Product p on p.Id = pd.ProductId where pd.PurchaseId = " + purchase.Id;
//        ObservableCollection<PurchaseDetail> PurchaseDetails = new ObservableCollection<PurchaseDetail>();

//        using (SqlConnection connection = new SqlConnection(connectionString))
//        {
//            using (SqlCommand command = new SqlCommand(query, connection))
//            {
//                try
//                {
//                    connection.Open();
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            PurchaseDetails.Add(new PurchaseDetail()
//                            {
//                                Id = Convert.ToInt32(reader["Id"]),
//                                Product = new Product() { Id = Convert.ToInt32(reader["ProductId"]), Name = reader["ProductName"].ToString() },
//                                Quantity = Convert.ToInt32(reader["Quantity"]),
//                                Price = Convert.ToInt32(reader["Price"]),
//                                TotalPrice = Convert.ToInt32(reader["TotalPrice"]),
//                            });
//                        }
//                    }
//                }
//                catch (SqlException ex)
//                {
//                    MessageBox.Show("An error occurred while fetching Purchases: " + ex.Message);
//                }

//            }
//        }

//        purchase.PurchaseDetails = PurchaseDetails;
//    }
//}

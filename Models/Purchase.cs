using MyShop.Common;
using MyShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    internal class Purchase : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Properties

        public Purchase() 
        { 
            PurchaseDetails = new ObservableCollection<PurchaseDetail>(); 
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
        private int _TotalAmount;
        public int TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }

        //private ppplier _Supplier;

        private DateTime _CreationDate = DateTime.Now;
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set
            {
                if (_CreationDate != value)
                {
                    _CreationDate = value;
                    OnPropertyChanged(nameof(CreationDate));
                }
            }
        }

        private ObservableCollection<PurchaseDetail> _PurchaseDetails;
        public ObservableCollection<PurchaseDetail> PurchaseDetails
        {
            get { return _PurchaseDetails; }
            set 
            {
                if (_PurchaseDetails != value)
                {
                    _PurchaseDetails = value;
                    OnPropertyChanged(nameof(PurchaseDetails));
                }
            }
        }


        #endregion
        #region Functions
        public void Insert()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Purchase (CreationDate) VALUES (@CreationDate); SELECT SCOPE_IDENTITY();";//what does scope identity means
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreationDate", CreationDate);

                    try
                    {
                        connection.Open();
                        var newId = command.ExecuteScalar(); // Get the newly inserted Purchase ID

                        if (newId != null)
                        {
                            foreach (var purchaseDetail in PurchaseDetails)
                            {
                                string queryDetail = "INSERT INTO PurchaseDetail (PurchaseId, ProductId, Quantity, Price, TotalPrice) VALUES (@PurchaseId, @ProductId, @Quantity, @Price, @TotalPrice)";
                                using (SqlCommand command1 = new SqlCommand(queryDetail, connection))
                                {
                                    command1.Parameters.AddWithValue("@PurchaseId", newId);
                                    command1.Parameters.AddWithValue("@ProductId", purchaseDetail.Product.Id);
                                    command1.Parameters.AddWithValue("@Quantity", purchaseDetail.Quantity);
                                    command1.Parameters.AddWithValue("@Price", purchaseDetail.Price); 
                                    command1.ExecuteNonQuery();
                                }
                                Product.UpdateStock(purchaseDetail.Product.Id, purchaseDetail.Quantity); // Update stock
                            }
                            MessageBox.Show("Purchase inserted successfully.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the Purchase: " + ex.Message);
                    }
                }
            }
        }
        public List<Purchase> FetchPurchases()
        {
            string query = @"SELECT  p.CreationDate FROM Purchase p";
                  
                      // Adjust table and column names if needed

            List<Purchase> purchases = new List<Purchase>();

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
                                purchases.Add(new Purchase()
                                {
                                   
                                    CreationDate = reader["CreationDate"] != DBNull.Value ? (DateTime)reader["CreationDate"] : default(DateTime),
                                    
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Purchases: " + ex.Message);
                    }
                }
            }

            return purchases;

            #endregion
        }

        // Method to update an existing Purchase
        public void Update()
        {
            
            //string query = "UPDATE Purchase SET Quantity = @Quantity, Price = @Price, CreationDate = @CreationDate, TotalPrice = @TotalPrice WHERE ProductId = @ProductId";

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@ProductId", Product.Id);
            //        command.Parameters.AddWithValue("@Quantity", Quantity);
            //        command.Parameters.AddWithValue("@Price", TotalPrice);
            //        command.Parameters.AddWithValue("@CreationDate", CreationDate);
            //        command.Parameters.AddWithValue("@TotalPrice", TotalPrice);

            //        try
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //            MessageBox.Show("Purchase updated successfully.");
            //        }
            //        catch (SqlException ex)
            //        {
            //            MessageBox.Show("An error occurred: " + ex.Message);
            //        }
            //    }
            //}
        }

        // Method to delete a Purchase
        public void Delete(int ProductId)
        {
            if (ProductId <= 0)
            {
                MessageBox.Show("Invalid ProductId Please select a valid Purchase.");
                return;
            }

            string query = "DELETE FROM Purchase WHERE ProductId = @ProductId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", ProductId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Purchase deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Purchase not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting the Purchase: " + ex.Message);
                    }
                }
            }
        }

        // Method to fetch all Purchases

    }
}
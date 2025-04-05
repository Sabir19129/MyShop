using MyShop.Common;
using MyShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    public class Purchase : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Properties

        public Purchase()
        {
            PurchaseDetails = new ObservableCollection<PurchaseDetail>();
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
        private int _TotalPrice;
        public int TotalPrice
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
        private PurchaseDetail _PurchaseDetail = new PurchaseDetail();
        public PurchaseDetail PurchaseDetail
        {
            get { return _PurchaseDetail; }
            set
            {
                if (_PurchaseDetail != value)
                {
                    _PurchaseDetail = value;
                    OnPropertyChanged(nameof(PurchaseDetail));
                }
            }
        }


        #endregion
        #region Functions
        public void Insert()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Purchase (CreationDate, SupplierId, PaymentId, TotalPrice) VALUES (@CreationDate, @SupplierId, @PaymentId, @TotalPrice); SELECT SCOPE_IDENTITY();";//what does scope identity means
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreationDate", CreationDate);
                    command.Parameters.AddWithValue("@SupplierId", Supplier.Id);
                    command.Parameters.AddWithValue("@PaymentId", Payment.Id);
                    command.Parameters.AddWithValue("@TotalPrice", TotalPrice);
                    try
                    {
                        connection.Open();
                        var newId = command.ExecuteScalar(); // Get the newly inserted Purchase ID

                        if (newId != null)
                        {
                            foreach (var purchaseDetail in PurchaseDetails)
                            {
                                string queryDetail = "INSERT INTO PurchaseDetail (PurchaseId, ProductId, Quantity, Price) VALUES (@PurchaseId, @ProductId, @Quantity, @Price)";
                                using (SqlCommand command1 = new SqlCommand(queryDetail, connection))
                                {
                                    command1.Parameters.AddWithValue("@PurchaseId", newId);
                                    command1.Parameters.AddWithValue("@ProductId", purchaseDetail.Product.Id);
                                    command1.Parameters.AddWithValue("@Quantity", purchaseDetail.Quantity);
                                    command1.Parameters.AddWithValue("@Price", purchaseDetail.Price);
                                    // command1.Parameters.AddWithValue("@TotalPrice", purchaseDetail.Quantity*purchaseDetail.Price);
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
        public static List<Purchase> FetchPurchases()
        {
            string query = @"SELECT 
            p.Id AS PurchaseId, 
            p.CreationDate AS CreationDate, 
            p.TotalPrice AS TotalPrice,
            s.Name AS SupplierName, 
            s.Id AS SupplierId, 
            pm.Name AS Payment, 
            pm.Id AS PaymentId
            FROM
            Purchase p
            INNER JOIN
            Supplier s ON s.Id = p.SupplierId
            INNER JOIN
            Payment pm ON pm.Id = p.PaymentId; ";

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
                                var purchase = new Purchase()
                                {

                                    CreationDate = reader["CreationDate"] != DBNull.Value ? (DateTime)reader["CreationDate"] : default(DateTime),
                                    Supplier = new Supplier() { Id = Convert.ToInt32(reader["SupplierId"]), Name = reader["SupplierName"].ToString() },
                                    Payment = new Payment() { Id = Convert.ToInt32(reader["PaymentId"]), Name = reader["Payment"].ToString() },
                                    TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToInt32(reader["TotalPrice"]) : 0,
                                    Id = Convert.ToInt32(reader["PurchaseId"])
                                };

                                purchases.Add(purchase);
                                FillPurchaseDetail(purchase);
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
        public static void FillPurchaseDetail(Purchase purchase)
        {
            string query = @"SELECT pd.Id, PurchaseId, pd.ProductId, P.Name as ProductName, pd.Quantity, 
                pd.Price, pd.TotalPrice From PurchaseDetail pd Inner Join Product p on p.Id = pd.ProductId where pd.PurchaseId = " + purchase.Id;
            ObservableCollection<PurchaseDetail> PurchaseDetails = new ObservableCollection<PurchaseDetail>();

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
                                PurchaseDetails.Add(new PurchaseDetail()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Product = new Product() { Id = Convert.ToInt32(reader["ProductId"]), Name = reader["ProductName"].ToString() },
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    TotalPrice = Convert.ToInt32(reader["TotalPrice"]),
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

            purchase.PurchaseDetails = PurchaseDetails;
        }
        public void Update(List<int> p_DeletedIds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Update the Purchase table
                string updatePurchaseQuery = @"
            UPDATE Purchase 
            SET CreationDate = @CreationDate, 
                SupplierId = @SupplierId, 
                PaymentId = @PaymentId 
            WHERE Id = @Id";

                using (SqlCommand updatePurchaseCommand = new SqlCommand(updatePurchaseQuery, connection))
                {
                    updatePurchaseCommand.Parameters.AddWithValue("@CreationDate", CreationDate);
                    updatePurchaseCommand.Parameters.AddWithValue("@SupplierId", Supplier.Id);
                    updatePurchaseCommand.Parameters.AddWithValue("@PaymentId", Payment.Id);
                    updatePurchaseCommand.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        updatePurchaseCommand.ExecuteNonQuery();

                        // Step 2: Update each PurchaseDetail
                        foreach (var purchaseDetail in PurchaseDetails)
                        {
                            string updatePurchaseDetailQuery = "";


                            if (purchaseDetail.Id != 0)
                            {
                                updatePurchaseDetailQuery = @"
                        UPDATE PurchaseDetail 
                        SET ProductId = @ProductId, 
                            Quantity = @Quantity, 
                            Price = @Price
                        WHERE Id = @PurchaseDetailId";
                            }
                            else
                            {
                                updatePurchaseDetailQuery = "INSERT INTO PurchaseDetail (PurchaseId, ProductId, Quantity, Price) VALUES (@PurchaseId, @ProductId, @Quantity, @Price)";
                            }

                            using (SqlCommand updatePurchaseDetailCommand = new SqlCommand(updatePurchaseDetailQuery, connection))
                            {
                                updatePurchaseDetailCommand.Parameters.AddWithValue("@PurchaseId", Id);
                                updatePurchaseDetailCommand.Parameters.AddWithValue("@ProductId", purchaseDetail.Product.Id);
                                updatePurchaseDetailCommand.Parameters.AddWithValue("@Quantity", purchaseDetail.Quantity);
                                updatePurchaseDetailCommand.Parameters.AddWithValue("@Price", purchaseDetail.Price);
                                updatePurchaseDetailCommand.Parameters.AddWithValue("@PurchaseDetailId", purchaseDetail.Id);
                                updatePurchaseDetailCommand.ExecuteNonQuery();

                            }
                            using (SqlCommand updatePurchaseDetailCommand = new SqlCommand(updatePurchaseDetailQuery, connection))
                            {
                            }
                        }

                        foreach (var deletedId in p_DeletedIds)
                        {
                            string deletePurchaseQuery = "DELETE FROM PurchaseDetail WHERE Id = @Id";
                            using (SqlCommand deletePurchaseDetailCommand = new SqlCommand(deletePurchaseQuery, connection))
                            {
                                deletePurchaseDetailCommand.Parameters.AddWithValue("@Id", deletedId);
                                deletePurchaseDetailCommand.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Purchase updated successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while updating the Purchase: " + ex.Message);
                    }
        
                }
            }
        }


        public void Delete()
        {
            if (Id <= 0)
            {
                MessageBox.Show("Invalid PurchaseId. Please select a valid Purchase.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Delete PurchaseDetails associated with the Purchase
                string deletePurchaseQuery = "DELETE FROM Purchase WHERE Id = @Id";
                using (SqlCommand deletePurchaseCommand = new SqlCommand(deletePurchaseQuery, connection))
                {
                    deletePurchaseCommand.Parameters.AddWithValue("@Id", Id);
                    try
                    {
                        int purchaseDetailsRowsAffected = deletePurchaseCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting PurchaseDetails: " + ex.Message);
                        return; // Exit if there's an error
                    }
                }
            }
        }

        public void DeletePurchaseDetail(int PurchaseId)
        {
            if (PurchaseId <= 0)
            {
                MessageBox.Show("Invalid PurchaseId. Please select a valid Purchase.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Delete PurchaseDetails associated with the Purchase
                string deletePurchaseDetailsQuery = "DELETE FROM PurchaseDetail WHERE PurchaseId = @PurchaseId";
                using (SqlCommand deletePurchaseDetailsCommand = new SqlCommand(deletePurchaseDetailsQuery, connection))
                {
                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@PurchaseId", PurchaseId);
                    try
                    {
                        int purchaseDetailsRowsAffected = deletePurchaseDetailsCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting PurchaseDetails: " + ex.Message);
                        return; // Exit if there's an error
                    }
                }

 
            }
        }

        // Method to fetch all Purchases

    }
}
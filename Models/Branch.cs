using MyShop.Common;
using MyShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    public class Branch : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";
        #region Properties

        public Branch()
        {
            BranchDetails = new ObservableCollection<BranchDetail>();
        }
        private ObservableCollection<BranchDetail> _BranchDetails;
        public ObservableCollection<BranchDetail> BranchDetails
        {
            get { return _BranchDetails; }
            set
            {
                if (_BranchDetails != value)
                {
                    _BranchDetails = value;
                    OnPropertyChanged(nameof(BranchDetails));
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

        private string _City;
        public string City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged(nameof(City));
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
        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                if (_StartDate != value)
                {
                    _StartDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
    
       
        private BranchDetail _BranchDetail = new BranchDetail();
        public BranchDetail BranchDetail
        {
            get { return _BranchDetail; }
            set
            {
                if (_BranchDetail != value)
                {
                    _BranchDetail = value;
                    OnPropertyChanged(nameof(BranchDetail));
                }
            }
        }


        #endregion
        #region Functions
        public void Insert()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Branch (StartDate, Name, Address, Value) VALUES (@StartDate, @Name, @Address, @Value); SELECT SCOPE_IDENTITY();";//what does scope identity means
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (StartDate < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                    {
                        throw new InvalidOperationException("Invalid Start Date. Please enter a correct date.");
                    }

                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Value", Value);
                    try
                    {
                        connection.Open();
                        var newId = command.ExecuteScalar(); // Get the newly inserted Branch ID

                        if (newId != null)
                        {
                            foreach (var BranchDetail in BranchDetails)
                            {
                                

                                string queryDetail = "INSERT INTO BranchDetail (StartTime, EndTime, NoOfEmployee, Feedback) " +
                     "VALUES (@StartTime, @EndTime, @NoOfEmployee, @Feedback)";

                                using (SqlCommand command1 = new SqlCommand(queryDetail, connection))
                                {
                                   
                                    command1.Parameters.AddWithValue("@StartTime", BranchDetail.StartTime);
                                    command1.Parameters.AddWithValue("@EndTime", BranchDetail.EndTime);
                                    command1.Parameters.AddWithValue("@NoOfEmployee", BranchDetail.NoOfEmployee);
                                    command1.Parameters.AddWithValue("@Feedback", BranchDetail.Feedback);
                                    command1.ExecuteNonQuery();
                                }

                            }
                            MessageBox.Show("Branch inserted successfully.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while inserting the Branch: " + ex.Message);
                    }
                }
            }
        }
        public static List<Branch> FetchBranchs()
        {
            string query = "SELECT Id, StartDate, Name, Address,Value FROM Branch";

            List<Branch> Branchs = new List<Branch>();

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
                                var branch = new Branch()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    StartDate = reader["StartDate"] != DBNull.Value ? (DateTime)reader["StartDate"] : default(DateTime),
                                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : string.Empty,
                                    Value = reader["Value"] != DBNull.Value ? Convert.ToInt32(reader["Value"]) : 0
                                };

                                Branchs.Add(branch);
                                FillBranchDetail(branch);

                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Branchs: " + ex.Message);
                    }

                }
            }

            return Branchs;

            #endregion
        }
        public static void FillBranchDetail(Branch Branch)
        {
            string query = @"SELECT Id, StartTime, EndTime, NoOfEmployee, Feedback 
FROM BranchDetail 
WHERE Id = " + Branch.Id;


            ObservableCollection<BranchDetail> BranchDetails = new ObservableCollection<BranchDetail>();

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
                                BranchDetails.Add(new BranchDetail()
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    StartTime = reader["StartTime"] != DBNull.Value ? reader["StartTime"].ToString() : string.Empty,
                                    EndTime = reader["EndTime"] != DBNull.Value ? reader["EndTime"].ToString() : string.Empty,
                                    NoOfEmployee = reader["NoOfEmployee"] != DBNull.Value ? Convert.ToInt32(reader["NoOfEmployee"].ToString()) : 0,
                                    Feedback = reader["Feedback"] != DBNull.Value ? reader["Feedback"].ToString() : string.Empty,

                                });

                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while fetching Branchs: " + ex.Message);
                    }

                }
            }

            Branch.BranchDetails = BranchDetails;
        }
        public void Update(List<int> p_DeletedIds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Update the Branch table
                string updateBranchQuery = @"
            UPDATE Branch 
            SET StartDate = @StartDate, 
                Name = @Name, 
                Address = @Address, 
                Value = @ValueId, 
            WHERE Id = @Id";

                using (SqlCommand updateBranchCommand = new SqlCommand(updateBranchQuery, connection))
                {
                    updateBranchCommand.Parameters.AddWithValue("@StartDate", StartDate);
                    updateBranchCommand.Parameters.AddWithValue("@Name", Name);
                    updateBranchCommand.Parameters.AddWithValue("@Address", Address);
                    updateBranchCommand.Parameters.AddWithValue("@Value", Value);
                    updateBranchCommand.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        updateBranchCommand.ExecuteNonQuery();

                        // Step 2: Update each BranchDetail
                        foreach (var BranchDetail in BranchDetails)
                        {
                            string updateBranchDetailQuery = "";


                            if (BranchDetail.Id != 0)
                            {
                                updateBranchDetailQuery = @"
                        UPDATE BranchDetail 
                        SET StartTime = @StartTime, 
                            EndTime = @EndTime, 
                            NoOfEmployee = @NoOfEmployee,
                            FeedBack = @FeedBack,
                        WHERE Id = @BranchDetailId";
                            }
                            else
                            {
                                updateBranchDetailQuery = "INSERT INTO BranchDetail (Id,,) VALUES (@Id,,,)";
                            }

                            using (SqlCommand updateBranchDetailCommand = new SqlCommand(updateBranchDetailQuery, connection))
                            {
                                updateBranchDetailCommand.Parameters.AddWithValue("@Id", Id);
                                
                                updateBranchDetailCommand.Parameters.AddWithValue("@BranchDetailId", BranchDetail.Id);
                                updateBranchDetailCommand.ExecuteNonQuery();

                            }
                            using (SqlCommand updateBranchDetailCommand = new SqlCommand(updateBranchDetailQuery, connection))
                            {
                            }
                        }

                        foreach (var deletedId in p_DeletedIds)
                        {
                            string deleteBranchQuery = "DELETE FROM BranchDetail WHERE Id = @Id";
                            using (SqlCommand deleteBranchDetailCommand = new SqlCommand(deleteBranchQuery, connection))
                            {
                                deleteBranchDetailCommand.Parameters.AddWithValue("@Id", deletedId);
                                deleteBranchDetailCommand.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Branch updated successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while updating the Branch: " + ex.Message);
                    }

                }
            }
        }


        public void Delete()
        {
            if (Id <= 0)
            {
                MessageBox.Show("Invalid Id. Please select a valid Branch.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Delete BranchDetails associated with the Branch
                string deleteBranchQuery = "DELETE FROM Branch WHERE Id = @Id";
                using (SqlCommand deleteBranchCommand = new SqlCommand(deleteBranchQuery, connection))
                {
                    deleteBranchCommand.Parameters.AddWithValue("@Id", Id);
                    try
                    {
                        int BranchDetailsRowsAffected = deleteBranchCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting BranchDetails: " + ex.Message);
                        return; // Exit if there's an error
                    }
                }
            }
        }

        public void DeleteBranchDetail(int Id)
        {
            if (Id <= 0)
            {
                MessageBox.Show("Invalid Id. Please select a valid Branch.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Delete BranchDetails associated with the Branch
                string deleteBranchDetailsQuery = "DELETE FROM BranchDetail WHERE Id = @Id";
                using (SqlCommand deleteBranchDetailsCommand = new SqlCommand(deleteBranchDetailsQuery, connection))
                {
                    deleteBranchDetailsCommand.Parameters.AddWithValue("@Id", Id);
                    try
                    {
                        int BranchDetailsRowsAffected = deleteBranchDetailsCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred while deleting BranchDetails: " + ex.Message);
                        return; // Exit if there's an error
                    }
                }


            }
        }

        // Method to fetch all Branchs

    }
}
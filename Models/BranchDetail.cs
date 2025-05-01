using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyShop.Models
{
    public class BranchDetail : BindableBase
    {
        private static string connectionString = "Data Source=SABIR\\SQLEXPRESS01;Initial Catalog=MyShopDb;Integrated Security=True";

        public event EventHandler QuantityChanged;


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
        private string _Feedback;
        public string Feedback
        {
            get { return _Feedback; }
            set
            {
                if (_Feedback != value)
                {
                    _Feedback = value;
                    OnPropertyChanged(nameof(Feedback));
                }
            }
        }
        private String _StartTime;
        public String StartTime
        {
            get { return _StartTime; }
            set
            {
                if (_StartTime != value)
                {
                    _StartTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }
        private String _EndTime;
        public String EndTime
        {
            get { return _EndTime; }
            set
            {
                if (_EndTime != value)
                {
                    _EndTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }
        private String _Timings;
        public String Timings
        {
            get { return _Timings; }
            set
            {
                if (_Timings != value)
                {
                    _Timings = value;
                    OnPropertyChanged(nameof(Timings));
                }
            }
        }
        private int _NoOfEmployee;
        public int NoOfEmployee
        {
            get { return _NoOfEmployee; }
            set
            {
                if (_NoOfEmployee != value)
                {
                    _NoOfEmployee = value;
                    OnPropertyChanged(nameof(NoOfEmployee));
                }
            }
        }
    }
}

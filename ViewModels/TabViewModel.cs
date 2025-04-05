using MyShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModels
{
    public class TabViewModel : BindableBase
    {
       
        private string _TabHeading;
        public string TabHeading
        {
            get { return _TabHeading; }
            set
            {
                if (_TabHeading != value)
                {
                    _TabHeading = value;
                    OnPropertyChanged(nameof(TabHeading));  // Notify UI to refresh visibility
                }
            }
        }
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }
    }

}

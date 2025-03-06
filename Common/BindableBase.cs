using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyShop.Common
{
    public class BindableBase : INotifyPropertyChanged// The INotifyPropertyChanged interface defines an event (PropertyChanged) that
                                                      // notifies clients when a property value changes,
    {
        public event PropertyChangedEventHandler PropertyChanged;

        

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}


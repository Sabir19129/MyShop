using MyShop.ViewModels;  // Ensure you have this using directive
using MyShop.Views;       // Ensure you have this using directive
using System.Windows;

namespace MyShop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        PaymentView paymentView = new PaymentView
        {
            DataContext = new PaymentViewModel() // Ensure ViewModel is set
        };



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserView userView = new UserView();
            UserViewModel userViewModel = new UserViewModel();
            userView.DataContext = userViewModel;
            userView.ShowDialog();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProductView productView = new ProductView();
            ProductViewModel productViewModel = new ProductViewModel();
            productView.DataContext = productViewModel;
            productView.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Make sure to instantiate the PurchaseView and ViewModel
            PurchaseView purchaseView = new PurchaseView();
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            purchaseView.DataContext = purchaseViewModel; // Set DataContext for data binding
            purchaseView.ShowDialog(); // Open PurchaseView as a dialog
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Make sure to instantiate the PurchaseView and ViewModel
            SaleView SaleView = new SaleView();
            SaleViewModel SaleViewModel = new SaleViewModel();
            SaleView.DataContext = SaleViewModel; // Set DataContext for data binding
            SaleView.ShowDialog(); // Open PurchaseView as a dialog
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Make sure to instantiate the SuppliersView and ViewModel
            SupplierView SupplierView = new SupplierView();
            SupplierViewModel SupplierViewModel = new SupplierViewModel();
            SupplierView.DataContext = SupplierViewModel; // Set DataContext for data binding
            SupplierView.ShowDialog(); // SuppliersView as a dialog
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            // Make sure to instantiate the PaymentView and ViewModel
            PaymentView PaymentView = new PaymentView();
            PaymentViewModel PaymentViewModel = new PaymentViewModel();
            PaymentView.DataContext = PaymentViewModel; // Set DataContext for data binding
            PaymentView.ShowDialog(); // SuppliersView as a dialog
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            CustomerView CustomerView = new CustomerView();
            CustomerViewModel CustomerViewModel = new CustomerViewModel();
            CustomerView.DataContext = CustomerViewModel;
            CustomerView.ShowDialog();
        }
    }
}

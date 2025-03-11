using MyShop.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for PurchaseView.xaml
    /// </summary>
    public partial class PurchaseView : Window
    {
        public PurchaseView()
        {
            InitializeComponent();
            DataContext = new PurchaseViewModel(); // Set the DataContext here
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Get the current caret position
                int caretIndex = textBox.CaretIndex;

                // Capitalize the first letter of each word
                string[] words = textBox.Text.ToLower().Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length > 0)
                    {
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                    }
                }

                // Join the words back into a single string
                textBox.Text = string.Join(" ", words);

                // Restore the caret position
                textBox.CaretIndex = caretIndex;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

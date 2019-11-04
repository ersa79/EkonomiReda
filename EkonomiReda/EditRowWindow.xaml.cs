using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using EkonomiReda.src;
using System.Windows.Controls.Primitives;

namespace EkonomiReda
{
    /// <summary>
    /// Interaction logic for EditRowWindow.xaml
    /// </summary>
    public partial class EditRowWindow : Window
    {

        private CsvRow CsvRow { get;  set; }
        public Selector Selector;

        public EditRowWindow(CsvRow csvRow)
        {
            InitializeComponent();
            CsvRow = csvRow;
            Transaction_TextBox.Text = csvRow.Transaction;
            Category_TextBox.Text = csvRow.Category;
            SubCategory_ListBox.Items.Add(csvRow.Category);
        }
 
        private void SaveRow_Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: if any changes, validate and make sure changes are saved
            DialogResult = true;
        }

        private void CancelRow_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
     }
}

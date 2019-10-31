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


        //public EditRowWindow()
        //{
        //    InitializeComponent();
        //    //CsvRow = csvRow;
        //}

        public EditRowWindow(CsvRow csvRow)
        {
            InitializeComponent();
            CsvRow = csvRow;
            Transaction_TextBox.Text = csvRow.Transaction;
            Category_TextBox.Text = csvRow.Category;
            SubCategory_ListBox.Items.Add(csvRow.Category);


        }

        //public EditRowWindow(Selector csvRow)
        //{
        //    InitializeComponent();
        //    Selector = csvRow;
        //}

        private void SaveRow_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelRow_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //public EditRowWindow(CsvRow csvRow)
        //{
        //    InitializeComponent();
        //}
    }
}

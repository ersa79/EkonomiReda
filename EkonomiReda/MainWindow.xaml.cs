using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EkonomiReda.src;
using System.Collections.ObjectModel;

namespace EkonomiReda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveToDB()
        {
            //TODO: Implement
        }

        private void ReadFromDB()
        {
            //TODO: Implement
        }

        //private void OpenFileDialog

        private void OpenFileAction()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                string filename = dlg.FileName;
                // Open document                
                CurrentOpenFile_TextBlock.Text = dlg.FileName;
                ObservableCollection<CsvRow> csvRowCollection = Utils.ReadFileToCsvRowCollection(filename, ",", Encoding.UTF8);
                CsvGrid.ItemsSource = csvRowCollection;
            }
        }


        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileAction();
        }

        private void SaveCsv_Button_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            string filename = "";
            if (CsvGrid.ItemsSource != null)
            {
                ObservableCollection<CsvRow> itemRows = (ObservableCollection<CsvRow>)CsvGrid.ItemsSource;
                List<string> itemRowsAsStrings = new List<string>();
                
                //Console.WriteLine("Reading rows from DataGrid:");
                foreach (CsvRow row in itemRows)
                {
                    itemRowsAsStrings.Add(row.ToString());
                }

                filename = Utils.SaveToLocation();
                if (filename == "")
                {
                    Console.WriteLine("Not a valid filename: " + filename);
                }
                else
                {
                    Console.WriteLine("Save filename: " + filename);
                    result = Utils.SaveFile(filename, itemRowsAsStrings, Encoding.UTF8);
                }
            }

            if (result)
            {
                Console.WriteLine("File '" + filename + "' saved succesfully");
            }
        }

        private void OpenCsv_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileAction();
        }

        private void ViewWholeYear_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CsvGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CsvGrid.SelectedItem == null) return;
            var selectedRow = CsvGrid.SelectedItem as CsvRow;
            EditRowWindow editPersonWindow = new EditRowWindow(CsvGrid.SelectedItem as CsvRow);
            //EditRowWindow editPersonWindow = new EditRowWindow(&CsvGrid.SelectedItem);

            //EditRowWindow editPersonWindow = new EditRowWindow
            //{
            //    //editPersonWindow.bind
            //    Owner = this

            //};
            editPersonWindow.ShowDialog();

            //if(editPersonWindow.DialogResult = 
        }

        private void CsvGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if((e.Column.Header.ToString() == "Date") ||
               (e.Column.Header.ToString() == "Amount") ||
               (e.Column.Header.ToString() == "Balance"))
            {
                e.Column.IsReadOnly = true;
            }
        }
    }
}

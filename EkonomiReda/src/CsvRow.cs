namespace EkonomiReda.src
{
    public class CsvRow
    {
        public string Date { get; set; }
        public string Transaction { get; set; }
        public string Category { get; set; }
        public string Amount { get; set; }
        public string Balance { get; set; }        
        public string SubCategory { get; set; }

        //public CsvRow()
        //{
        //    Date = "";
        //    Transaction = "";
        //    Category = "";
        //    Amount = "";
        //    Balance = "";
        //    SubCategory = "";
        //}

        public CsvRow(RawDataRow dataRow)
        {
            Date = dataRow.GetDate();
            Transaction = dataRow.GetTransaction();
            Category = dataRow.GetCategory();
            Amount = dataRow.GetAmount();
            Balance = dataRow.GetBalance();
            SubCategory = "Unset";

        }

        public CsvRow(string[] items)
        {
            Date = items[0];
            Transaction = items[1];
            Category = items[2];
            Amount = items[3];
            Balance = items[4];
            SubCategory = "Unset";
        }

        public override string ToString()
        {
            return Date + "," + Transaction + "," + Category + "," + SubCategory + ",\"" + Amount + "\",\"" + Balance + "\"";
        }

    }
}

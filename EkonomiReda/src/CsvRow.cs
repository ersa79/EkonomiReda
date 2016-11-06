using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkonomiReda.src
{
    class CsvRow
    {
        public string date { get; set; }
        public string transaction { get; set; }
        public string category { get; set; }
        public string amount { get; set; }
        public string balance { get; set; }        

        public CsvRow(string[] items)
        {
            date = items[0];
            transaction = items[1];
            category = items[2];
            amount = items[3];
            balance = items[4];
        }

        public override string ToString()
        {
            return date + "," + transaction + "," + category + ",\"" + amount + "\",\"" + balance + "\"";
        }

    }
}

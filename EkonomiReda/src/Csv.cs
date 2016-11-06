using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkonomiReda.src
{
    class Csv
    {
        List<BillRow> Rows;        
        
        public Csv(List<BillRow> rows)
        {
            this.Rows = rows;            
        }

        override public string ToString()
        {
            string csvToString = "";
            foreach (BillRow row in Rows)
            {
                csvToString += row.ToString() + "\n";
            }
            return csvToString;
        }

        public int NumberOfRows()
        {
            return Rows.Count;
        }

        public List<BillRow> GetRows()
        {
            return Rows;
        }
    }
}

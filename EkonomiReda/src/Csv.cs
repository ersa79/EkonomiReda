using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EkonomiReda.src
{
    class Csv
    {
        private List<RawDataRow> Rows;        
        
        public Csv(List<RawDataRow> rows)
        {
            this.Rows = rows;            
        }

        override public string ToString()
        {
            string csvToString = "";
            foreach (RawDataRow row in Rows)
            {
                csvToString += row.ToString(";") + "\n";
            }
            return csvToString;
        }

        public int NumberOfRows()
        {
            return Rows.Count;
        }

        public List<RawDataRow> GetRows()
        {
            return Rows;
        }
    }
}

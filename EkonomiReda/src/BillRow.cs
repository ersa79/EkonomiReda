using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EkonomiReda.src
{
    public class RawDataRow : IEquatable<RawDataRow>
    {
        protected string[] DataArray = new string[5];
        private bool DataHeader = false;

        public RawDataRow()
        {
            DataArray.SetValue("", 0);
            this.DataArray.SetValue("", 1);
            this.DataArray.SetValue("", 2);
            this.DataArray.SetValue("", 3);
            this.DataArray.SetValue("", 4);
            this.DataHeader = false;
        }


        public RawDataRow(string date, string transaction, string category, string amount, string balance, bool header)
        {
            this.DataArray.SetValue(date, 0);
            this.DataArray.SetValue(transaction, 1);
            this.DataArray.SetValue(category, 2);
            this.DataArray.SetValue(amount, 3);
            this.DataArray.SetValue(balance, 4);
            this.DataHeader = header;
        }

        public RawDataRow(string[] items, bool header)
        {
            if (items.Length != 5)
            {
                Console.WriteLine("Wrong format of row");
                throw new FormatException("Wrong format of input string[]. Length must be 5");
            }

            this.DataHeader = header;            
            int index = 0;
            foreach (string str in items)
            {
                DataArray.SetValue(str, index);
                index++;
            }
        }

        public string GetDate()        { return DataArray[0]; }
        public string GetTransaction() { return DataArray[1]; }
        public string GetCategory()    { return DataArray[2]; }
        public string GetAmount()      { return DataArray[3]; }
        public string GetBalance()     { return DataArray[4]; }
        public bool IsHeader() { return DataHeader; }
        public string[] GetDataArray() { return DataArray; }
     
        public Decimal GetDecimalAmount()
        {
            return Decimal.Parse(DataArray[3], NumberStyles.Currency);
        }

        public bool Equals(RawDataRow other)
        {
            return new DataRowSameStrings().Equals(this, other);            
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string ToString(string delimiter = ", ")
        {
            return DataArray[0] + delimiter + DataArray[1] + delimiter + DataArray[2] + delimiter + DataArray[3] + delimiter + DataArray[4];
        }       
    }

    //Comparer for DataRow, compares all strings
    public class DataRowSameStrings : EqualityComparer<RawDataRow>
    {
        public override bool Equals(RawDataRow x, RawDataRow y)
        {
            if (x.GetDate().Equals(y.GetDate()) &&
                x.GetTransaction().Equals(y.GetTransaction()) &&
                x.GetCategory().Equals(y.GetCategory()) &&
                x.GetAmount().Equals(y.GetAmount()) &&
                x.GetBalance().Equals(y.GetBalance()))
            {
                return true;
            }
            else
            {
                return false;
            }                
        }

        public override int GetHashCode(RawDataRow obj)
        {
            int hcode = 0;
            foreach (string str in obj.GetDataArray())
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(str);
                foreach (byte b in asciiBytes)
                {
                    hcode += b;
                }
            }
            return hcode;
        }
    }

    //DataRowCollection
    public class DataRowCollection : ICollection<RawDataRow>
    {
        private List<RawDataRow> dataRowList;

        public DataRowCollection()
        {
            dataRowList = new List<RawDataRow>();
        }

        //Needed for ability to index a DataRowCollection
        //Ex. 
        //DataRowCollection brc = new DataRowCollection();
        //DataRow br = brc[i]; 
        public RawDataRow this[int index]
        {
            get { return (RawDataRow)dataRowList[index]; }
            set { dataRowList[index] = value; }
        }

        public void Add(RawDataRow item)
        {
            if (!Contains(item))
            {
                dataRowList.Add(item);
            }
            else
            {
                Console.WriteLine("DataRowCollection.Add: List already contains equal DataRow item:\n" + item.ToString());
            }
        }

        public void Clear()
        {
            dataRowList.Clear();
        }

        public bool Contains(RawDataRow item)
        {
            foreach (RawDataRow row in dataRowList)
            {
                if (row.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(RawDataRow[] dataArray, int arrayIndex)
        {
            if (dataArray == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > dataArray.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < dataRowList.Count; i++)
            {
                dataArray[i + arrayIndex] = dataRowList[i];
            }
        }

        public int Count
        {
            get { return dataRowList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(RawDataRow item)
        {
            //TODO: might need to use for loop and RemoveAt(i)
            foreach(RawDataRow row in dataRowList)
            {
                if(row.Equals(item))
                {
                    dataRowList.Remove(item);
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<RawDataRow> GetEnumerator()
        {
            return new DataRowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DataRowEnumerator(this);
        }

        
        public override string ToString()
        {
            string str = ""; 
            foreach (RawDataRow billRow in dataRowList)
            {
                str += billRow.ToString();
            }
            return str;
        }
    }

    // Enumerator
    public class DataRowEnumerator : IEnumerator<RawDataRow>
    {
        private DataRowCollection dataRowCollection;
        private int curIndex;
        private RawDataRow curDataRow;

        public DataRowEnumerator(DataRowCollection collection)
        {
            dataRowCollection = collection;
            curIndex = -1;
            curDataRow = default(RawDataRow);
        }

        public bool MoveNext()
        {
            if (++curIndex >= dataRowCollection.Count)
            {
                return false;
            }
            else
            {
                curDataRow = dataRowCollection[curIndex];                
            }
            return true;
        }

        public RawDataRow Current
        {
            get { return curDataRow; }
        }

        public void Dispose()
        {
            curIndex = -1;
            curDataRow = default(RawDataRow);
            throw new NotImplementedException();
            
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Reset() { curIndex = -1; curDataRow = default(RawDataRow); }
    }

   

}
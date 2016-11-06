using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EkonomiReda.src
{
    public class BillRow : IEquatable<BillRow>
    {
        private string[] BillArray = new string[5];
        private bool BillHeader;

        public BillRow(string date, string transaction, string category, string amount, string balance, bool header)
        {
            this.BillArray.SetValue(date, 0);
            this.BillArray.SetValue(transaction, 1);
            this.BillArray.SetValue(category, 2);
            this.BillArray.SetValue(amount, 3);
            this.BillArray.SetValue(balance, 4);
            this.BillHeader = header;
        }

        public BillRow(string[] items, bool header)
        {
            if (items.Length != 5)
            {
                Console.WriteLine("Wrong format of row");
                throw new FormatException("Wrong format of input string[]. Length must be 5");
            }

            this.BillHeader = header;            
            int index = 0;
            foreach (string str in items)
            {
                BillArray.SetValue(str, index);
                index++;
            }
        }

        public string GetDate()        { return BillArray[0]; }
        public string GetTransaction() { return BillArray[1]; }
        public string GetCategory()    { return BillArray[2]; }
        public string GetAmount()      { return BillArray[3]; }
        public string GetBalance()     { return BillArray[4]; }
        public bool IsHeader() { return BillHeader; }
        public string[] GetBillArray() { return BillArray; }
     
        public Decimal GetDecimalAmount()
        {
            return Decimal.Parse(BillArray[3], NumberStyles.Currency);
        }

        public bool Equals(BillRow other)
        {
            return new BillRowSameStrings().Equals(this, other);            
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return BillArray[0] + ", " + BillArray[1] + ", " + BillArray[2] + ", " + BillArray[3] + ", " + BillArray[4];
        }       
    }

    //Comparer for BillRow, compares all strings
    public class BillRowSameStrings : EqualityComparer<BillRow>
    {
        public override bool Equals(BillRow x, BillRow y)
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

        public override int GetHashCode(BillRow obj)
        {
            int hcode = 0;
            foreach (string str in obj.GetBillArray())
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

    //BillRowCollection
    public class BillRowCollection : ICollection<BillRow>
    {
        private List<BillRow> billRowList;

        public BillRowCollection()
        {
            billRowList = new List<BillRow>();
        }

        //Needed for ability to index a BillRowCollection
        //Ex. 
        //BillRowCollection brc = new BillRowCollection();
        //BillRow br = brc[i]; 
        public BillRow this[int index]
        {
            get { return (BillRow)billRowList[index]; }
            set { billRowList[index] = value; }
        }

        public void Add(BillRow item)
        {
            if (!Contains(item))
            {
                billRowList.Add(item);
            }
            else
            {
                Console.WriteLine("BillRowCollection.Add: List already contains equal BillRow item:\n" + item.ToString());
            }
        }

        public void Clear()
        {
            billRowList.Clear();
        }

        public bool Contains(BillRow item)
        {
            foreach (BillRow billRow in billRowList)
            {
                if (billRow.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(BillRow[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < billRowList.Count; i++)
            {
                array[i + arrayIndex] = billRowList[i];
            }
        }

        public int Count
        {
            get { return billRowList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(BillRow item)
        {
            //TODO: might need to use for loop and RemoveAt(i)
            foreach(BillRow billRow in billRowList)
            {
                if(billRow.Equals(item))
                {
                    billRowList.Remove(item);
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<BillRow> GetEnumerator()
        {
            return new BillRowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BillRowEnumerator(this);
        }

        
        public override string ToString()
        {
            string str = ""; 
            foreach (BillRow billRow in billRowList)
            {
                str += billRow.ToString();
            }
            return str;
        }
    }

    // Enumerator
    public class BillRowEnumerator : IEnumerator<BillRow>
    {
        private BillRowCollection billRowCollection;
        private int curIndex;
        private BillRow curBillRow;

        public BillRowEnumerator(BillRowCollection collection)
        {
            billRowCollection = collection;
            curIndex = -1;
            curBillRow = default(BillRow);
        }

        public bool MoveNext()
        {
            if (++curIndex >= billRowCollection.Count)
            {
                return false;
            }
            else
            {
                curBillRow = billRowCollection[curIndex];                
            }
            return true;
        }

        public BillRow Current
        {
            get { return curBillRow; }
        }

        public void Dispose()
        {
            curIndex = -1;
            curBillRow = default(BillRow);
            throw new NotImplementedException();
            
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Reset() { curIndex = -1; curBillRow = default(BillRow); }
    }

   

}
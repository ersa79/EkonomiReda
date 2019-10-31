using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using System.IO;

namespace EkonomiReda.src
{
    static class Utils
    {
        static readonly string INSURANCE = "Insurance";
        static readonly string PHONETVBROADBAND = "PhoneTvBroadband";
        static readonly string FOOD = "Food";
        static readonly string UNCATEGORIZED = "Uncategorized";
        static readonly string ELECTRICAL = "Electrical";
        static readonly string HOUSE = "House";
        static readonly string PETROL = "Petrol";
        static readonly string LOAN = "Loan";
        static readonly string CLOTHES = "Clothes";

        public static ObservableCollection<CsvRow> ReadFileToCsvRowCollection(string filename, string delimiter, Encoding encoding)
        {
            ObservableCollection<CsvRow> csvRowCollection = new ObservableCollection<CsvRow>();
            using (TextFieldParser parser = new TextFieldParser(filename, encoding))
            {
                parser.Delimiters = new string[] { delimiter };
                
                while (true)
                {
                    string[] parts = parser.ReadFields();
                    if (parts == null)
                    {
                        break;
                    }
                    csvRowCollection.Add(new CsvRow(parts));

                }
             }
            return csvRowCollection;
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        public static string SaveToLocation()
        {            
            Microsoft.Win32.SaveFileDialog saveDiag = new Microsoft.Win32.SaveFileDialog();
            saveDiag.DefaultExt = ".csv";
            string location = "";
            // Show save file dialog box
            Nullable<bool> result = saveDiag.ShowDialog();
            
            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                location = saveDiag.FileName;
            } 
            else if (result == false)
            {
                location = "Cancel button pressed";
            }

            return location;
        }

        

        public static bool SaveFile(string filename, List<string> rowList, Encoding encoding)
        {
            bool result = true;
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename, false, encoding))
                {
                    foreach (string line in rowList)
                    {
                        file.WriteLine(line);
                    }                    
                }
            }
            catch (Exception e)
            {
                result = false;
                Console.WriteLine(e.Message);                
            }
            return result;
        }

    
        public static Csv ReadFile(string filename)
        {
            Csv csvFile;
            List<RawDataRow> billRows = new List<RawDataRow>();
            //CsvHeader csvHeader = null;                      

            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.Delimiters = new string[] { "," };
                while (true)
                {
                    string[] parts = parser.ReadFields();
                    if (parts == null)
                    {
                        break;
                    }                    
                    billRows.Add(new RawDataRow(parts, false));                    
                }
                csvFile = new Csv(billRows);                
            }

            Dictionary<string, Decimal> categorizedDecimalCsv = Utils.CategorizeCsvDecimal(csvFile);
            foreach (KeyValuePair<string, Decimal> pair in categorizedDecimalCsv)
            {
                Console.WriteLine(pair.Key + ": " + pair.Value);

            }
            return csvFile;

        }


        public static Dictionary<string, Decimal> CategorizeCsvDecimal(Csv csvFile)
        {
            Dictionary<string, Decimal> dictionary = new Dictionary<string, Decimal>();
            Dictionary<string, Regex> regexList = new Dictionary<string, Regex>(); //TODO: createRegexList method?
            regexList.Add(PHONETVBROADBAND, new Regex("TELIA", RegexOptions.IgnoreCase));
            regexList.Add(INSURANCE, new Regex("FÖRENADE|FOLKSAM|UNIONEN|SKANDIA", RegexOptions.IgnoreCase));
            regexList.Add(FOOD, new Regex("ICA|COOP|KONSUM|WILLY|MAT", RegexOptions.IgnoreCase));
            regexList.Add(PETROL, new Regex("QSTAR|Q8|CIRKLE", RegexOptions.IgnoreCase));
            regexList.Add(LOAN, new Regex("CENTRALA STUDIE|TOYOTA KRED|Huslån", RegexOptions.IgnoreCase));
            regexList.Add(HOUSE, new Regex("BYGGMAX|JULA", RegexOptions.IgnoreCase));
            regexList.Add(ELECTRICAL, new Regex("VATTENFALL|UMEÅ ENERGI|UMEA ENERGI", RegexOptions.IgnoreCase));
            regexList.Add(CLOTHES, new Regex("ELLOS|HM|H&M|BON PRIX", RegexOptions.IgnoreCase));


            foreach (RawDataRow row in csvFile.GetRows())
            {
                Decimal amount;
                try
                {
                    amount = row.GetDecimalAmount();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not parse row amount to double or row amount to Decimal \n", e.Message);
                    break;
                }
                string transaction = row.GetTransaction();
                bool foundCategory = false;

                foreach (KeyValuePair<string, Regex> pair in regexList)
                {
                    if (!foundCategory)
                    {
                        foundCategory = AddCategoryDecimal(pair.Key, pair.Value, transaction, amount, dictionary);
                    }
                }

                if (!foundCategory)
                {
                    AddCategoryDecimal(UNCATEGORIZED, amount, dictionary);
                }
            }

            return dictionary;
        }


        public static bool AddCategoryDecimal(string category, Regex categoryRegex, string transaction, Decimal amount, Dictionary<string, Decimal> dict)
        {
            if (!categoryRegex.IsMatch(transaction))
            {
                return false;
            }

            AddCategoryDecimal(category, amount, dict);
            return true;
        }


        private static void AddCategoryDecimal(string category, Decimal amount, Dictionary<string, Decimal> dict)
        {
            if (dict.Keys.Contains(category))
            {
                dict[category] += amount;
                Console.WriteLine("Added {0} to Category {1}", amount, category);
            }
            else
            {
                dict.Add(category, amount);
                Console.WriteLine("Added new Category {0} with value {1}", category, amount);
            }
        }

    }
}

using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbManager
{
    public class Row
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public List<string> Values { get; set; }

        public Row(List<ColumnDefinition> columnDefinitions, List<string> values)
        {
            //TODO DEADLINE 1.A: Initialize member variables
            this.ColumnDefinitions = columnDefinitions;
            this.Values = values;

        }

        public void SetValue(string columnName, string value)
        {
            //TODO DEADLINE 1.A: Given a column name and value, change the value in that column
            var column = this.ColumnDefinitions.FirstOrDefault(col => col.Name == columnName);
            if (column != null)
            {
                int index = this.ColumnDefinitions.IndexOf(column);
                this.Values[index] = value;
            }


        }

        public string GetValue(string columnName)
        {
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            var column = this.ColumnDefinitions.FirstOrDefault(col => col.Name == columnName);
            if (column != null)
            {
                int index = this.ColumnDefinitions.IndexOf(column);
                    return (Values[index]);
            }


            return null;
            
        }

        public bool IsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Given a condition (column name, operator and literal value, return whether it is true or not
            //for this row. Check Condition.IsTrue method
            var colName = condition.ColumnName;
            var valueColumn = GetValue(colName);
            var columnDef = this.ColumnDefinitions.FirstOrDefault(col => col.Name == colName);
            if (columnDef != null)
            {
                return condition.IsTrue(valueColumn, columnDef.Type);
            }
           
            
            return false;
            
        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";

        private static string Encode(string value)
        {
            //TODO DEADLINE 1.C: Encode the delimiter in value

            
            return null;
            
        }

        private static string Decode(string value)
        {
            //TODO DEADLINE 1.C: Decode the value doing the opposite of Encode()
            
            return null;
            
        }

        public string AsText()
        {
            //TODO DEADLINE 1.C: Return the row as string with all values separated by the delimiter

            if (this.Values == null || this.Values.Count == 0)
            {
                return "";
            }
            List<string> encodedValues = new List<string>();
            foreach(string val in this.Values)
            {
                encodedValues.Add(Encode(val));
            }
            return string.Join(Delimiter,encodedValues);
            
        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
            //TODO DEADLINE 1.C: Parse a rowReturn the row as string with all values separated by the delimiter
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            string[] parts = value.Split(new string[] { Delimiter }, StringSplitOptions.None);
            List<string> decodedValues = new List<string>();
            foreach (string part in parts)
            {
                decodedValues.Add(Decode(part));
            }
            return new Row(columns, decodedValues);
            
        }
    }
}

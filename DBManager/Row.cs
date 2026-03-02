using DbManager.Parser;
using System;
using System.Collections.Generic;

namespace DbManager
{
    public class Row
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public List<string> Values { get; set; } = new List<string>();

        public Row(List<ColumnDefinition> columnDefinitions, List<string> values)
        {
<<<<<<< HEAD
            ColumnDefinitions = columnDefinitions ?? new List<ColumnDefinition>();
            Values = values != null ? new List<string>(values) : new List<string>();
=======
            //TODO DEADLINE 1.A: Initialize member variables
            this.ColumnDefinitions = columnDefinitions;
            this.Values = values;

>>>>>>> master
        }

        public void SetValue(string columnName, string value)
        {
<<<<<<< HEAD
            if (string.IsNullOrWhiteSpace(columnName))
                return;

            int index = -1;
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i] != null && ColumnDefinitions[i].Name == columnName)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return;

            while (Values.Count <= index)
            {
                Values.Add("");
            }

            Values[index] = value;
=======
            //TODO DEADLINE 1.A: Given a column name and value, change the value in that column
            var column = this.ColumnDefinitions.FirstOrDefault(col => col.Name == columnName);
            if (column != null)
            {
                int index = this.ColumnDefinitions.IndexOf(column);
                this.Values[index] = value;
            }


>>>>>>> master
        }

        public string GetValue(string columnName)
        {
<<<<<<< HEAD
            if (string.IsNullOrWhiteSpace(columnName))
                return null;

            int index = -1;
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i] != null && ColumnDefinitions[i].Name == columnName)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0 || index >= Values.Count)
                return null;

            return Values[index];
=======
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            var column = this.ColumnDefinitions.FirstOrDefault(col => col.Name == columnName);
            if (column != null)
            {
                int index = this.ColumnDefinitions.IndexOf(column);
                    return (Values[index]);
            }


            return null;
            
>>>>>>> master
        }

        public bool IsTrue(Condition condition)
        {
<<<<<<< HEAD
            if (condition == null || string.IsNullOrWhiteSpace(condition.ColumnName))
                return false;

            int index = -1;
            ColumnDefinition column = null;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i] != null && ColumnDefinitions[i].Name == condition.ColumnName)
                {
                    index = i;
                    column = ColumnDefinitions[i];
                    break;
                }
            }

            if (index < 0 || column == null)
                return false;

            if (Values == null || index >= Values.Count)
                return false;

            string leftValue = Values[index];
            return condition.IsTrue(leftValue, column.Type);
=======
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
            
>>>>>>> master
        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";

        private static string Encode(string value)
        {
<<<<<<< HEAD
            if (value == null)
                return "";

            return value.Replace(Delimiter, DelimiterEncoded);
=======
            //TODO DEADLINE 1.C: Encode the delimiter in value
            if(string.IsNullOrEmpty(value)){
                return value;
            }

            return value.Replace(Delimiter, DelimiterEncoded);
            
>>>>>>> master
        }

        private static string Decode(string value)
        {
<<<<<<< HEAD
            if (value == null)
                return "";

            return value.Replace(DelimiterEncoded, Delimiter);
=======
            //TODO DEADLINE 1.C: Decode the value doing the opposite of Encode()
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Replace(DelimiterEncoded, Delimiter);

>>>>>>> master
        }

        public string AsText()
        {
<<<<<<< HEAD
            if (Values == null || Values.Count == 0)
                return "";

            List<string> encoded = new List<string>();
            for (int i = 0; i < Values.Count; i++)
            {
                encoded.Add(Encode(Values[i]));
            }

            return string.Join(Delimiter, encoded);
=======
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
            
>>>>>>> master
        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
<<<<<<< HEAD
            if (value == null)
                value = "";

            string[] parts = value.Split(Delimiter, StringSplitOptions.None);
            List<string> values = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                values.Add(Decode(parts[i]));
            }

            return new Row(columns, values);
=======
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
            
>>>>>>> master
        }
    }
}
using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DbManager
{
    public class Row
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public List<string> Values { get; set; } = new List<string>();

        public Row(List<ColumnDefinition> columnDefinitions, List<string> values)
        {
            // Eðer null gelirse boþ listeye çek
            ColumnDefinitions = columnDefinitions ?? new List<ColumnDefinition>();
            Values = values != null ? new List<string>(values) : new List<string>();
        }

        public void SetValue(string columnName, string value)
        {
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

            // Deðer listesi column index'ine kadar yoksa doldur
            while (Values.Count <= index)
            {
                Values.Add("");
            }

            Values[index] = value;
        }

        public string GetValue(string columnName)
        {
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
        }

        public bool IsTrue(Condition condition)
        {
            if (condition == null)
                return false;

            int colIndex = -1;
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name == condition.ColumnName)
                {
                    colIndex = i;
                    break;
                }
            }

            if (colIndex < 0 || colIndex >= Values.Count)
                return false;

            string value = Values[colIndex];
            ColumnDefinition.DataType type = ColumnDefinitions[colIndex].Type;

            if (type == ColumnDefinition.DataType.Double)
            {
                double vDouble = double.Parse(value);
                double cDouble = double.Parse(condition.LiteralValue);

                if (condition.Operator == Condition.GreaterThan) return vDouble > cDouble;
                else if (condition.Operator == Condition.LessThan) return vDouble < cDouble;
                else if (condition.Operator == Condition.Equal) return vDouble == cDouble;
                else if (condition.Operator == Condition.GreaterOrEqual) return vDouble >= cDouble;
                else if (condition.Operator == Condition.LessOrEqual) return vDouble <= cDouble;
                else if (condition.Operator == Condition.NotEqual) return vDouble != cDouble;
                else return false;
            }

            else if (type == ColumnDefinition.DataType.Int)
            {
                int vInt = int.Parse(value);
                int cInt = int.Parse(condition.LiteralValue);

                if (condition.Operator == Condition.GreaterThan) return vInt > cInt;
                else if (condition.Operator == Condition.LessThan) return vInt < cInt;
                else if (condition.Operator == Condition.Equal) return vInt == cInt;
                else if (condition.Operator == Condition.GreaterOrEqual) return vInt >= cInt;
                else if (condition.Operator == Condition.LessOrEqual) return vInt <= cInt;
                else if (condition.Operator == Condition.NotEqual) return vInt != cInt;
                else return false;
            }

            else
            {
                string condValue = condition.LiteralValue;

                if (condition.Operator == Condition.Equal) return value == condValue;
                else if (condition.Operator == Condition.NotEqual) return value != condValue;
                else if (condition.Operator == Condition.LessThan) return string.Compare(value, condValue) < 0;
                else if (condition.Operator == Condition.GreaterThan) return string.Compare(value, condValue) > 0;
                else return false;
            }
        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";

        private static string Encode(string value)
        {
            if (value == null)
                return "";

            // ":" karakterini güvenli hale getir
            return value.Replace(Delimiter, DelimiterEncoded);
        }

        private static string Decode(string value)
        {
            if (value == null)
                return "";

            // Encode'un tersini yap
            return value.Replace(DelimiterEncoded, Delimiter);
        }

        public string AsText()
        {
            if (Values == null || Values.Count == 0)
                return "";
            string result = "";

            for (int i=0; i < Values.Count; i++)
            {
                result += Encode(Values[i]);
                if(i < Values.Count - 1)
                {
                    result += Delimiter;
                }
            }
            return result;
        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
            if (value == null)
                value = "";

            string[] parts = value.Split(Delimiter, StringSplitOptions.None);
            List<string> values = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                values.Add(Decode(parts[i]));
            }

            return new Row(columns, values);
        }
    }
}
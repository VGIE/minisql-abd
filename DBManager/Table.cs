using DbManager.Parser;
using System;
using System.Collections.Generic;

namespace DbManager
{
    public class Table
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        private List<Row> Rows = new List<Row>();

        public string Name { get; private set; } = null;

        public Table(string name, List<ColumnDefinition> columns)
        {
            Name = name;
            ColumnDefinitions = columns ?? new List<ColumnDefinition>();
            Rows = new List<Row>();
        }

        public Row GetRow(int i)
        {
            if (i >= 0 && i < Rows.Count)
            {
                return Rows[i];
            }
            return null;
        }

        public void AddRow(Row row)
        {
            if (row != null)
            {
                Rows.Add(row);
            }
        }

        public int NumRows()
        {
            return Rows.Count;
        }

        public ColumnDefinition GetColumn(int i)
        {
            if (i >= 0 && i < ColumnDefinitions.Count)
            {
                return ColumnDefinitions[i];
            }
            return null;
        }

        public int NumColumns()
        {
            return ColumnDefinitions.Count;
        }

        public ColumnDefinition ColumnByName(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
                return null;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i] != null && ColumnDefinitions[i].Name == column)
                    return ColumnDefinitions[i];
            }
            return null;
        }

        public int ColumnIndexByName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                return -1;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i] != null && ColumnDefinitions[i].Name == columnName)
                    return i;
            }
            return -1;
        }

        public override string ToString()
        {
            // "" <- no columns, no rows
            if (NumColumns() == 0)
                return "";

            // "['Name','Age']"
            List<string> cols = new List<string>();
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                cols.Add(ColumnDefinitions[i].Name);
            }

            string result = "['" + string.Join("','", cols) + "']";

            // "{'Adolfo','23'}{'Jacinto','24'}"
            for (int r = 0; r < Rows.Count; r++)
            {
                List<string> vals = Rows[r].Values ?? new List<string>();
                result += "{'" + string.Join("','", vals) + "'}";
            }

            return result;
        }

        public void DeleteIthRow(int row)
        {
            //if there is no i-th row, do nothing
            if (row >= 0 && row < Rows.Count)
            {
                Rows.RemoveAt(row);
            }
        }

        private List<int> RowIndicesWhereConditionIsTrue(Condition condition)
        {
            List<int> indices = new List<int>();

            if (condition == null)
                return indices;

            for (int i = 0; i < Rows.Count; i++)
            {
                if (Rows[i] != null && Rows[i].IsTrue(condition))
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        public void DeleteWhere(Condition condition)
        {
            if (condition == null)
                return;

            List<int> indices = RowIndicesWhereConditionIsTrue(condition);

           
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                DeleteIthRow(indices[i]);
            }
        }

        public Table Select(List<string> columnNames, Condition condition)
        {
            if (columnNames == null || columnNames.Count == 0)
                return null;

            List<ColumnDefinition> selectedColumns = new List<ColumnDefinition>();
            List<int> selectedIndices = new List<int>();

            for (int i = 0; i < columnNames.Count; i++)
            {
                int idx = ColumnIndexByName(columnNames[i]);
                if (idx < 0)
                    return null;

                selectedIndices.Add(idx);
                selectedColumns.Add(GetColumn(idx));
            }

            Table resultTable = new Table("Result", selectedColumns);

            for (int r = 0; r < Rows.Count; r++)
            {
                if (Rows[r] == null)
                    continue;

                if (condition != null && !Rows[r].IsTrue(condition))
                    continue;

                List<string> newValues = new List<string>();
                for (int c = 0; c < selectedIndices.Count; c++)
                {
                    int colIndex = selectedIndices[c];

                    
                    string v = (Rows[r].Values != null && colIndex < Rows[r].Values.Count)
                        ? Rows[r].Values[colIndex]
                        : "";

                    newValues.Add(v);
                }

                resultTable.Insert(newValues);
            }

            return resultTable;
        }

        public bool Insert(List<string> values)
        {
            if (values == null)
                return false;

            if (values.Count != NumColumns())
                return false;

            Row row = new Row(ColumnDefinitions, values);
            AddRow(row);
            return true;
        }

        public bool Update(List<SetValue> setValues, Condition condition)
        {
            if (condition == null)
                return false;

            if (setValues == null || setValues.Count == 0)
                return true;

            for (int i = 0; i < Rows.Count; i++)
            {
                if (Rows[i] == null)
                    continue;

                if (!Rows[i].IsTrue(condition))
                    continue;

                for (int s = 0; s < setValues.Count; s++)
                {
                    int colIndex = ColumnIndexByName(setValues[s].ColumnName);
                    if (colIndex < 0)
                        continue;

                    
                    while (Rows[i].Values.Count <= colIndex)
                        Rows[i].Values.Add("");

                    Rows[i].Values[colIndex] = setValues[s].Value;
                }
            }

            return true;
        }

        //Only for testing purposes
        public const string TestTableName = "TestTable";
        public const string TestColumn1Name = "Name";
        public const string TestColumn2Name = "Height";
        public const string TestColumn3Name = "Age";
        public const string TestColumn1Row1 = "Rodolfo";
        public const string TestColumn1Row2 = "Maider";
        public const string TestColumn1Row3 = "Pepe";
        public const string TestColumn2Row1 = "1.62";
        public const string TestColumn2Row2 = "1.67";
        public const string TestColumn2Row3 = "1.55";
        public const string TestColumn3Row1 = "25";
        public const string TestColumn3Row2 = "67";
        public const string TestColumn3Row3 = "51";
        public const ColumnDefinition.DataType TestColumn1Type = ColumnDefinition.DataType.String;
        public const ColumnDefinition.DataType TestColumn2Type = ColumnDefinition.DataType.Double;
        public const ColumnDefinition.DataType TestColumn3Type = ColumnDefinition.DataType.Int;

        public static Table CreateTestTable(string tableName = TestTableName)
        {
            return new Table(tableName, new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Id"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            });
        }

        public void CheckForTesting(List<List<string>> rows)
        {
            if (rows.Count != NumRows())
                throw new Exception($"The table has {NumRows()} rows and {rows.Count} were expected");

            int rowIndex = 0;
            foreach (List<string> row in rows)
            {
                if (GetRow(rowIndex).Values.Count != row.Count)
                    throw new Exception($"The {rowIndex}-th row has {GetRow(rowIndex).Values.Count} values and {row.Count} were expected");

                for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
                {
                    if (GetRow(rowIndex).Values[columnIndex] != row[columnIndex])
                        throw new Exception($"The [{rowIndex},{columnIndex}] element is {GetRow(rowIndex).Values[columnIndex]} instead of {row[columnIndex]}");
                }

                rowIndex++;
            }
        }
    }
}
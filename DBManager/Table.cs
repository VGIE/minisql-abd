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

        public Table(string name, List<ColumnDefinition> columns)//zeynep
        {
<<<<<<< HEAD
            Name = name;
            ColumnDefinitions = columns ?? new List<ColumnDefinition>();
=======
        
            // TODO DEADLINE 1.A: Initialize member variables
            this.Name = name;
            this.ColumnDefinitions = columns;
        
            
>>>>>>> master
        }

        public Row GetRow(int i)//zeynep
        {
<<<<<<< HEAD
=======
            // TODO DEADLINE 1.A: Return the i-th row
>>>>>>> master
            if (i >= 0 && i < Rows.Count)
            {
                return Rows[i];
            }
            return null;
        }

        public void AddRow(Row row)
        {
<<<<<<< HEAD
=======
            // TODO DEADLINE 1.A: Add a new row
>>>>>>> master
            if (row != null)
            {
                Rows.Add(row);
            }
        }

        public int NumRows()//zeynep
        {
<<<<<<< HEAD
=======
            // TODO DEADLINE 1.A: Return the number of rows
>>>>>>> master
            return Rows.Count;
        }

        public ColumnDefinition GetColumn(int i)//besma
        {
<<<<<<< HEAD
            if (i >= 0 && i < ColumnDefinitions.Count)
            {
                return ColumnDefinitions[i];
            }
            return null;
=======
            //TODO DEADLINE 1.A: Return the i-th column
            if (i >= 0 && i < this.ColumnDefinitions.Count)
            {
                return this.ColumnDefinitions[i];
            }
            int cont = 0;
            foreach (ColumnDefinition col in ColumnDefinitions)
            {
                if (cont == i)
                {
                    return col;
                }
                cont++;
            }

            
            return null;

>>>>>>> master
        }

        public int NumColumns()//besma
        {
<<<<<<< HEAD
            return ColumnDefinitions.Count;
=======
            //TODO DEADLINE 1.A: Return the number of columns
            int cont = 0;
            foreach (ColumnDefinition col in ColumnDefinitions)
            {
                cont++;
            }
            
            return cont;
            
        }
        
        public ColumnDefinition ColumnByName(string column)
        {
            //TODO DEADLINE 1.A: Return the number of columns
            foreach (ColumnDefinition col in ColumnDefinitions)
            {
                if (col.Name == column)
                {
                    return col;
                }
            }

            return null;
            
        }
        public int ColumnIndexByName(string columnName)
        {
            //TODO DEADLINE 1.A: Return the zero-based index of the column named columnName
            for ( int i = 0; i< ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name == columnName)
                {
                    return i;
                }
            }
            return -1;
            
>>>>>>> master
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
<<<<<<< HEAD
            if (NumColumns() == 0)
                return "";

            List<string> cols = new List<string>();
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                cols.Add(ColumnDefinitions[i].Name);
            }

            string result = "['" + string.Join("','", cols) + "']";

            for (int r = 0; r < Rows.Count; r++)
            {
                List<string> vals = Rows[r].Values;
                result += "{'" + string.Join("','", vals) + "'}";
            }

            return result;
=======
            //TODO DEADLINE 1.A: Return the table as a string. The format is specified in the documentation
            //Valid examples:
            //"['Name']{'Adolfo'}{'Jacinto'}" <- one column, two rows
            //"['Name','Age']{'Adolfo','23'}{'Jacinto','24'}" <- two columns, two rows
            //"" <- no columns, no rows
            //"['Name']" <- one column, no rows
            if(ColumnDefinitions.Count == 0 && Rows.Count == 0)
            {
                return "";
            }

            string result = "[";
            for(int i = 0; i< ColumnDefinitions.Count; i++)
            {
                result += "'" + ColumnDefinitions[i].Name + "'";

                if(i < ColumnDefinitions.Count - 1)
                {
                    result += ",";
                }
            }
            result += "]";

            foreach(Row row in Rows)
            {
                result += "{";
                for (int j = 0; j < row.Values.Count; j++)
                {
                    result += "'" + row.Values[j] + "'";
                    if (j < row.Values.Count - 1)
                    {
                        result += ",";
                    }
                }
                result += "}";
                
            }

            return result;
            
>>>>>>> master
        }

        public void DeleteIthRow(int row)
        {
<<<<<<< HEAD
            if (row >= 0 && row < Rows.Count)
            {
                Rows.RemoveAt(row);
            }
=======
            //TODO DEADLINE 1.A: Delete the i-th row. If there is no i-th row, do nothing
            if (row < 0 || row >= Rows.Count)
            {
                return;
            }
            Rows.RemoveAt(row);
>>>>>>> master
        }

        private List<int> RowIndicesWhereConditionIsTrue(Condition condition)
        {
<<<<<<< HEAD
            List<int> indices = new List<int>();

            if (condition == null)
                return indices;

            for (int i = 0; i < Rows.Count; i++)
            {
                if (Rows[i] != null && Rows[i].IsTrue(condition))
=======
            //TODO DEADLINE 1.A: Returns the indices of all the rows where the condition is true. Check Row.IsTrue()

            List<int> indices = new List<int>();

            for (int i = 0; i < NumRows(); i++)
            {
                if (Rows[i].IsTrue(condition))
>>>>>>> master
                {
                    indices.Add(i);
                }
            }

            return indices;
<<<<<<< HEAD
=======

>>>>>>> master
        }

        public void DeleteWhere(Condition condition)
        {
<<<<<<< HEAD
            if (condition == null)
                return;

            List<int> indices = RowIndicesWhereConditionIsTrue(condition);
=======
            //TODO DEADLINE 1.A: Delete all rows where the condition is true. Check RowIndicesWhereConditionIsTrue()
            
            List<int> indices = RowIndicesWhereConditionIsTrue(condition);

>>>>>>> master
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                DeleteIthRow(indices[i]);
            }
<<<<<<< HEAD
=======

>>>>>>> master
        }

        public Table Select(List<string> columnNames, Condition condition)
        {
<<<<<<< HEAD
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
                if (condition != null)
                {
                    if (Rows[r] == null || !Rows[r].IsTrue(condition))
                        continue;
                }

                List<string> newValues = new List<string>();
                for (int c = 0; c < selectedIndices.Count; c++)
                {
                    newValues.Add(Rows[r].Values[selectedIndices[c]]);
                }

                resultTable.Insert(newValues);
            }

            return resultTable;
=======
            //TODO DEADLINE 1.A: Return a new table (with name 'Result') that contains the result of the select. The condition
            //may be null (if no condition, all rows should be returned). This is the most difficult method in this class

            List<ColumnDefinition> resultColumns = new List<ColumnDefinition>();

            foreach (string colName in columnNames)
            {
                ColumnDefinition col = ColumnByName(colName);
                if (col != null)
                    resultColumns.Add(col);
            }

            Table result = new Table("Result", resultColumns);

            foreach (Row row in Rows)
            {
                if (condition == null || row.IsTrue(condition))
                {
                    List<string> values = new List<string>();

                    foreach (string colName in columnNames)
                    {
                        values.Add(row.GetValue(colName));
                    }

                    result.AddRow(new Row(ColumnDefinitions, values));
                }
            }

            return result;

>>>>>>> master
        }

        public bool Insert(List<string> values)
        {
<<<<<<< HEAD
            if (values == null)
                return false;

            if (values.Count == 0)
                return false;
=======
            //TODO DEADLINE 1.A: Insert a new row with the values given. If the number of values is not correct, return false. True otherwise
>>>>>>> master

            if (values.Count != NumColumns())
                return false;

<<<<<<< HEAD
            Row row = new Row(ColumnDefinitions, values);
            AddRow(row);
            return true;
=======
            Row newRow = new Row(ColumnDefinitions,values);
            AddRow(newRow);

            return true;

>>>>>>> master
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
                    if (rows.Count != NumRows())
                        throw new Exception($"The {rowIndex}-th row has {GetRow(rowIndex).Values.Count} values and {row.Count} were expected");

                for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
                {
                    if (GetRow(rowIndex).Values[columnIndex] != row[columnIndex])
                        if (rows.Count != NumRows())
                            throw new Exception($"The [{rowIndex},{columnIndex}] element is {GetRow(rowIndex).Values[columnIndex]} instead of {row[columnIndex]}");
                }

                rowIndex++;
            }
        }
    }
}
using DbManager.Parser;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.AccessControl;

namespace DbManager
{
    public class Update: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<SetValue> Columns { get; private set; }
        public Condition Where { get; private set; }

        public Update(string table, List<SetValue> columnNames, Condition where)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table = table;
            this.Columns = columnNames;
            this.Where = where;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //UpdateSuccess or the last error in the database
            Table table = database.TableByName(this.Table);
            if (table == null)
                return Constants.TableDoesNotExistError;
            foreach (SetValue setValue in this.Columns)
            {

                if (table.ColumnByName(setValue.ColumnName) == null)
                    return Constants.ColumnDoesNotExistError;
            }

            for (int i = 0; i < table.NumRows(); i++)
            {
                Row row = table.GetRow(i);
                if  (row != null)
                {
                    if (this.Where==null || row.IsTrue(this.Where)){

                        foreach (SetValue setValue in this.Columns)
                        {
                            row.SetValue(setValue.ColumnName, setValue.Value);
                        }
                    }
                }
            }
            
            return Constants.UpdateSuccess;
            
        }

       
    }
}
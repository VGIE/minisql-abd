using System;
using DbManager.Parser;

namespace DbManager
{
    public class DropTable : MiniSqlQuery
    {
        public string Table { get; private set; }

        public DropTable(string table)
        {
            Table = table;
        }

        public string Execute(Database database)
        {
            if (database == null)
            {
                throw new NullReferenceException();
            }

            bool result = database.DropTable(Table);

            if (!result)
            {
                return database.LastErrorMessage;
            }

            return Constants.DropTableSuccess;
        }
    }
}
using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


namespace DbManager
{
    public class Database
    {
        private List<Table> Tables = new List<Table>();
        private string m_username;

        public string LastErrorMessage { get; private set; }

        public Manager SecurityManager { get; private set; }

        //This constructor should only be used from Load (without needing to set a password for the user). It cannot be used from any other class
        private Database()
        {
        }

        public Database(string adminUsername, string adminPassword)
        {
            //DEADLINE 1.B: Initalize the member variables
            m_username = adminUsername;
            Tables = new List<Table>();
            SecurityManager = new Manager(adminUsername);
        }

        public bool AddTable(Table table)
        {
            //DEADLINE 1.B: Add a new table to the database
            Tables.Add(table);
            return true;
            
        }

        public Table TableByName(string tableName)
        {
            //DEADLINE 1.B: Find and return the table with the given name
            foreach(Table table in Tables)
            {
                if(table.Name == tableName)
                {
                    return table;
                }
            }
            
            return null;
            
        }

        public bool CreateTable(string tableName, List<ColumnDefinition> ColumnDefinition)
        {
            //DEADLINE 1.B: Create and new table with the given name and columns. If there is already a table with that name,
            //return false and set LastErrorMessage with the appropriate error (Check Constants.cs)
            //Do the same if no column is provided
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            if (TableByName(tableName)!=null)
            {
                LastErrorMessage=Constants.TableAlreadyExistsError;
                return false;
            }
            if(ColumnDefinition==null || ColumnDefinition.Count==0)
            {
                LastErrorMessage=Constants.DatabaseCreatedWithoutColumnsError;
                return false;
            }
            Table newTable =new Table(tableName,ColumnDefinition);
            Tables.Add(newTable);
            LastErrorMessage=Constants.CreateTableSuccess;
            
            return true;
            
        }

        public bool DropTable(string tableName)
        {
            Table table = TableByName(tableName);

            
            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            
            Tables.Remove(table);

            
            LastErrorMessage = Constants.DropTableSuccess;
            return true;
        }


        public bool Insert(string tableName, List<string> values)
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            Table table = TableByName(tableName);
            if (table == null)
            {
                this.LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            bool success = table.Insert(values);
            if (!success)
            {
                this.LastErrorMessage = Constants.ColumnCountsDontMatch;
                return false;
            }
            this.LastErrorMessage = Constants.InsertSuccess;
            return true;
            
        }

        public Table Select(string tableName, List<string> columns, Condition condition)
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table

            Table table = TableByName(tableName);
            if (table == null)
            {
                this.LastErrorMessage = Constants.TableDoesNotExistError;
                return null;
            }
            foreach (string colName in columns)
            {
                if (table.ColumnByName(colName) == null)
                {
                    this.LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return null;
                }
            }
            return table.Select(columns, condition);


        }

        public bool DeleteWhere(string tableName, Condition columnCondition)
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            Table table = TableByName(tableName);

            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            if (table.ColumnByName(columnCondition.ColumnName) == null)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return false;
            }

            table.DeleteWhere(columnCondition);

            LastErrorMessage = Constants.DeleteSuccess;
            return true;

        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition)
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            Table table = TableByName(tableName);

            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            if (table.ColumnByName(columnCondition.ColumnName) == null)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return false;
            }

            foreach (SetValue setValue in columnNames)
            {
                if (table.ColumnByName(setValue.ColumnName) == null)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return false;
                }
            }

            table.Update(columnNames, columnCondition);

            LastErrorMessage = Constants.UpdateSuccess;
            return true;

        }





        public bool Save(string databaseName)
        {
            //DEADLINE 1.C + DEADLINE 5
            try
            {
                
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string dbPath = Path.Combine(baseDir, $"{databaseName}.db");
                string secPath = Path.Combine(baseDir, $"{databaseName}.sec");

#pragma warning disable SYSLIB0011
                BinaryFormatter formatter = new BinaryFormatter();

              
                using (FileStream fs = new FileStream(dbPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(fs, Tables);
                }

                
                using (FileStream fs = new FileStream(secPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(fs, SecurityManager);
                }
#pragma warning restore SYSLIB0011

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Database Load(string databaseName, string username, string password)
        {
            
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string dbPath = Path.Combine(baseDir, $"{databaseName}.db");
                string secPath = Path.Combine(baseDir, $"{databaseName}.sec");

                if (!File.Exists(dbPath) || !File.Exists(secPath))
                    return null;

                List<Table> loadedTables;
                Manager loadedSecurity;

#pragma warning disable SYSLIB0011
                BinaryFormatter formatter = new BinaryFormatter();

                
                using (FileStream fs = new FileStream(dbPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    loadedTables = (List<Table>)formatter.Deserialize(fs);
                }

                
                using (FileStream fs = new FileStream(secPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    loadedSecurity = (Manager)formatter.Deserialize(fs);
                }
#pragma warning restore SYSLIB0011

               
                Database db = new Database();

              
                db.Tables = loadedTables ?? new List<Table>();
                db.SecurityManager = loadedSecurity;

              
                db.m_username = username;

               
                if (db.SecurityManager == null)
                    return null;

                if (!TryVerifyPassword(db.SecurityManager, username, password))
                    return null;

                return db;
            }
            catch
            {
                return null;
            }
        }

        // Tries common method names in Manager without you needing to know the exact API.
        // Returns true only if a known method exists AND it returns true.
        private static bool TryVerifyPassword(Manager manager, string username, string password)
        {
            // Try: bool Login(string user, string pass)
            // Try: bool CheckPassword(string user, string pass)
            // Try: bool Authenticate(string user, string pass)
            // Try: bool CheckUserPassword(string user, string pass)
            string[] methodNames = { "Login", "CheckPassword", "Authenticate", "CheckUserPassword" };

            Type t = manager.GetType();

            foreach (string name in methodNames)
            {
                MethodInfo mi = t.GetMethod(name, new Type[] { typeof(string), typeof(string) });
                if (mi != null && mi.ReturnType == typeof(bool))
                {
                    object result = mi.Invoke(manager, new object[] { username, password });
                    return result is bool b && b;
                }
            }

            // If no known method exists, safest is to fail auth.
            return false;
        }


        public string ExecuteMiniSQLQuery(string query)
        {
            //Parse the query
            MiniSqlQuery miniSQLQuery = MiniSQLParser.Parse(query);

            //If the parser returns null, there must be a syntax error (or the parser is failing)
            if (miniSQLQuery == null)
                return Constants.SyntaxError;

            //Once the query is parsed, we run it on this database
            return miniSQLQuery.Execute(this);
        }


        public bool IsUserAdmin()
        {
            return SecurityManager.IsUserAdmin();
        }





        //All these methods are ONLY FOR TESTING. Use them to simplify creating unit tests:
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminPassword";
        public static Database CreateTestDatabase()
        {
            Database database = new Database(AdminUsername, AdminPassword);

            database.Tables.Add(Table.CreateTestTable());

            return database;
        }

        public void AddTuplesForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);
            foreach (List<string> row in rows)
            {
                table.Insert(row);
            }
        }

        public void CheckForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);

            table.CheckForTesting(rows);
        }
    }
}






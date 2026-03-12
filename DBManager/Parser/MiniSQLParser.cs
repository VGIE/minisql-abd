using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            if (string.IsNullOrWhiteSpace(miniSQLQuery))
                return null;


            const string selectPattern =
                @"^\s*SELECT\s+(?<cols>\*|[A-Za-z][A-Za-z0-9_]*(\s*,\s*[A-Za-z][A-Za-z0-9_]*)*)\s+" +
                @"FROM\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s*" +
                @"(?:(?:WHERE\s+(?<wcol>[A-Za-z][A-Za-z0-9_]*)\s*(?<wop>=|!=|<=|>=|<|>)\s*(?<wval>'[^']*'|[^;\s]+)\s*))?" +
                @";?\s*$";

           
            const string insertPattern =
                @"^\s*INSERT\s+INTO\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+" +
                @"VALUES\s*\(\s*(?<vals>.*)\s*\)\s*;?\s*$";

          
            const string dropTablePattern =
                @"^\s*DROP\s+TABLE\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s*;?\s*$";

            const string createTablePattern =
                @"^\s*CREATE\s+TABLE\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s*" +
                @"\(\s*(?<cols>.+?)\s*\)\s*;?\s*$";

        
            const string updateTablePattern =
                @"^\s*UPDATE\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+" +
                @"SET\s+(?<set>.+?)\s+" +
                @"WHERE\s+(?<wcol>[A-Za-z][A-Za-z0-9_]*)\s*(?<wop>=|!=|<=|>=|<|>)\s*(?<wval>'[^']*'|[^;\s]+)\s*;?\s*$";


            const string deletePattern =
                @"^\s*DELETE\s+FROM\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+" +
                @"WHERE\s+(?<wcol>[A-Za-z][A-Za-z0-9_]*)\s*(?<wop>=|!=|<=|>=|<|>)\s*(?<wval>'[^']*'|[^;\s]+)\s*;?\s*$";


            const string createSecurityProfilePattern =
                @"^\s*CREATE\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9_]*)\s*;?\s*$";

            const string dropSecurityProfilePattern =
                @"^\s*DROP\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9_]*)\s*;?\s*$";

            const string grantPattern =
                @"^\s*GRANT\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string revokePattern =
                @"^\s*REVOKE\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";


            const string addUserPattern =
                @"^\s*ADD\s+USER\s*\(\s*(?<username>[A-Za-z][A-Za-z0-9]*)\s*,\s*(?<password>[^,\)\s]+)\s*,\s*(?<profile>[A-Za-z][A-Za-z0-9]*)\s*\)\s*;?\s*$";


            const string deleteUserPattern =
                @"^\s*DELETE\s+USER\s+(?<username>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";


            var mCreate = Regex.Match(miniSQLQuery, createSecurityProfilePattern);
            if (mCreate.Success)
                return new CreateSecurityProfile(mCreate.Groups["profile"].Value);

            var mDrop = Regex.Match(miniSQLQuery, dropSecurityProfilePattern);
            if (mDrop.Success)
                return new DropSecurityProfile(mDrop.Groups["profile"].Value);

            var mGrant = Regex.Match(miniSQLQuery, grantPattern);
            if (mGrant.Success)
                return new Grant(
                    mGrant.Groups["privilege"].Value,
                    mGrant.Groups["table"].Value,
                    mGrant.Groups["profile"].Value
                );

            var mRevoke = Regex.Match(miniSQLQuery, revokePattern);
            if (mRevoke.Success)
                return new Revoke(
                    mRevoke.Groups["privilege"].Value,
                    mRevoke.Groups["table"].Value,
                    mRevoke.Groups["profile"].Value
                );

            var mAddUser = Regex.Match(miniSQLQuery, addUserPattern);
            if (mAddUser.Success)
                return new AddUser(
                    mAddUser.Groups["username"].Value,
                    mAddUser.Groups["password"].Value,
                    mAddUser.Groups["profile"].Value
                );

            var mDeleteUser = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (mDeleteUser.Success)
                return new DeleteUser(mDeleteUser.Groups["username"].Value);


            var mSelect = Regex.Match(miniSQLQuery, selectPattern);
            if (mSelect.Success)
            {
                string table = mSelect.Groups["table"].Value;

                List<string> columns;
                string colsText = mSelect.Groups["cols"].Value.Trim();
                if (colsText == "*")
                {
                    
                    columns = new List<string> { "*" };
                }
                else
                {
                    columns = CommaSeparatedNames(colsText);
                }

                Condition condition = null;
                if (mSelect.Groups["wcol"].Success)
                {
                    condition = new Condition(
                        mSelect.Groups["wcol"].Value,
                        mSelect.Groups["wop"].Value,
                        Unquote(mSelect.Groups["wval"].Value)
                    );
                }

                return new Select(table, columns, condition);
            }

        
            var mInsert = Regex.Match(miniSQLQuery, insertPattern);
            if (mInsert.Success)
            {
                string table = mInsert.Groups["table"].Value;
                List<string> values = CommaSeparatedValues(mInsert.Groups["vals"].Value);
                return new Insert(table, values);
            }

        
            var mDropTable = Regex.Match(miniSQLQuery, dropTablePattern);
            if (mDropTable.Success)
            {
                return new DropTable(mDropTable.Groups["table"].Value);
            }

           
            var mCreateTable = Regex.Match(miniSQLQuery, createTablePattern);
            if (mCreateTable.Success)
            {
                string table = mCreateTable.Groups["table"].Value;
                string colsRaw = mCreateTable.Groups["cols"].Value;

                List<ColumnDefinition> columns = ParseCreateTableColumns(colsRaw);
                if (columns == null)
                    return null;

                return new CreateTable(table, columns);
            }

          
            var mUpdate = Regex.Match(miniSQLQuery, updateTablePattern);
            if (mUpdate.Success)
            {
                string table = mUpdate.Groups["table"].Value;
                string setRaw = mUpdate.Groups["set"].Value;

                List<SetValue> setValues = ParseSetValues(setRaw);
                if (setValues == null)
                    return null;

                Condition condition = new Condition(
                    mUpdate.Groups["wcol"].Value,
                    mUpdate.Groups["wop"].Value,
                    Unquote(mUpdate.Groups["wval"].Value)
                );

                return new Update(table, setValues, condition);
            }

           
            var mDelete = Regex.Match(miniSQLQuery, deletePattern);
            if (mDelete.Success)
            {
                string table = mDelete.Groups["table"].Value;

                Condition condition = new Condition(
                    mDelete.Groups["wcol"].Value,
                    mDelete.Groups["wop"].Value,
                    Unquote(mDelete.Groups["wval"].Value)
                );

                return new Delete(table, condition);
            }

       
            return null;
        }


        static List<string> CommaSeparatedNames(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            string[] parts = text.Split(",", StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                string name = parts[i].Trim();
                if (!string.IsNullOrEmpty(name))
                    result.Add(name);
            }

            return result;
        }


        static List<string> CommaSeparatedValues(string text)
        {
            List<string> values = new List<string>();
            if (text == null) return values;

   
            bool inQuotes = false;
            var current = new System.Text.StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch == '\'')
                {
                    inQuotes = !inQuotes;
                    current.Append(ch);
                    continue;
                }

                if (ch == ',' && !inQuotes)
                {
                    values.Add(Unquote(current.ToString().Trim()));
                    current.Clear();
                    continue;
                }

                current.Append(ch);
            }

            if (current.Length > 0)
                values.Add(Unquote(current.ToString().Trim()));

            return values;
        }

        static string Unquote(string value)
        {
            if (value == null) return null;
            value = value.Trim();
            if (value.Length >= 2 && value.StartsWith("'") && value.EndsWith("'"))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        static List<SetValue> ParseSetValues(string setRaw)
        {
            if (string.IsNullOrWhiteSpace(setRaw))
                return null;

            List<SetValue> list = new List<SetValue>();
            List<string> pairs = CommaSeparatedValues(setRaw); 

            for (int i = 0; i < pairs.Count; i++)
            {
                string p = pairs[i];
                int eq = p.IndexOf('=');
                if (eq <= 0) return null;

                string col = p.Substring(0, eq).Trim();
                string val = p.Substring(eq + 1).Trim();

                list.Add(new SetValue(col, Unquote(val)));
            }

            return list;
        }

        static List<ColumnDefinition> ParseCreateTableColumns(string colsRaw)
        {
            if (string.IsNullOrWhiteSpace(colsRaw))
                return null;

            string[] parts = colsRaw.Split(",", StringSplitOptions.RemoveEmptyEntries);

            List<ColumnDefinition> cols = new List<ColumnDefinition>();

            for (int i = 0; i < parts.Length; i++)
            {
                string item = parts[i].Trim();
                if (string.IsNullOrEmpty(item))
                    continue;

                // "TYPE NAME"
                string[] tokens = Regex.Split(item, @"\s+");
                if (tokens.Length < 2)
                    return null;

                string typeText = tokens[0].Trim();
                string nameText = tokens[1].Trim();

                ColumnDefinition.DataType dt;
                if (typeText == "STRING") dt = ColumnDefinition.DataType.String;
                else if (typeText == "INT") dt = ColumnDefinition.DataType.Int;
                else if (typeText == "DOUBLE") dt = ColumnDefinition.DataType.Double;
                else return null;

                cols.Add(new ColumnDefinition(dt, nameText));
            }

            return cols;
        }
    }
}
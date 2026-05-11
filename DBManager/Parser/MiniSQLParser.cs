using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DbManager.Parser;

namespace DbManager
{
    public class MiniSQLParser
    {
        private const string Asterisk = "*";
        private const string StringType = "TEXT";

        private const string IntType = "INT";
        private const string DoubleType = "DOUBLE";

        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            if (string.IsNullOrWhiteSpace(miniSQLQuery))
                return null;

            string input = miniSQLQuery.Trim();

            const string selectPattern =@"^SELECT\s+(\*|[a-zA-Z0-9]+(?:,[a-zA-Z0-9]+)*)\s+FROM\s+([a-zA-Z0-9]+)$";
            const string selectWherePattern = @"^SELECT\s+(\*|[a-zA-Z0-9]+(?:,[a-zA-Z0-9]+)*)\s+FROM\s+([a-zA-Z0-9]+)\s+WHERE\s+([a-zA-Z0-9]+)(=|<|>)('[-]?\d+(\.\d+)?'|'[^']+'|'[-]?\d+(\.\d+)?)$";
            const string insertPattern =@"^INSERT\s+INTO\s+(\w+)\s+VALUES\s*\((('[-]?\d+(\.\d+)?'|'[^']+')(?:,('[-]?\d+(\.\d+)?'|'[^']+'))*)\)$";
            const string dropTablePattern = @" ^\s*DROP\s+TABLE\s+(?<table>[a-zA-Z][a-zA-Z0-9_]*)\s*;?\s*\z";
            const string createTablePattern = @"^CREATE\s+TABLE\s+(\w+)\s+\((\w+\s+(?:INT|DOUBLE|TEXT)(?:,\w+\s+(?:INT|DOUBLE|TEXT))*)?\)\s*;?\s*$";


            const string updateTablePattern = @"^UPDATE\s+(\w+)\s+SET\s+(\w+=('[-]?\d+(\.\d+)?'|'[^']+')(?:,(\w+=('[-]?\d+(\.\d+)?'|'[^']+'))*)?)\s+WHERE\s+(\w+)(=|<|>)('[-]?\d+(\.\d+)?'|'[^']+')$";
            const string deletePattern =
                @"^DELETE\s+FROM\s+(\w+)\s+WHERE\s+(\w+)(=|<|>)('-?\d+(\.\d+)?'|'[^']+')$";

            const string createSecurityProfilePattern =
                @"^\s*CREATE\s+SECURITY\s+PROFILE\s+(?<profile>[a-zA-Z][a-zA-Z0-9_]*)\s*;?\s*$";

            const string dropSecurityProfilePattern =
                @"^\s*DROP\s+SECURITY\s+PROFILE\s+(?<profile>[a-zA-Z][a-zA-Z0-9_]*)\s*;?\s*$";

            const string grantPattern =
                @"^\s*GRANT\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[a-zA-Z][a-zA-Z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string revokePattern =
                @"^\s*REVOKE\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[a-zA-Z][a-zA-Z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";


            const string addUserPattern =
                @"^\s*ADD\s+USER\s*\(\s*(?<username>[A-Za-z][A-Za-z0-9]*)\s*,\s*(?<password>[^,\)\s]+)\s*,\s*(?<profile>[A-Za-z][A-Za-z0-9]*)\s*\)\s*;?\s*$";


            const string deleteUserPattern =
                @"^\s*DELETE\s+USER\s+(?<username>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";


            Match mSelect = Regex.Match(miniSQLQuery, selectPattern);
            if (mSelect.Success)
            {
                return new Select(mSelect.Groups[2].Value, CommaSeparatedNames(mSelect.Groups[1].Value));
            }
            Match mSelectWhere = Regex.Match(miniSQLQuery, selectWherePattern);
            if (mSelectWhere.Success)
            {
                string tableName = mSelectWhere.Groups[2].Value;
                List<string> columns = CommaSeparatedNames(mSelectWhere.Groups[1].Value);
                string col = mSelectWhere.Groups[3].Value;
                string operador = mSelectWhere.Groups[4].Value;
                string valor = mSelectWhere.Groups[5].Value;

                if (valor.Contains(" ") && !valor.StartsWith("'")) return null;
                return new Select(tableName, columns, new Condition(col, operador, valor.Trim('\'')));

         
}

            Match mInsert = Regex.Match(miniSQLQuery, insertPattern);
            if (mInsert.Success)
            {
                var values = ParseInsertValues(mInsert.Groups[2].Value);
                if (values == null) return null;
                return new Insert(mInsert.Groups[1].Value.Trim(), values);

            }


            var mDropTable = Regex.Match(input, dropTablePattern);
            if (mDropTable.Success) return new DropTable(mDropTable.Groups["table"].Value);

            var mCreateTable = Regex.Match(input, createTablePattern);
            if (mCreateTable.Success)
            {
                var columns = ParseCreateTableColumns(mCreateTable.Groups[2].Value);
                return new CreateTable(mCreateTable.Groups[1].Value, columns);
            }

            Match mUpdate = Regex.Match(miniSQLQuery, updateTablePattern);
            if (mUpdate.Success)
            {
                var setValues = ParseSetValues(mUpdate.Groups[2].Value);
                if (setValues == null || setValues.Count == 0) return null;
                var condition = new Condition(mUpdate.Groups[8].Value, mUpdate.Groups[9].Value, mUpdate.Groups[10].Value.Trim('\''));
                return new Update(mUpdate.Groups[1].Value, setValues, condition);
            }

            var mDelete = Regex.Match(input, deletePattern);
            if (mDelete.Success)
            {
                string valorDelete = mDelete.Groups[4].Value.Trim();
                if (valorDelete.Contains(" ") && !valorDelete.StartsWith("'")) return null;
                return new Delete(mDelete.Groups[1].Value.Trim(), new Condition(mDelete.Groups[2].Value.Trim(), mDelete.Groups[3].Value.Trim(), valorDelete.Trim('\'')));
            }
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



            return null; 
        }

        static List<string> CommaSeparatedNames(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            string[] parts = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            foreach (var p in parts) result.Add(p.Trim());
            return result;
        }
        static List<string> ParseInsertValues(string text)
        {
            string[] textParts = text.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            foreach (string v in textParts)
            {
                string val = v.Trim();
                if (val.Contains(" ") && !val.StartsWith("'")) return null; // Falla si hay espacios sin comillas
                if ((val.StartsWith("'") && !val.EndsWith("'")) || (!val.StartsWith("'") && val.EndsWith("'"))) return null;
                result.Add(val.Trim('\''));
            }
            return result;
        }

        

        static string Unquote(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            value = value.Trim();
            if (value.Length >= 2 && value.StartsWith("'") && value.EndsWith("'"))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        static List<SetValue> ParseSetValues(string setRaw)
        {
            if (string.IsNullOrWhiteSpace(setRaw)) return null;
            List<SetValue> list = new List<SetValue>();
            bool inQuotes = false;
            string current = "";
            List<string> pairs = new List<string>();
            for (int i = 0; i < setRaw.Length; i++)
            {
                char ch = setRaw[i];
                if (ch == '\'') inQuotes = !inQuotes;
                if (ch == ',' && !inQuotes)
                {
                    pairs.Add(current.Trim());
                    current = "";
                }
                else current += ch;
            }
            if (!string.IsNullOrWhiteSpace(current)) pairs.Add(current.Trim());

            foreach (var p in pairs)
            {
                int eq = p.IndexOf('=');
                if (eq <= 0) return null;
                list.Add(new SetValue(p.Substring(0, eq).Trim(), Unquote(p.Substring(eq + 1).Trim())));
            }
            return list;
        }

        static List<ColumnDefinition> ParseCreateTableColumns(string colsRaw)
        {
            if (string.IsNullOrWhiteSpace(colsRaw)) return null;
            string[] parts = colsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<ColumnDefinition> cols = new List<ColumnDefinition>();
            for (int i=0; i<parts.Length; i++ )
            {
                string item = parts[i].Trim();
                if (string.IsNullOrEmpty(item))
                    continue;

                // "TYPE NAME"
                string[] tokens = Regex.Split(item, @"\s+");
                if (tokens.Length != 2)
                    return null;

                string nameText = tokens[0].Trim();            
                string typeText = tokens[1].Trim().ToUpper();

                ColumnDefinition.DataType dt;
                if (typeText == StringType) dt = ColumnDefinition.DataType.String;
                else if (typeText == IntType) dt = ColumnDefinition.DataType.Int;
                else if (typeText == DoubleType) dt = ColumnDefinition.DataType.Double;
                else return null;

                cols.Add(new ColumnDefinition(dt, nameText));
            }
            if (cols.Count == 0)
                return null;

            return cols;
        }
    }
}

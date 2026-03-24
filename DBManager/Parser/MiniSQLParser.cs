using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DbManager.Parser;

namespace DbManager
{
    public class MiniSQLParser
    {
        private const string Asterisk = "*";
        private const string StringType = "STRING";
        private const string IntType = "INT";
        private const string DoubleType = "DOUBLE";

        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            if (string.IsNullOrWhiteSpace(miniSQLQuery))
                return null;

            string input = miniSQLQuery.Trim();

            const string grantPattern = @"^GRANT\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?$";
            const string revokePattern = @"^REVOKE\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?$";
            const string createSecurityProfilePattern = @"^CREATE\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?$";
            const string dropSecurityProfilePattern = @"^DROP\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?$";
            const string selectPattern = @"^SELECT\s+(?<cols>\*|[A-Za-z][A-Za-z0-9_]*(\s*,\s*[A-Za-z][A-Za-z0-9_]*)*)\s+FROM\s+(?<table>[A-Za-z][A-Za-z0-9_]*)(?:\s+WHERE\s+(?<wcol>[A-Za-z][A-Za-z0-9_]*)\s*(?<wop>=|!=|<=|>=|<|>)\s*(?<wval>'[^']*'|[^;\s]+))?\s*;?$";
            const string insertPattern = @"^INSERT\s+INTO\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+VALUES\s*\(\s*(?<vals>.*)\s*\)\s*;?$";
            const string dropTablePattern = @"^DROP\s+TABLE\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s*;?$";
            const string createTablePattern = @"^CREATE\s+TABLE\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s*\(\s*(?<cols>.+?)\s*\)\s*;?$";
            const string updateTablePattern = @"^UPDATE\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+SET\s+(?<set>.+?)\s+WHERE\s+(?<wcol>[A-Za-z][A-Za-z0-9_]*)\s*(?<wop>=|!=|<=|>=|<|>)\s*(?<wval>'[^']*'|[^;\s]+)\s*;?$";
            const string deletePattern = @"^DELETE\s+FROM\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+WHERE\s+(?<wcol>[A-Za-z][A-Za-z0-9_]*)\s*(?<wop>=|!=|<=|>=|<|>)\s*(?<wval>'[^']*'|[^;\s]+)\s*;?$";
            const string addUserPattern = @"^ADD\s+USER\s*\(\s*(?<username>[A-Za-z][A-Za-z0-9]*)\s*,\s*(?<password>[^,\)\s]+)\s*,\s*(?<profile>[A-Za-z][A-Za-z0-9]*)\s*\)\s*;?$";
            const string deleteUserPattern = @"^DELETE\s+USER\s+(?<username>[A-Za-z][A-Za-z0-9]*)\s*;?$";

            var mGrant = Regex.Match(input, grantPattern);
            if (mGrant.Success) return new Grant(mGrant.Groups["privilege"].Value, mGrant.Groups["table"].Value, mGrant.Groups["profile"].Value);

            var mRevoke = Regex.Match(input, revokePattern);
            if (mRevoke.Success) return new Revoke(mRevoke.Groups["privilege"].Value, mRevoke.Groups["table"].Value, mRevoke.Groups["profile"].Value);

            var mCreateSec = Regex.Match(input, createSecurityProfilePattern);
            if (mCreateSec.Success) return new CreateSecurityProfile(mCreateSec.Groups["profile"].Value);

            var mDropSec = Regex.Match(input, dropSecurityProfilePattern);
            if (mDropSec.Success) return new DropSecurityProfile(mDropSec.Groups["profile"].Value);

            var mAddUser = Regex.Match(input, addUserPattern);
            if (mAddUser.Success) return new AddUser(mAddUser.Groups["username"].Value, mAddUser.Groups["password"].Value, mAddUser.Groups["profile"].Value);

            var mDeleteUser = Regex.Match(input, deleteUserPattern);
            if (mDeleteUser.Success) return new DeleteUser(mDeleteUser.Groups["username"].Value);

            var mSelect = Regex.Match(input, selectPattern);
            if (mSelect.Success)
            {
                string table = mSelect.Groups["table"].Value;
                string colsText = mSelect.Groups["cols"].Value.Trim();
                List<string> columns = (colsText == Asterisk) ? new List<string> { "*" } : CommaSeparatedNames(colsText);
                Condition condition = null;
                if (mSelect.Groups["wcol"].Success)
                    condition = new Condition(mSelect.Groups["wcol"].Value, mSelect.Groups["wop"].Value, Unquote(mSelect.Groups["wval"].Value));
                return new Select(table, columns, condition);
            }

            var mInsert = Regex.Match(input, insertPattern);
            if (mInsert.Success)
                return new Insert(mInsert.Groups["table"].Value, CommaSeparatedValues(mInsert.Groups["vals"].Value));

            var mDropTable = Regex.Match(input, dropTablePattern);
            if (mDropTable.Success) return new DropTable(mDropTable.Groups["table"].Value);

            var mCreateTable = Regex.Match(input, createTablePattern);
            if (mCreateTable.Success)
            {
                var columns = ParseCreateTableColumns(mCreateTable.Groups["cols"].Value);
                return columns == null ? null : new CreateTable(mCreateTable.Groups["table"].Value, columns);
            }

            var mUpdate = Regex.Match(input, updateTablePattern);
            if (mUpdate.Success)
            {
                var setValues = ParseSetValues(mUpdate.Groups["set"].Value);
                if (setValues == null || setValues.Count == 0) return null;
                var condition = new Condition(mUpdate.Groups["wcol"].Value, mUpdate.Groups["wop"].Value, Unquote(mUpdate.Groups["wval"].Value));
                return new Update(mUpdate.Groups["table"].Value, setValues, condition);
            }

            var mDelete = Regex.Match(input, deletePattern);
            if (mDelete.Success)
                return new Delete(mDelete.Groups["table"].Value, new Condition(mDelete.Groups["wcol"].Value, mDelete.Groups["wop"].Value, Unquote(mDelete.Groups["wval"].Value)));

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

        static List<string> CommaSeparatedValues(string text)
        {
            List<string> values = new List<string>();
            if (text == null) return values;
            bool inQuotes = false;
            string current = "";
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch == '\'') inQuotes = !inQuotes;
                if (ch == ',' && !inQuotes)
                {
                    values.Add(Unquote(current.Trim()));
                    current = "";
                }
                else current += ch;
            }
            if (!string.IsNullOrWhiteSpace(current)) values.Add(Unquote(current.Trim()));
            return values;
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
            foreach (var item in parts)
            {
                string[] tokens = Regex.Split(item.Trim(), @"\s+");
                if (tokens.Length < 2) return null;

                string typeText = tokens[0].ToUpper().Trim();
                string nameText = tokens[1].Trim();

                ColumnDefinition.DataType dt;
                if (typeText == StringType) dt = ColumnDefinition.DataType.String;
                else if (typeText == IntType) dt = ColumnDefinition.DataType.Int;
                else if (typeText == DoubleType) dt = ColumnDefinition.DataType.Double;
                else return null;

                cols.Add(new ColumnDefinition(dt, nameText));
            }
            return cols;
        }
    }
}
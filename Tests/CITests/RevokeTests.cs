using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            // TODO DEADLINE 2
            const string selectPattern = null;
            const string insertPattern = null;
            const string dropTablePattern = null;
            const string createTablePattern = null;
            const string updateTablePattern = null;
            const string deletePattern = null;

            // TODO DEADLINE 4 (CASE-SENSITIVE)

            const string createSecurityProfilePattern =
                @"^\s*CREATE\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string dropSecurityProfilePattern =
                @"^\s*DROP\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string grantPattern =
                @"^\s*GRANT\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string revokePattern =
                @"^\s*REVOKE\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string addUserPattern =
                @"^\s*ADD\s+USER\s*\(\s*(?<username>[A-Za-z][A-Za-z0-9]*)\s*,\s*(?<password>[^,\)\s]+)\s*,\s*(?<profile>[A-Za-z][A-Za-z0-9]*)\s*\)\s*;?\s*$";

            const string deleteUserPattern =
                @"^\s*DELETE\s+USER\s+(?<username>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";


            var mCreate = Regex.Match(miniSQLQuery, createSecurityProfilePattern);
            if (mCreate.Success)
            {
                string profile = mCreate.Groups["profile"].Value;
                return new CreateSecurityProfile(profile);
            }

            var mDrop = Regex.Match(miniSQLQuery, dropSecurityProfilePattern);
            if (mDrop.Success)
            {
                string profile = mDrop.Groups["profile"].Value;
                return new DropSecurityProfile(profile);
            }

            var mGrant = Regex.Match(miniSQLQuery, grantPattern);
            if (mGrant.Success)
            {
                string privilege = mGrant.Groups["privilege"].Value;
                string table = mGrant.Groups["table"].Value;
                string profile = mGrant.Groups["profile"].Value;

                return new Grant(privilege, table, profile);
            }

            var mRevoke = Regex.Match(miniSQLQuery, revokePattern);
            if (mRevoke.Success)
            {
                string privilege = mRevoke.Groups["privilege"].Value;
                string table = mRevoke.Groups["table"].Value;
                string profile = mRevoke.Groups["profile"].Value;

                return new Revoke(privilege, table, profile);
            }

            var mAddUser = Regex.Match(miniSQLQuery, addUserPattern);
            if (mAddUser.Success)
            {
                string username = mAddUser.Groups["username"].Value;
                string password = mAddUser.Groups["password"].Value;
                string profile = mAddUser.Groups["profile"].Value;

                return new AddUser(username, password, profile);
            }

            var mDeleteUser = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (mDeleteUser.Success)
            {
                string username = mDeleteUser.Groups["username"].Value;
                return new DeleteUser(username);
            }

            return null;
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();

            for (int i = 0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }

            return commaSeparator;
        }
    }
}
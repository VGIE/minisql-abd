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

<<<<<<< HEAD
            // TODO DEADLINE 4 (CASE-SENSITIVE)

            const string createSecurityProfilePattern =
                @"^\s*CREATE\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9_]*)\s*;?\s*$";

            const string dropSecurityProfilePattern =
                @"^\s*DROP\s+SECURITY\s+PROFILE\s+(?<profile>[A-Za-z][A-Za-z0-9_]*)\s*;?\s*$";

            const string grantPattern =
                @"^\s*GRANT\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            const string revokePattern =
                @"^\s*REVOKE\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>[A-Za-z][A-Za-z0-9_]*)\s+TO\s+(?<profile>[A-Za-z][A-Za-z0-9]*)\s*;?\s*$";

            // AddUser: (user,password,profile)  -> '_' yasak
            const string addUserPattern =
                @"^\s*ADD\s+USER\s*\(\s*(?<username>[A-Za-z][A-Za-z0-9]*)\s*,\s*(?<password>[^,\)\s]+)\s*,\s*(?<profile>[A-Za-z][A-Za-z0-9]*)\s*\)\s*;?\s*$";

            // DeleteUser: DELETE USER user  -> '_' yasak
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
                return new Grant(mGrant.Groups["privilege"].Value, mGrant.Groups["table"].Value, mGrant.Groups["profile"].Value);

            var mRevoke = Regex.Match(miniSQLQuery, revokePattern);
            if (mRevoke.Success)
                return new Revoke(mRevoke.Groups["privilege"].Value, mRevoke.Groups["table"].Value, mRevoke.Groups["profile"].Value);

            var mAddUser = Regex.Match(miniSQLQuery, addUserPattern);
            if (mAddUser.Success)
                return new AddUser(mAddUser.Groups["username"].Value, mAddUser.Groups["password"].Value, mAddUser.Groups["profile"].Value);

            var mDeleteUser = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (mDeleteUser.Success)
                return new DeleteUser(mDeleteUser.Groups["username"].Value);

            return null;
=======
            //TODO DEADLINE 4
            const string createSecurityProfilePattern = @"^CREATE\s+SECURITY\s+PROFILE\s+([a-zA-Z0-9]+)$";
            
            const string dropSecurityProfilePattern = @"^DROP\s+SECURITY\s+PROFILE\s+([a-zA-Z0-9]+)$";
            
            const string grantPattern = @"^GRANT\s+(SELECT|INSERT|DELETE|UPDATE)\s+ON\s+([a-zA-Z0-9]+)\s+TO\s+([a-zA-Z0-9]+)$";
            
            const string revokePattern = @"^REVOKE\s+(SELECT|INSERT|DELETE|UPDATE)\s+ON\s+([a-zA-Z0-9]+)\s+TO\s+([a-zA-Z0-9]+)$";
            
            const string addUserPattern = @"^ADD\s+USER\s*\(\s*([a-zA-Z0-9]+)\s*,\s*([a-zA-Z0-9]+)\s*,\s*([a-zA-Z0-9]+)\s*\)$";
            
            const string deleteUserPattern = @"^DELETE\s+USER\s+([a-zA-Z0-9]+)$";

            Match match;
            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            match = Regex.Match(miniSQLQuery, addUserPattern);
            if (match.Success)
                return new AddUser(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            
            match = Regex.Match(miniSQLQuery, createSecurityProfilePattern);
            if (match.Success)
                return new CreateSecurityProfile(match.Groups[1].Value);

            match = Regex.Match(miniSQLQuery, dropSecurityProfilePattern);
            if (match.Success)
                return new DropSecurityProfile(match.Groups[1].Value);

            match = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (match.Success)
                return new DeleteUser(match.Groups[1].Value);

            match = Regex.Match(miniSQLQuery, grantPattern);
            if (match.Success)
                return new Grant(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);





                return null;
           
>>>>>>> master
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
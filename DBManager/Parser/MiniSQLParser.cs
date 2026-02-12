using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = null;
            
            const string insertPattern = null;
            
            const string dropTablePattern = null;
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = null;
            
            const string updateTablePattern = null;
            
            const string deletePattern = null;
            

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
           
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}

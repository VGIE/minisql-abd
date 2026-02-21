using System;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;
        }

        public string Execute(Database database)
        {
            if (!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            if (database.SecurityManager.ProfileByName(ProfileName) == null)
                return Constants.SecurityProfileDoesNotExistError;

            Privilege privilege = (Privilege)Enum.Parse(typeof(Privilege), PrivilegeName);

            database.SecurityManager.RevokePrivilege(ProfileName, TableName, privilege);

            return Constants.RevokePrivilegeSuccess;
        }
    }
}
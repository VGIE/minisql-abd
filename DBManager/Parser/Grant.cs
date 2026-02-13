using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
    public class Grant : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Grant(string privilegeName, string tableName, string profileName)
        {
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;
        }

        public string Execute(Database database)
        {
            if (!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            Profile profile = database.SecurityManager.ProfileByName(ProfileName);
            if (profile == null)
                return Constants.SecurityProfileDoesNotExistError;

            if (!Enum.TryParse(PrivilegeName, true, out Privilege privilege))
                return Constants.PrivilegeDoesNotExistError;

            if (!profile.GrantPrivilege(TableName, privilege))
                return Constants.ProfileAlreadyHasPrivilege;

            return Constants.GrantPrivilegeSuccess;
        }
    }
}

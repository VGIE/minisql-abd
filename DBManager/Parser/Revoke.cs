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
            if (database == null) return Constants.Error;

            if (!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            if (database.TableByName(TableName) == null)
                return Constants.TableDoesNotExistError;

            Profile profile = database.SecurityManager.ProfileByName(ProfileName);
            if (profile == null)
                return Constants.SecurityProfileDoesNotExistError;

            if (!Enum.TryParse(PrivilegeName, true, out Privilege privilege))
                return Constants.PrivilegeDoesNotExistError;

            profile.RevokePrivilege(TableName, privilege);

            return Constants.RevokePrivilegeSuccess;
        }
    }
}
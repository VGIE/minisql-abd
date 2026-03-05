using System;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; private set; }
        public string TableName { get; private set; }
        public string ProfileName { get; private set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;
        }

        public string Execute(Database database)
        {
            if (database == null || !database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            // Profil var mý?
            if (database.SecurityManager.ProfileByName(ProfileName) == null)
                return Constants.SecurityProfileDoesNotExistError;

            // Privilege dönüþümü: enum adýný bilmeye gerek yok
            Privilege privilege = PrivilegeUtils.FromPrivilegeName(PrivilegeName);

            // Yetkiyi geri al
            database.SecurityManager.RevokePrivilege(ProfileName, TableName, privilege);

            return Constants.RevokePrivilegeSuccess;
        }
    }
}
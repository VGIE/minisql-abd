using System;
using System.Collections.Generic;
using System.Text;
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
            //TODO DEADLINE 4: Initialize member variables
            //PrivilegeName =privilegeName;
            //TableName = tableName;
            //ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 
            //Security.Privilege priv = PrivilegeUtils.FromPrivilegeName(this.PrivilegeName);
            //Profile profile = database.SecurityManager.ProfileByName(this.ProfileName);

            //if(profile == null)
            //{
              //  return Constants.SecurityProfileDoesNotExistError;
            //}
            //if (!database.SecurityManager.IsGrantedPrivilege(profile,TableName,priv))
            //{
              //  return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            //}
            //database.SecurityManager.RevokePrivilege(profile,TableName, priv);
            //return Constants.RevokePrivilegeSuccess;
            return null;
        }

    }
}

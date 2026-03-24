using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
    public class DropSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public DropSecurityProfile(string profileName)
        {
            ProfileName = profileName;
        }

        public string Execute(Database database)
        {
            if (database == null)
            {
                return Constants.Error;
            }

            if (!database.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            bool removed = database.SecurityManager.RemoveProfile(ProfileName);

            if (!removed)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

            return Constants.DropSecurityProfileSuccess;
        }
    }
}
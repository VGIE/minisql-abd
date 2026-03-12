using System;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
    public class CreateSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; private set; }

        public CreateSecurityProfile(string profileName)
        {
            ProfileName = profileName;
        }

        public string Execute(Database database)
        {
           
            if (database == null || !database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            
            if (database.SecurityManager.ProfileByName(ProfileName) == null)
            {
                database.SecurityManager.AddProfile(new Profile { Name = ProfileName });
            }

            return Constants.CreateSecurityProfileSuccess;
        }
    }
}
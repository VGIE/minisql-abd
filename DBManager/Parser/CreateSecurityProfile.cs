using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class CreateSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public CreateSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, CreateSecurityProfileSuccess
            if (database.SecurityManager.ProfileByName(this.ProfileName) != null)
            {
                return Constants.Error;
            }
            Profile newProfile = new Profile();
            newProfile.Name = this.ProfileName;
            database.SecurityManager.Profiles.Add(newProfile);

            return Constants.CreateSecurityProfileSuccess;
        
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class AddUser : MiniSqlQuery
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string ProfileName { get; private set; }


        public AddUser(string username, string password, string profileName)
        {
            Username = username;
            Password = password;
            ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            if (!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            var profile = database.SecurityManager.ProfileByName(ProfileName);
            if (profile == null)
                return Constants.SecurityProfileDoesNotExistError;

            var existingUser = database.SecurityManager.UserByName(Username);
            if (existingUser != null)
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            var user = new DbManager.Security.User(Username, Password);

            if (profile.Users == null)
                profile.Users = new List<DbManager.Security.User>();

            profile.Users.Add(user);

            return Constants.AddUserSuccess;
        }

    }
}

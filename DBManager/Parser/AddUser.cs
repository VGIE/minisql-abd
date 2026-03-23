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
            
            if (database == null || !database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            Profile profile = database.SecurityManager.ProfileByName(ProfileName);
            if (profile == null)
                return Constants.SecurityProfileDoesNotExistError;

            
            if (database.SecurityManager.UserByName(Username) != null)
                return Constants.UserAlreadyExist;

            
            if (profile.Users == null)
                profile.Users = new List<User>();
            User newUser = new User();

            newUser.Username = Username;

            newUser.EncryptedPassword = Password;

            
            profile.Users.Add(newUser);

            return Constants.AddUserSuccess;
        }
    }
}
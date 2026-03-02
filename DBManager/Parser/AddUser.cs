using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;
<<<<<<< HEAD

=======
>>>>>>> master
namespace DbManager
{
 
    public class AddUser : MiniSqlQuery
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string ProfileName { get; private set; }


        public AddUser(string username, string password, string profileName)
        {
<<<<<<< HEAD
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
=======
            //TODO DEADLINE 4: Initialize member variables
            this.Username = username;
            this.Password = password;
            this.ProfileName = profileName;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, AddUserSuccess
            Profile profile = database.SecurityManager.ProfileByName(this.ProfileName);
            if (profile == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            if (database.SecurityManager.UserByName(this.Username) != null)
            {
                return Constants.UserAlreadyExist;
            }
            User user = new User(this.Username, this.Password);
            profile.Users.Add(user);

            return Constants.AddUserSuccess;


            
>>>>>>> master
        }

    }
}

using System;
using DbManager.Parser;

namespace DbManager
{
    public class DeleteUser : MiniSqlQuery
    {
        public string Username { get; private set; }

        public DeleteUser(string username)
        {
<<<<<<< HEAD
            Username = username;
=======
            //TODO DEADLINE 4: Initialize member variables
            this.Username = username;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, UserDoesNotExistError, DeleteUserSuccess
            if(!database.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            if(database.SecurityManager.ProfileByUser(this.Username) == null)
            {
                return Constants.UserDoesNotExistError;
            }

            database.SecurityManager.RemoveProfile(this.Username);

            return Constants.DeleteUserSuccess;
            
>>>>>>> master
        }

        public string Execute(Database database)
        {
            if (!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            var user = database.SecurityManager.UserByName(Username);
            if (user == null)
                return Constants.UserDoesNotExistError;

            bool removed = false;

            for (int i = 0; i < database.SecurityManager.Profiles.Count; i++)
            {
                var profile = database.SecurityManager.Profiles[i];
                if (profile?.Users == null)
                    continue;

                for (int j = 0; j < profile.Users.Count; j++)
                {
                    if (profile.Users[j] != null && profile.Users[j].Username == Username)
                    {
                        profile.Users.RemoveAt(j);
                        removed = true;
                        break;
                    }
                }

                if (removed)
                    break;
            }

            if (!removed)
                return Constants.UserDoesNotExistError;

            return Constants.DeleteUserSuccess;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;

        public Manager(string username)
        {
            m_username = username;
        }

        public bool IsUserAdmin()
        {
             //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise
            
            return false;
        }

        public bool IsPasswordCorrect(string username, string password)
        {
           //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            User user = UserByName(username);

            if (user == null)
            {
                return false;
            }
            
            string encryptedPassword = Encryption.Encrypt(password);

            return user.EncryptedPassword == encryptedPassword;
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
             //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            Profile profile = ProfileByName(profileName);

            if (profile == null | string.IsNullOrEmpty(table))
            {
                return;
            }

            profile.RevokePrivilege(table, privilege);
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            
            return false;
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            if (profile == null)
                return;

            foreach (Profile p in Profiles)
            {
                if (p.Name == profile.Name)
                {
                    return; 
                }
            }

            Profiles.Add(profile);
        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            foreach (Profile profile in Profiles)
            {
                foreach (User u in profile.Users)
                    if (u.Username == username)
                    {
                        return u;
                    }
            }

             return null;
        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null

            if (profileName == null || profileName == "")
            {
                return null;
            }

            foreach (Profile p in Profiles)
            {
                if (p.Name == profileName)
                {
                    return p; 
                }
            }

            return null;
        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            foreach (Profile p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (u.Username == username)
                    {
                        return p; 
                    }
                }
            }

            return null;
        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            Profile profileToRemove = ProfileByName(profileName);
            if (profileToRemove == null)
            {
                return false;
            }
            Profiles.Remove(profileToRemove);

            return true;
        }

       public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username

            string fileName = databaseName + ".sec";

            Manager manager = new Manager(username);

            if (!File.Exists(fileName))
                return manager;

            Profile currentProfile = null;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("PROFILE:"))
                    {
                        string profileName = line.Substring("PROFILE:".Length);

                        currentProfile = new Profile { Name = profileName };
                        manager.AddProfile(currentProfile);
                    }
                    else if (line.StartsWith("USER:") && currentProfile != null)
                    {
                        string userData = line.Substring("USER:".Length);
                        string[] parts = userData.Split(',');

                        if (parts.Length == 2)
                        {
                            User user = new User();
                            user.Username = parts[0];
                            user.EncryptedPassword = parts[1];

                            currentProfile.Users.Add(user);
                        }
                    }
                }
            }

            return manager;

        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            string fileName = databaseName + ".sec";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Profile p in Profiles)
                {
                    writer.WriteLine("PROFILE:" + p.Name);
                    foreach (User u in p.Users)
                    {
                        writer.WriteLine("USER:" + u.Username + "," + u.EncryptedPassword);
                    }
                }
            }
        }
        
    }
}
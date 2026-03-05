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
            var profile = ProfileByUser(m_username);
            if (profile == null)
                return false;

            return profile.Name == Profile.AdminProfileName;
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            var user = UserByName(username);
            if (user == null)
                return false;

            string encrypted = EncryptPassword(password);
            return string.Equals(user.EncryptedPassword, encrypted, StringComparison.OrdinalIgnoreCase);
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            var profile = ProfileByName(profileName);
            if (profile == null)
                return;

            profile.GrantPrivilege(table, privilege);
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            var profile = ProfileByName(profileName);
            if (profile == null)
                return;

            profile.RevokePrivilege(table, privilege);
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            var profile = ProfileByUser(username);
            if (profile == null)
                return false;

            return profile.IsGrantedPrivilege(table, privilege);
        }

        public void AddProfile(Profile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.Name))
                return;

            if (ProfileByName(profile.Name) != null)
                return;

            Profiles.Add(profile);
        }

        public User UserByName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            for (int i = 0; i < Profiles.Count; i++)
            {
                var profile = Profiles[i];
                if (profile?.Users == null)
                    continue;

                for (int j = 0; j < profile.Users.Count; j++)
                {
                    var user = profile.Users[j];
                    if (user != null && user.Username == username)
                        return user;
                }
            }

            return null;
        }

        public Profile ProfileByName(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return null;

            for (int i = 0; i < Profiles.Count; i++)
            {
                var p = Profiles[i];
                if (p != null && p.Name == profileName)
                    return p;
            }

            return null;
        }

        public Profile ProfileByUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            for (int i = 0; i < Profiles.Count; i++)
            {
                var profile = Profiles[i];
                if (profile?.Users == null)
                    continue;

                for (int j = 0; j < profile.Users.Count; j++)
                {
                    var user = profile.Users[j];
                    if (user != null && user.Username == username)
                        return profile;
                }
            }

            return null;
        }

        public bool RemoveProfile(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return false;

            var profile = ProfileByName(profileName);
            if (profile == null)
                return false;

            return Profiles.Remove(profile);
        }

        public static Manager Load(string databaseName, string username)
        {
            // Bu projede Save/Load formatý net deðil, o yüzden þimdilik boþ býrakýldý.
            // Eðer sizde dosya formatý/klasör yolu varsa gönder, birebir doldurayým.
            return new Manager(username);
        }

        public void Save(string databaseName)
        {
            // Bu projede Save/Load formatý net deðil, o yüzden þimdilik boþ býrakýldý.
            // Eðer sizde dosya formatý/klasör yolu varsa gönder, birebir doldurayým.
        }

        private static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password ?? string.Empty);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
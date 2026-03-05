using System;
using System.Security.Cryptography;
using System.Text;

namespace DbManager.Security
{
    public class User
    {
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        public User(string username, string password)
        {
            Username = username;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password ?? string.Empty);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                EncryptedPassword = sb.ToString();
            }
        }

        public User() { }
    }
}
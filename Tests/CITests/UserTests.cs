
/*using System.Security.Cryptography;
using System.Text;
using DbManager.Security;
using Xunit;

namespace OurTests
{
    public class UserTests
    {
        [Fact]
        public void Constructor_SetsUsername()
        {
            User user = new User("ahmet", "1234");

            Assert.Equal("ahmet", user.Username);
        }

        [Fact]
        public void Constructor_EncryptsPassword()
        {
            User user = new User("ahmet", "1234");

            Assert.NotNull(user.EncryptedPassword);
            Assert.NotEqual("1234", user.EncryptedPassword);
        }

        [Fact]
        public void Constructor_UsesSha256Hash_ForPassword()
        {
            User user = new User("ahmet", "1234");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes("1234");
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                string expectedHash = sb.ToString();

                Assert.Equal(expectedHash, user.EncryptedPassword);
            }
        }

        [Fact]
        public void Constructor_SamePassword_ProducesSameHash()
        {
            User user1 = new User("ahmet", "1234");
            User user2 = new User("mehmet", "1234");

            Assert.Equal(user1.EncryptedPassword, user2.EncryptedPassword);
        }

        [Fact]
        public void Constructor_DifferentPasswords_ProduceDifferentHashes()
        {
            User user1 = new User("ahmet", "1234");
            User user2 = new User("ahmet", "abcd");

            Assert.NotEqual(user1.EncryptedPassword, user2.EncryptedPassword);
        }

        [Fact]
        public void Constructor_NullPassword_UsesEmptyStringHash()
        {
            User user = new User("ahmet", null);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(string.Empty);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                string expectedHash = sb.ToString();

                Assert.Equal(expectedHash, user.EncryptedPassword);
            }
        }

        [Fact]
        public void EmptyConstructor_CreatesObject()
        {
            User user = new User();

            Assert.NotNull(user);
        }
    }
}*/
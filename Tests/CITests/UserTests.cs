
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
using DbManager.Security;
using Xunit;

namespace OurTests
{
    public class UserTests
    {
        [Fact]
        public void Constructor_SetsUsername()
        {
            var user = new User("zeynep", "secret123");

            Assert.Equal("zeynep", user.Username);
        }

        [Fact]
        public void Constructor_DoesNotStorePlainPassword()
        {
            const string plain = "secret123";
            var user = new User("zeynep", plain);

            Assert.NotNull(user.EncryptedPassword);
            Assert.NotEqual(plain, user.EncryptedPassword);
        }

        [Fact]
        public void Constructor_EncryptedPassword_MatchesEncryptionHelper()
        {
            const string plain = "secret123";
            var user = new User("zeynep", plain);

            Assert.Equal(Encryption.Encrypt(plain), user.EncryptedPassword);
        }

        [Fact]
        public void Constructor_SamePassword_ProducesSameEncryptedPassword_ForDifferentUsernames()
        {
            var first = new User("zeynep", "samepass");
            var second = new User("bob", "samepass");

            Assert.Equal(first.EncryptedPassword, second.EncryptedPassword);
        }

        [Fact]
        public void Constructor_DifferentPasswords_ProduceDifferentEncryptedPasswords()
        {
            var first = new User("zeynep", "firstpass");
            var second = new User("zeynep", "secondpass");

            Assert.NotEqual(first.EncryptedPassword, second.EncryptedPassword);
        }

        [Fact]
        public void Constructor_NullPassword_EncryptedPassword_IsNull()
        {
            var user = new User("zeynep", null);

            Assert.Null(user.EncryptedPassword);
        }

        [Fact]
        public void ParameterlessConstructor_CreatesInstance()
        {
            var user = new User();

            Assert.NotNull(user);
        }
    }
}
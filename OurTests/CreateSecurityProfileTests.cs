using DbManager;
using Xunit;

namespace OurTests
{
    public class CreateSecurityProfileTests
    {
        [Fact]
        public void Constructor_SetsProfileName()
        {
            CreateSecurityProfile query = new CreateSecurityProfile("Admins");

            Assert.Equal("Admins", query.ProfileName);
        }

        [Fact]
        public void Execute_NullDatabase_ReturnsRequiredPrivilegeError()
        {
            CreateSecurityProfile query = new CreateSecurityProfile("Admins");

            string result = query.Execute(null);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
        }

        [Fact]
        public void Execute_TestDatabase_ReturnsRequiredPrivilegeError()
        {
            Database database = Database.CreateTestDatabase();
            CreateSecurityProfile query = new CreateSecurityProfile("Admins");

            string result = query.Execute(database);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
        }

        [Fact]
        public void Execute_TestDatabase_DoesNotCreateProfile_WhenUserIsNotAdmin()
        {
            Database database = Database.CreateTestDatabase();
            CreateSecurityProfile query = new CreateSecurityProfile("Admins");

            query.Execute(database);

            Assert.Null(database.SecurityManager.ProfileByName("Admins"));
        }
    }
}
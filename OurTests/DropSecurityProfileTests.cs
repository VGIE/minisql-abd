using DbManager;
using Xunit;

namespace OurTests
{
    public class DropSecurityProfileTests
    {
        [Fact]
        public void Constructor_SetsProfileName()
        {
            DropSecurityProfile query = new DropSecurityProfile("Admins");

            Assert.Equal("Admins", query.ProfileName);
        }

        [Fact]
        public void Execute_TestDatabase_ReturnsRequiredPrivilegeError()
        {
            Database database = Database.CreateTestDatabase();
            DropSecurityProfile query = new DropSecurityProfile("Admins");

            string result = query.Execute(database);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
        }

        [Fact]
        public void Execute_TestDatabase_ProfileDoesNotExistButStillReturnsPrivilegeError_First()
        {
            Database database = Database.CreateTestDatabase();
            DropSecurityProfile query = new DropSecurityProfile("NotExistingProfile");

            string result = query.Execute(database);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
        }

        [Fact]
        public void Execute_TestDatabase_DoesNotRemoveAnything_WhenUserIsNotAdmin()
        {
            Database database = Database.CreateTestDatabase();
            DropSecurityProfile query = new DropSecurityProfile("Admins");

            string result = query.Execute(database);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
            Assert.Null(database.SecurityManager.ProfileByName("Admins"));
        }
    }
}
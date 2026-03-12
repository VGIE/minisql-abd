using System;
using Xunit;
using DbManager;

namespace SecurityParsingTests
{
    public class AddUserTests
    {
        [Fact]
        public void Correct()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("user", query.Username);
            Assert.Equal("password", query.Password);
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("ADD USER (User,Password,Profile)") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("User", query.Username);
            Assert.Equal("Password", query.Password);
            Assert.Equal("Profile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            AddUser query = MiniSQLParser.Parse("ADD     USER      (user,password,profile)") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("user", query.Username);
            Assert.Equal("password", query.Password);
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("ADD USER     (OtherUser,password123,profile1)") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("OtherUser", query.Username);
            Assert.Equal("password123", query.Password);
            Assert.Equal("profile1", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpacesInsideParentheses()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER ( user , password , profile )") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("user", query.Username);
            Assert.Equal("password", query.Password);
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("ADD USER (   admin   ,   12345   ,   Admins   )") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("admin", query.Username);
            Assert.Equal("12345", query.Password);
            Assert.Equal("Admins", query.ProfileName);
        }

        [Fact]
        public void CorrectWithoutSemicolonAndWithSemicolon()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("user", query.Username);

            query = MiniSQLParser.Parse("ADD USER (user,password,profile);") as AddUser;

            Assert.NotNull(query);
            Assert.Equal("user", query.Username);
            Assert.Equal("password", query.Password);
            Assert.Equal("profile", query.ProfileName);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);

            query = MiniSQLParser.Parse("Add User (user,password,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("add user (user,password,profile)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectUserWithForbiddenChars()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);

            query = MiniSQLParser.Parse("ADD USER (user_1,password,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user 1,password,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (1user,password,profile)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);

            query = MiniSQLParser.Parse("ADD USER (user,password,profile_1)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user,password,profile 1)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user,password,1profile)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);

            query = MiniSQLParser.Parse("ADD USER ()") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (,,)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user,password)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectWithMissingUsernameOrPasswordOrProfile()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (,password,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user,,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user,password,)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectWithTooManyParameters()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile,extra)") as AddUser;

            Assert.Null(query);
        }

        [Fact]
        public void IncorrectWithoutParentheses()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER user,password,profile") as AddUser;

            Assert.Null(query);
        }
    }
}
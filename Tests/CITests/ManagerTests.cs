using DbManager;
using DbManager.Security;
using System;
using Xunit;

namespace SecurityParsingTests
{
    public class ManagerTests
    {
        [Fact]
        public void AddProfile()
        {
            Manager manager = new Manager("admin");

            Profile profile1 = new Profile { Name = "Sales" };
            manager.AddProfile(profile1);
            Assert.Equal(1, manager.Profiles.Count);
            Assert.Equal("Sales", manager.Profiles[0].Name);

            Profile profileDuplicate = new Profile { Name = "Sales" };
            manager.AddProfile(profileDuplicate);
            Assert.Equal(1, manager.Profiles.Count); // no se añade

            manager.AddProfile(null);
            Assert.Equal(1, manager.Profiles.Count); // sigue sin cambios

            Profile profile2 = new Profile { Name = "HR" };
            manager.AddProfile(profile2);
            Assert.Equal(2, manager.Profiles.Count);
            Assert.Equal("HR", manager.Profiles[1].Name);
        }

        [Fact]
        public void ProfileByName()
        {
            Manager manager = new Manager("admin");

            Profile profile1 = new Profile { Name = "HR" };
            Profile profile2 = new Profile { Name = "Sales" };
            manager.AddProfile(profile1);
            manager.AddProfile(profile2);

            Profile result1 = manager.ProfileByName("HR");
            Assert.NotNull(result1);
            Assert.Equal("HR", result1.Name);

            Profile result2 = manager.ProfileByName("Sales");
            Assert.NotNull(result2);
            Assert.Equal("Sales", result2.Name);

            Profile result3 = manager.ProfileByName("Finance");
            Assert.Null(result3);

            Profile result4 = manager.ProfileByName("");
            Assert.Null(result4);

            Profile result5 = manager.ProfileByName(null);
            Assert.Null(result5);
        }
        [Fact]
        public void UserbyName()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Estudiantes" };
            User user = new User();
            user.Username = "danna";
            user.EncryptedPassword = Encryption.Encrypt("1234");
            profile.Users.Add(user);
            manager.AddProfile(profile);
            User result = manager.UserByName("danna");
            Assert.NotNull(result);
            Assert.Equal("danna", result.Username);

        }
        [Fact]
        public void Remove()
        {

        }
        [Fact]
        public void RemoveNull()
        {

        }

        [Fact]
        public void Save()
        {

        }
        public void SaveNull()
        {

        }
    }
}
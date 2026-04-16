using DbManager;
using DbManager.Security;
using System;
using System.IO;
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
            Assert.Single(manager.Profiles);
            Assert.Equal("Sales", manager.Profiles[0].Name);

            Profile profileDuplicate = new Profile { Name = "Sales" };
            manager.AddProfile(profileDuplicate);
            Assert.Single(manager.Profiles);

            manager.AddProfile(null);
            Assert.Single(manager.Profiles);

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
        public void IsPasswordCorrect_ValidCredentials_ReturnsTrue()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "TestProfile" };

            User user = new User();
            user.Username = "ana";
            user.EncryptedPassword = Encryption.Encrypt("pass123");

            profile.Users.Add(user);
            manager.AddProfile(profile);

            bool result = manager.IsPasswordCorrect("ana", "pass123");

            Assert.True(result);
        }

        [Fact]
        public void IsPasswordCorrect_InvalidPassword_ReturnsFalse()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "TestProfile" };

            User user = new User();
            user.Username = "ana";
            user.EncryptedPassword = Encryption.Encrypt("pass123");

            profile.Users.Add(user);
            manager.AddProfile(profile);

            bool result = manager.IsPasswordCorrect("ana", "wrongpass");

            Assert.False(result);
        }

        [Fact]
        public void RevokePrivilege_RemovesPrivilegeFromProfile()
        {
            Manager manager = new Manager("admin");
            string profileName = "ManagerProfile";
            string tableName = "Sales";
            Profile profile = new Profile { Name = profileName };
            manager.AddProfile(profile);

            Privilege myPrivilege = Privilege.Select;
            profile.GrantPrivilege(tableName, myPrivilege);

            manager.RevokePrivilege(profileName, tableName, myPrivilege);

            bool hasPrivilege = profile.IsGrantedPrivilege(tableName, myPrivilege);
            Assert.False(hasPrivilege);
        }

        [Fact]
        public void ProfileByUser_ReturnsCorrectProfile_IfUserExists()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "IT_Dept" };
            User user = new User();
            user.Username = "dev_user";

            profile.Users.Add(user);
            manager.AddProfile(profile);

            Profile result = manager.ProfileByUser("dev_user");

            Assert.NotNull(result);
            Assert.Equal("IT_Dept", result.Name);
        }

        [Fact]
        public void ProfileByUser_ReturnsNull_IfUserDoesNotExist()
        {
            Manager manager = new Manager("admin");

            Profile result = manager.ProfileByUser("unknown");

            Assert.Null(result);
        }
        [Fact]
        public void UserbyNameNull()
        {
            Manager manager = new Manager("admin");
            User resultFantasma = manager.UserByName("pedro");
            User resultNulo = manager.UserByName(null);
            Assert.Null(resultFantasma);
            Assert.Null(resultNulo);

        }
        



        [Fact]
        public void LoadTest()
        {
            string dbName = "loaddb";
            string fileName = dbName + ".sec";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("PROFILE:Students");
                writer.WriteLine("USER:ana," + Encryption.Encrypt("1234"));
                writer.WriteLine("USER:juan," + Encryption.Encrypt("abcd"));
                writer.WriteLine("PROFILE:Teachers");
                writer.WriteLine("USER:carlos," + Encryption.Encrypt("pass"));
            }

            Manager manager = Manager.Load(dbName, "admin");

            Assert.Equal(2, manager.Profiles.Count);

            Profile students = manager.ProfileByName("Students");
            Assert.NotNull(students);
            Assert.Equal(2, students.Users.Count);
            Assert.Equal("ana", students.Users[0].Username);
            Assert.Equal("juan", students.Users[1].Username);

            Profile teachers = manager.ProfileByName("Teachers");
            Assert.NotNull(teachers);
            Assert.Single(teachers.Users);
            Assert.Equal("carlos", teachers.Users[0].Username);
        }

        [Fact]
        public void Load_FileDoesNotExist()
        {
            Manager manager = Manager.Load("db_inexistente", "admin");

            Assert.Empty(manager.Profiles);
        }

        [Fact]
        public void SaveAndLoadWithIncorrectCredentials()
        {
            string dbName = "incorrectpassdb";

            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Students" };

            User user = new User("ana", "1234");
            profile.Users.Add(user);
            manager.AddProfile(profile);

            manager.Save(dbName);

            Manager loadedManager = Manager.Load(dbName, "admin");

            bool result = loadedManager.IsPasswordCorrect("ana", "wrongpassword");

            Assert.False(result);
        }

        [Fact]
        public void SaveAndLoadWithIncorrectCredentials2()
        {
            string dbName = "nouserdb";

            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Students" };

            User user = new User("ana", "1234");
            profile.Users.Add(user);
            manager.AddProfile(profile);

            manager.Save(dbName);

            Manager loadedManager = Manager.Load(dbName, "admin");

            bool result = loadedManager.IsPasswordCorrect("pedro", "1234");

            Assert.False(result);
        }
         [Fact]
        public void Remove()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Estudiantes" };
            manager.AddProfile(profile);
            Assert.Single(manager.Profiles);
            bool result = manager.RemoveProfile("Estudiantes");
            Assert.True(result);
            Assert.Empty(manager.Profiles);

        }
        [Fact]
        public void RemoveNull()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile(null);
            Assert.False(result);

        }
        [Fact]
        public void Remove_Dont_Exist()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile(null);
            Assert.False(result);

        }

        [Fact]
        public void Save()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Estudiantes" };
            
            User user = new User();
            user.Username = "danna";
            user.EncryptedPassword = Encryption.Encrypt("1234");
            
            profile.Users.Add(user);
            manager.AddProfile(profile);

            string nombreBD = "BaseNormal";
            manager.Save(nombreBD);
            Assert.True(System.IO.File.Exists(nombreBD + ".sec"));
            if (System.IO.File.Exists(nombreBD + ".sec"))
            {
                System.IO.File.Delete(nombreBD + ".sec");
            }

        }
        public void SaveNull()
        {
            Manager manager = new Manager("admin");
            manager.Save(null);
            manager.Save("");
            Assert.False(System.IO.File.Exists(".sec"));

        }
        [Fact]
        public void Save_ManagerVacio()
        {
            Manager manager = new Manager("admin");
            string nombreBD = "BaseVacia";
            manager.Save(nombreBD);
            Assert.True(System.IO.File.Exists(nombreBD + ".sec"));
            if (System.IO.File.Exists(nombreBD + ".sec"))
            {
                System.IO.File.Delete(nombreBD + ".sec");
            }
        }
    }
}
    }
}
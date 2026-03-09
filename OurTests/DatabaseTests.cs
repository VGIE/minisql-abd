using System.ComponentModel.DataAnnotations;
using DbManager;

namespace OurTests
{
    public class DatabaseTests
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        /*
        [Fact]
        
        public void CreateTable_ValidName_True()
        {
            Database db = new DataBase("admin", "password123");
            List <ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition("id", DataType.Integer);
                new ColumnDefinition("name", DataType.String);
           
            }

            bool result = db.CreateTable("Users",columns);

            Assert.True(result);
            Assert.Equal(Constants.CreateTableSuccess, db.LastErrorMessage);
        }

        [Fact]
        public void CreateTable_DuplicateName_False()
        {
            Database db = new Database("admin", "password123");
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition("id", DataType.Integer);
            }
            db.CreateTable("Users",columns);
            bool result =
        }*/
        [Fact]
        public void InsertTest()
        {
            Database database = Database.CreateTestDatabase();
            List<string> newRow = new List<string>()
            {
                "1","Danna"
            };
            bool result = database.Insert("TestTable", newRow);
            Assert.True(result);
            Assert.Equal(Constants.InsertSuccess, database.LastErrorMessage);

        }
        [Fact]
        public void InsertVoidTest()
        {
            Database database = Database.CreateTestDatabase();
            List<string> newRow = new List<string>()
            {
                "1","Danna"
            };
            bool result = database.Insert("TablaInventada", newRow);
            Assert.False(result);
            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);
        }
        [Fact]
        public void InsertWrongTest()
        {
            Database database = Database.CreateTestDatabase();
            List<string> badRow = new List<string>()
            {
                "1","Danna","Bad"

            };
            bool result = database.Insert("TestTable", badRow);
            Assert.False(result);
            Assert.Equal(Constants.ColumnCountsDontMatch, database.LastErrorMessage);

        }

    }
}
using DbManager;
using DbManager.Parser;
using System.ComponentModel.DataAnnotations;

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
        public void DeleteWhereTest()
        {
            Database database = Database.CreateTestDatabase();

            Condition condition = new Condition("Id", "=", "1");

            bool result = database.DeleteWhere("TestTable", condition);

            Assert.True(result);
            Assert.Equal(Constants.DeleteSuccess, database.LastErrorMessage);
        }

        [Fact]
        public void UpdateTest()
        {
            Database database = Database.CreateTestDatabase();

            List<SetValue> setValues = new List<SetValue>()
            {
                new SetValue("Name", "David")
            };

            Condition condition = new Condition("Id", "=", "1");

            bool result = database.Update("TestTable", setValues, condition);

            Assert.True(result);
            Assert.Equal(Constants.UpdateSuccess, database.LastErrorMessage);
        }

        [Fact]
        public void UpdateErrorsTest()
        {
            Database database = Database.CreateTestDatabase();

            List<SetValue> setValues = new List<SetValue>()
            {
                new SetValue("FakeColumn", "David")
            };

            Condition condition = new Condition("Id", "=", "1");

            bool result = database.Update("TestTable", setValues, condition);

            Assert.False(result);
            Assert.Equal(Constants.ColumnDoesNotExistError, database.LastErrorMessage);
        }
    }
}
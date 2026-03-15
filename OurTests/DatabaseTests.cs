using DbManager;
using DbManager.Parser;
using System.ComponentModel.DataAnnotations;

namespace OurTests
{
    public class DatabaseTests
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        
        [Fact]
        
        public void CreateTable_ValidName_True()
        {
            Database db = new Database("Besma","password123");
            List <ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "id"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "name")
           
            };

            bool result = db.CreateTable("Users",columns);

            Assert.True(result);
            Assert.Equal(Constants.CreateTableSuccess, db.LastErrorMessage);
        }

        [Fact]
        public void CreateTable_DuplicateName_False()
        {
            Database db = new Database("Besma", "password123");
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.Int, "id"),
            };
            db.CreateTable("Users",columns);
            bool result = db.CreateTable("Users", columns);

            Assert.False(result);
            Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);
        }
        [Fact]
        public void AddTable_True()
        {
            Database db = new Database("Besma", "password123");
            Table table = Table.CreateTestTable();

            bool result = db.AddTable(table);

            Assert.True(result);
            Assert.NotNull(db.TableByName(Table.TestTableName));
        }

        [Fact]
        public void TableByName_ReturnsTable()
        {
            Database db = new Database("Besma", "password123");
            db.AddTable(Table.CreateTestTable());
            Table result = db.TableByName(Table.TestTableName);

            Assert.NotNull(result);
            Assert.Equal(Table.TestTableName, result.Name);
        }

        [Fact]
        public void TableByName_ReturnsNull()
        {
            Database db = new Database("Besma", "password123");

            Table result = db.TableByName("Does not Exist");
            Assert.Null(result);
        }

        [Fact]
        public void DataBaseTest()
        {
            Database db = new Database("Besma", "password123");
            Assert.NotNull(db.SecurityManager);
            Assert.Null(db.TableByName("random"));
        }


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
        [Fact]
        public void SelectVoidTest()
        {
            Database database = Database.CreateTestDatabase();
            List<string> columns = new List<string>()
            {
                "Name"
            };
            Table result = database.Select("TableNull", columns, null);
            Assert.Null(result);
            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);
        }
        [Fact]
        public void SelectWrongTest()
        {
            Database database = Database.CreateTestDatabase();
            List<string> columns = new List<string>()
            { "Id","Ciudad"};
            Table result = database.Select("TestTable", columns, null);
            Assert.Null(result);
            Assert.Equal(Constants.ColumnDoesNotExistError, database.LastErrorMessage);
        }
        [Fact]
        public void SelectSuccessTest()
        {
            Database database = Database.CreateTestDatabase();
            List<string> select = new List<string>()
            {
                "Id"
            };
            Table result = database.Select("TestTable", select, null);
            Assert.NotNull(result);
            Assert.Equal(1, result.NumColumns());
            Assert.Equal("Id", result.GetColumn(0).Name);

        }



        [Fact]
        public void DeleteWhere()
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
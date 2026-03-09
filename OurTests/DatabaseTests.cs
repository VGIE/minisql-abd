using System.ComponentModel.DataAnnotations;
using DbManager;

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
    }
}
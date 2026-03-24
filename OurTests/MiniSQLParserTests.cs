using System.Collections.Generic;
using DbManager;
using DbManager.Parser;
using Xunit;

namespace OurTests
{
    public class MiniSQLParserTests
    {
        [Fact]
        public void Parse_NullAndEmpty_ReturnsNull()
        {
            Assert.Null(MiniSQLParser.Parse(null));
            Assert.Null(MiniSQLParser.Parse("   "));
            Assert.Null(MiniSQLParser.Parse("INVALID QUERY"));
        }

        [Fact]
        public void Parse_Select_Success()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("SELECT * FROM Students;");
            Assert.NotNull(result);
            Select select = Assert.IsType<Select>(result);
            Assert.Equal("Students", select.Table);
            Assert.Equal("*", select.Columns[0]);

            Assert.NotNull(MiniSQLParser.Parse("SELECT Name, Age FROM Users WHERE Age >= 18;"));
            Assert.NotNull(MiniSQLParser.Parse("SELECT Name FROM Users WHERE Name = 'Ahmet';"));
        }

        [Fact]
        public void Parse_Insert_Success()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("INSERT INTO Students VALUES ('Ahmet', 21);");
            Insert insert = Assert.IsType<Insert>(result);
            Assert.Equal("Students", insert.Table);
            Assert.Equal("Ahmet", insert.Values[0]);
            Assert.Equal("21", insert.Values[1]);
        }

        [Fact]
        public void Parse_Update_Success()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("UPDATE Students SET Name = 'Ali', Age = 20 WHERE Id = 1;");
            Update update = Assert.IsType<Update>(result);
            Assert.Equal("Students", update.Table);
            Assert.Equal(2, update.Columns.Count);
        }

        [Fact]
        public void Parse_Security_Success()
        {
            Assert.NotNull(MiniSQLParser.Parse("CREATE SECURITY PROFILE Admins;"));
            Assert.NotNull(MiniSQLParser.Parse("GRANT SELECT ON Students TO Admins;"));
            Assert.NotNull(MiniSQLParser.Parse("ADD USER (ahmet, 12345, Admins);"));
            Assert.NotNull(MiniSQLParser.Parse("DELETE USER ahmet;"));
        }

        [Fact]
        public void Parse_Negative_ReturnsNull()
        {
            Assert.Null(MiniSQLParser.Parse("SELECT * Students;"));
            Assert.Null(MiniSQLParser.Parse("INSERT Students VALUES ('Pepe');"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE Students ();"));
            Assert.Null(MiniSQLParser.Parse("GRANT DANCE ON Students TO Admins;"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM Students;"));
            Assert.Null(MiniSQLParser.Parse("UPDATE Students SET WHERE Id = 1;"));
        }

        [Fact]
        public void Database_Logic_Integration_Test()
        {
            Database db = new Database("admin", "adminPass");

            List<ColumnDefinition> cols = new List<ColumnDefinition> {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            db.CreateTable("Employees", cols);
            Assert.NotNull(db.TableByName("Employees"));

            db.Insert("Employees", new List<string> { "Zeynep", "22" });
            Table selectResult = db.Select("Employees", new List<string> { "*" }, null);

            Assert.Equal(1, selectResult.NumRows());
            Assert.Equal("Zeynep", selectResult.GetRow(0).Values[0]);

            db.Update("Employees", new List<SetValue> { new SetValue("Age", "23") }, new Condition("Name", "=", "Zeynep"));
            Assert.Equal("23", db.TableByName("Employees").GetRow(0).Values[1]);

            db.DeleteWhere("Employees", new Condition("Age", ">", "20"));
            Assert.Equal(0, db.TableByName("Employees").NumRows());
        }

        [Fact]
        public void Database_Error_Handling_Test()
        {
            Database db = new Database("admin", "adminPass");

            db.CreateTable("Test", new List<ColumnDefinition> { new ColumnDefinition(ColumnDefinition.DataType.Int, "ID") });
            bool createDuplicate = db.CreateTable("Test", new List<ColumnDefinition> { new ColumnDefinition(ColumnDefinition.DataType.Int, "ID") });

            Assert.False(createDuplicate);
            Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);

            Assert.False(db.Insert("NonExistent", new List<string> { "val" }));
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
        }
    }
}
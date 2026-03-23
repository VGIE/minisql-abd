using System.Collections.Generic;
using DbManager;
using DbManager.Parser;
using Xunit;

namespace OurTests
{
    public class MiniSQLParserTests
    {
        [Fact]
        public void Parse_NullQuery_ReturnsNull()
        {
            MiniSqlQuery result = MiniSQLParser.Parse(null);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_EmptyQuery_ReturnsNull()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("   ");

            Assert.Null(result);
        }

        [Fact]
        public void Parse_InvalidQuery_ReturnsNull()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("INVALID QUERY");

            Assert.Null(result);
        }

        [Fact]
        public void Parse_SelectAll_ReturnsSelectObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("SELECT * FROM Students;");

            Assert.NotNull(result);
            Select select = Assert.IsType<Select>(result);

            Assert.Equal("Students", select.Table);
            Assert.Single(select.Columns);
            Assert.Equal("*", select.Columns[0]);
            Assert.Null(select.Where);
        }

        [Fact]
        public void Parse_SelectColumnsWithoutWhere_ReturnsSelectObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("SELECT Name, Age FROM Users;");

            Assert.NotNull(result);
            Select select = Assert.IsType<Select>(result);

            Assert.Equal("Users", select.Table);
            Assert.Equal(2, select.Columns.Count);
            Assert.Equal("Name", select.Columns[0]);
            Assert.Equal("Age", select.Columns[1]);
            Assert.Null(select.Where);
        }

        [Fact]
        public void Parse_SelectWithWhere_ReturnsSelectObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("SELECT Name, Age FROM Users WHERE Age >= 18;");

            Assert.NotNull(result);
            Select select = Assert.IsType<Select>(result);

            Assert.Equal("Users", select.Table);
            Assert.Equal(2, select.Columns.Count);
            Assert.Equal("Name", select.Columns[0]);
            Assert.Equal("Age", select.Columns[1]);

            Assert.NotNull(select.Where);
            Assert.Equal("Age", select.Where.ColumnName);
            Assert.Equal(">=", select.Where.Operator);
            Assert.Equal("18", select.Where.LiteralValue);
        }

        [Fact]
        public void Parse_SelectWithStringWhere_ReturnsUnquotedValue()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("SELECT Name FROM Users WHERE Name = 'Ahmet';");

            Assert.NotNull(result);
            Select select = Assert.IsType<Select>(result);

            Assert.Equal("Users", select.Table);
            Assert.Single(select.Columns);
            Assert.Equal("Name", select.Columns[0]);

            Assert.NotNull(select.Where);
            Assert.Equal("Name", select.Where.ColumnName);
            Assert.Equal("=", select.Where.Operator);
            Assert.Equal("Ahmet", select.Where.LiteralValue);
        }

        [Fact]
        public void Parse_Insert_ReturnsInsertObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("INSERT INTO Students VALUES ('Ahmet', 21);");

            Assert.NotNull(result);
            Insert insert = Assert.IsType<Insert>(result);

            Assert.Equal("Students", insert.Table);
            Assert.Equal(2, insert.Values.Count);
            Assert.Equal("Ahmet", insert.Values[0]);
            Assert.Equal("21", insert.Values[1]);
        }

        [Fact]
        public void Parse_InsertWithCommaInsideString_ReturnsCorrectValues()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("INSERT INTO Notes VALUES ('Hello, world', 5);");

            Assert.NotNull(result);
            Insert insert = Assert.IsType<Insert>(result);

            Assert.Equal("Notes", insert.Table);
            Assert.Equal(2, insert.Values.Count);
            Assert.Equal("Hello, world", insert.Values[0]);
            Assert.Equal("5", insert.Values[1]);
        }

        [Fact]
        public void Parse_DropTable_ReturnsDropTableObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("DROP TABLE Products;");

            Assert.NotNull(result);
            DropTable dropTable = Assert.IsType<DropTable>(result);

            Assert.Equal("Products", dropTable.Table);
        }

        [Fact]
        public void Parse_CreateTable_ReturnsCreateTableObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("CREATE TABLE Students (STRING Name, INT Age, DOUBLE Grade);");

            Assert.NotNull(result);
            CreateTable createTable = Assert.IsType<CreateTable>(result);

            Assert.Equal("Students", createTable.Table);
            Assert.Equal(3, createTable.ColumnsParameters.Count);

            Assert.Equal("Name", createTable.ColumnsParameters[0].Name);
            Assert.Equal(ColumnDefinition.DataType.String, createTable.ColumnsParameters[0].Type);

            Assert.Equal("Age", createTable.ColumnsParameters[1].Name);
            Assert.Equal(ColumnDefinition.DataType.Int, createTable.ColumnsParameters[1].Type);

            Assert.Equal("Grade", createTable.ColumnsParameters[2].Name);
            Assert.Equal(ColumnDefinition.DataType.Double, createTable.ColumnsParameters[2].Type);
        }

        [Fact]
        public void Parse_CreateTableInvalidType_ReturnsNull()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("CREATE TABLE Test (BOOL Active);");

            Assert.Null(result);
        }

        [Fact]
        public void Parse_UpdateSingleColumn_ReturnsUpdateObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("UPDATE Students SET Name = 'Mehmet' WHERE Id = 5;");

            Assert.NotNull(result);
            Update update = Assert.IsType<Update>(result);

            Assert.Equal("Students", update.Table);
            Assert.Single(update.Columns);

            Assert.Equal("Name", update.Columns[0].ColumnName);
            Assert.Equal("Mehmet", update.Columns[0].Value);

            Assert.NotNull(update.Where);
            Assert.Equal("Id", update.Where.ColumnName);
            Assert.Equal("=", update.Where.Operator);
            Assert.Equal("5", update.Where.LiteralValue);
        }

        [Fact]
        public void Parse_UpdateMultipleColumns_ReturnsUpdateObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("UPDATE Students SET Name = 'Ali', Age = 20 WHERE Id = 1;");

            Assert.NotNull(result);
            Update update = Assert.IsType<Update>(result);

            Assert.Equal("Students", update.Table);
            Assert.Equal(2, update.Columns.Count);

            Assert.Equal("Name", update.Columns[0].ColumnName);
            Assert.Equal("Ali", update.Columns[0].Value);

            Assert.Equal("Age", update.Columns[1].ColumnName);
            Assert.Equal("20", update.Columns[1].Value);

            Assert.NotNull(update.Where);
            Assert.Equal("Id", update.Where.ColumnName);
            Assert.Equal("=", update.Where.Operator);
            Assert.Equal("1", update.Where.LiteralValue);
        }

        [Fact]
        public void Parse_Delete_ReturnsDeleteObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("DELETE FROM Students WHERE Age < 18;");

            Assert.NotNull(result);
            Delete delete = Assert.IsType<Delete>(result);

            Assert.Equal("Students", delete.Table);
            Assert.NotNull(delete.Where);
            Assert.Equal("Age", delete.Where.ColumnName);
            Assert.Equal("<", delete.Where.Operator);
            Assert.Equal("18", delete.Where.LiteralValue);
        }

        //[Fact]
        //public void Parse_DeleteWithExtraSpaces_ReturnsCorrectDeleteObject()
        //{
        //    string query = "  DELETE   FROM    Students   WHERE  Age  >  18  ;  ";
        //    MiniSqlQuery result = MiniSQLParser.Parse(query);

        //    Assert.NotNull(result);
        //    Delete delete = Assert.IsType<Delete>(result);

        //    Assert.Equal("Students", delete.Table);
        //    Assert.Equal("Age", delete.Where.ColumnName);
        //    Assert.Equal(">", delete.Where.Operator);
        //    Assert.Equal("18", delete.Where.LiteralValue);
        //}

        //[Fact]
        //public void Parse_DeleteWithCase_ReturnsCorrectDeleteObject()
        //{
        //    string query = "DELETE FROM Students WHERE Name = 'ANNE';";
        //    MiniSqlQuery result = MiniSQLParser.Parse(query);

        //    Assert.NotNull(result);
        //    Delete delete = Assert.IsType<Delete>(result);

        //    Assert.Equal("Students", delete.Table);
        //    Assert.Equal("Name", delete.Where.ColumnName);

        //    Assert.Equal("ANNE", delete.Where.LiteralValue.Replace("'", ""));
        //}

        //[Fact]
        //public void Parse_DeleteSimpleStringCondition_ReturnsDeleteObject()
        //{
        //    string query = "DELETE FROM Employees WHERE City = 'Vitoria';";
        //    MiniSqlQuery result = MiniSQLParser.Parse(query);

        //    Assert.NotNull(result);
        //    Delete delete = Assert.IsType<Delete>(result);

        //    Assert.Equal("Employees", delete.Table);
        //    Assert.Equal("City", delete.Where.ColumnName);
        //    Assert.Equal("=", delete.Where.Operator);

        //    Assert.Equal("Vitoria", delete.Where.LiteralValue);
        //}

        [Fact]
        public void Parse_CreateSecurityProfile_ReturnsCreateSecurityProfileObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("CREATE SECURITY PROFILE Admins;");

            Assert.NotNull(result);
            CreateSecurityProfile profile = Assert.IsType<CreateSecurityProfile>(result);

            Assert.Equal("Admins", profile.ProfileName);
        }

        [Fact]
        public void Parse_DropSecurityProfile_ReturnsDropSecurityProfileObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("DROP SECURITY PROFILE Guests;");

            Assert.NotNull(result);
            DropSecurityProfile profile = Assert.IsType<DropSecurityProfile>(result);

            Assert.Equal("Guests", profile.ProfileName);
        }

        [Fact]
        public void Parse_Grant_ReturnsGrantObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("GRANT SELECT ON Students TO Admins;");

            Assert.NotNull(result);
            Grant grant = Assert.IsType<Grant>(result);

            Assert.Equal("SELECT", grant.PrivilegeName);
            Assert.Equal("Students", grant.TableName);
            Assert.Equal("Admins", grant.ProfileName);
        }

        [Fact]
        public void Parse_Revoke_ReturnsRevokeObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("REVOKE UPDATE ON Students TO Editors;");

            Assert.NotNull(result);
            Revoke revoke = Assert.IsType<Revoke>(result);

            Assert.Equal("UPDATE", revoke.PrivilegeName);
            Assert.Equal("Students", revoke.TableName);
            Assert.Equal("Editors", revoke.ProfileName);
        }

        [Fact]
        public void Parse_AddUser_ReturnsAddUserObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("ADD USER (ahmet, 12345, Admins);");

            Assert.NotNull(result);
            AddUser addUser = Assert.IsType<AddUser>(result);

            Assert.Equal("ahmet", addUser.Username);
            Assert.Equal("12345", addUser.Password);
            Assert.Equal("Admins", addUser.ProfileName);
        }

        [Fact]
        public void Parse_DeleteUser_ReturnsDeleteUserObject()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("DELETE USER ahmet;");

            Assert.NotNull(result);
            DeleteUser deleteUser = Assert.IsType<DeleteUser>(result);

            Assert.Equal("ahmet", deleteUser.Username);
        }

        [Fact]
        public void Parse_DeleteWithoutWhere_ReturnsNull()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("DELETE FROM Students;");

            Assert.Null(result);
        }

        [Fact]
        public void Parse_UpdateWrongSyntax_ReturnsNull()
        {
            MiniSqlQuery result = MiniSQLParser.Parse("UPDATE Students SET WHERE Id = 1;");

            Assert.Null(result);
        }

        
        //[Fact]
        //public void Parse_Select_IncorrectSelectWithTextAfter()
        //{
        //    MiniSqlQuery result1 = MiniSQLParser.Parse("SELECT * FROM Students; extra text");
        //    Assert.Null(result1);

        //    MiniSqlQuery result2 = MiniSQLParser.Parse("SELECT * FROM Students WHERE nonsense");
        //    Assert.Null(result2);

        //    MiniSqlQuery result3 = MiniSQLParser.Parse("SELECT Name FROM Users xyz");
        //    Assert.Null(result3);
        //}

       
    }
}
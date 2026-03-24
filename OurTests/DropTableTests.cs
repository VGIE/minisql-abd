using System;
using System.Collections.Generic;
using DbManager;
using Xunit;

namespace OurTests
{
    public class DropTableTests
    {
        [Fact]
        public void Constructor_SetsTableName()
        {
            DropTable query = new DropTable("Students");
            Assert.Equal("Students", query.Table);
        }

        [Fact]
        public void Execute_ExistingTable_ReturnsSuccessMessage()
        {
            Database db = new Database("admin", "admin");
            db.CreateTable("ToDrop", new List<ColumnDefinition> { new ColumnDefinition(ColumnDefinition.DataType.Int, "ID") });

            DropTable query = new DropTable("ToDrop");
            string result = query.Execute(db);

            Assert.Equal(Constants.DropTableSuccess, result);
            Assert.Null(db.TableByName("ToDrop"));
        }

        [Fact]
        public void Execute_NonExistingTable_ReturnsErrorMessage()
        {
            Database db = new Database("admin", "admin");
            DropTable query = new DropTable("NonExistent");

            string result = query.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void Execute_NullDatabase_ThrowsException()
        {
            DropTable query = new DropTable("AnyTable");
            Assert.Throws<NullReferenceException>(() => query.Execute(null));
        }
    }
}
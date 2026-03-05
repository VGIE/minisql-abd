using System.Collections.Generic;
using Xunit;
using DbManager;

namespace OurTests
{
    public class TableTests
    {
        [Fact]
        public void Insert_ShouldAddRow()
        {
            Table table = Table.CreateTestTable();

            List<string> row = new List<string> { "1", "John" };

            bool result = table.Insert(row);

            Assert.True(result);
        }

        [Fact]
        public void Insert_WrongColumnCount_ShouldFail()
        {
            Table table = Table.CreateTestTable();

            List<string> row = new List<string> { "1" };

            bool result = table.Insert(row);

            Assert.False(result);
        }

        [Fact]
        public void MultipleInsert_ShouldStoreAllRows()
        {
            Table table = Table.CreateTestTable();

            table.Insert(new List<string> { "1", "John" });
            table.Insert(new List<string> { "2", "Jane" });

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "1", "John" },
                new List<string> { "2", "Jane" }
            });
        }

        [Fact]
        public void EmptyInsert_ShouldFail()
        {
            Table table = Table.CreateTestTable();

            bool result = table.Insert(new List<string>());

            Assert.False(result);
        }
    }
}

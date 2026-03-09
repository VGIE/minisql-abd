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


        [Fact]
        public void ColumnByName_ReturnsColumnWithGivenName()
        {
            Table table = Table.CreateTestTable();

            ColumnDefinition result = table.ColumnByName("Name");

            Assert.NotNull(result);
            Assert.Equal("Name", result.Name);
        }

        [Fact]
        public void ColumnIndexByName_ShouldReturnFirstColumn()
        {
            Table table = Table.CreateTestTable();

            int index = table.ColumnIndexByName("Id");

            Assert.Equal(0, index);
        }

        [Fact]
        public void TestToString()
        {
            Table table = Table.CreateTestTable();

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Id"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            List<string> valores = new List<string> { "001", "Rodolfo" };
            Row row = new Row(columns, valores);
            table.AddRow(row);

            string esperado = "['Id','Name']{'001','Rodolfo'}";
            Assert.Equal(esperado, table.ToString());
        }

        [Fact]
        public void TestToString_EmptyTable()
        {
            Table table = Table.CreateTestTable();

            string resultado = table.ToString();

            Assert.Equal("['Id','Name']", resultado);
        }

        [Fact]
        public void DeleteIthRow_ShouldReturnSecondCreatedRow()
        {
            Table table = Table.CreateTestTable();

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Id"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            table.AddRow(new Row(columns, new List<string> { "1", "A" }));
            table.AddRow(new Row(columns, new List<string> { "2", "B" }));

            table.DeleteIthRow(0);

            string esperado = "['Id','Name']{'2','B'}";
            Assert.Equal(esperado, table.ToString());
        }

        [Fact]
        public void RowIsConditionTrueStringTest()
        {

        }

        [Fact]
        public void TableSelectWithoutConditionTest()
        {
        }

        [Fact]
        public void TableDeleteRowsWhereConditionIsTrueIntTest()
        {

        }

        [Fact]
        public void TableDeleteRowsWhereConditionIsTrueStringTest()
        {

        }

        [Fact]
        public void RowIsConditionTrueDoubleTest()
        {

        }

        [Fact]
        public void RowIsConditionTrueIntTest()
        {

        }

        [Fact]
        public void TableDeleteRowsWhereConditionIsTrueDoubleTest()
        {

        }

        [Fact]
        public void TableSelectWithoutConditionAndDisorderedColumnsTest()
        {

        }
    }
}

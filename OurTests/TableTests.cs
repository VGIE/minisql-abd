using DbManager;
using DbManager.Parser;
using System.Collections.Generic;
using Xunit;

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
        public void ToStringTest()
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
        public void ToStringTest_EmptyTable()
        {
            Table table = Table.CreateTestTable();

            string resultado = table.ToString();

            Assert.Equal("['Id','Name']", resultado);
        }

        [Fact]
        public void DeleteIthRow_ShouldDeleteFirstCreatedRow()
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
        public void DeleteIthRow_DeleteOutOfRangeRow()
        {
            Table table = Table.CreateTestTable();

            table.Insert(new List<string> { "1", "Test" });

            table.DeleteIthRow(5);
            table.DeleteIthRow(-1);

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "1", "Test" }
            });
        }

        [Fact]
        public void TableSelectWithoutConditionTest()
        {
            Table table = Table.CreateTestTable();

            table.Insert(new List<string> { "1", "David" });
            table.Insert(new List<string> { "2", "Anne" });

            Table result = table.Select(new List<string> { "Id", "Name" }, null);

            result.CheckForTesting(new List<List<string>>
            {
                new List<string> { "1", "David" },
                new List<string> { "2", "Anne" }
            });
        }

        [Fact]
        public void TableDeleteRowsWhereConditionIsTrueIntTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
    {
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
    };

            Table table = new Table("Test", columns);

            table.Insert(new List<string> { "10" });
            table.Insert(new List<string> { "20" });
            table.Insert(new List<string> { "30" });

            Condition condition = new Condition("Age", ">", "15");

            table.DeleteWhere(condition);

            table.CheckForTesting(new List<List<string>>
    {
        new List<string> { "10" }
    });
        }

        [Fact]
        public void DeleteIthRow_DeleteLastRow()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Id"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            Table table = new Table("Test", columns);

            table.AddRow(new Row(columns, new List<string> { "1", "Primera" }));
            table.AddRow(new Row(columns, new List<string> { "2", "Ultima" }));

            table.DeleteIthRow(1);

            string esperado = "['Id','Name']{'1','Primera'}";
            Assert.Equal(esperado, table.ToString());
        }

        [Fact]
        public void RowIsConditionTrueDoubleTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
        {
            new ColumnDefinition(ColumnDefinition.DataType.Double, "Height")
        };

            Row row = new Row(columns, new List<string> { "1.70" });

            Condition condition = new Condition("Height", ">", "1.60");

            Assert.False(row.IsTrue(condition));
        }

        [Fact]
        public void RowIsConditionTrueStringTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            Row row = new Row(columns, new List<string> { "David" });

            Condition condition = new Condition("Name", "=", "David");

            Assert.True(row.IsTrue(condition));
        }

        [Fact]
        public void TableDeleteRowsWhereConditionIsTrueStringTest()
        {
            Table table = Table.CreateTestTable();

            table.Insert(new List<string> { "1", "David" });
            table.Insert(new List<string> { "2", "Anne" });
            table.Insert(new List<string> { "3", "Danna" });

            Condition condition = new Condition("Name", "=", "David");

            table.DeleteWhere(condition);

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "2", "Anne" }
            });
        }

        [Fact]
        public void RowIsConditionTrueIntTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Row row = new Row(columns, new List<string> { "30" });

            Condition condition = new Condition("Age", ">", "20");

            Assert.True(row.IsTrue(condition));
        }

        [Fact]
        public void TableSelectWithoutConditionAndDisorderedColumnsTest()
        {
            Table table = Table.CreateTestTable();

            table.Insert(new List<string> { "1", "David" });

            Table result = table.Select(new List<string> { "Name", "Id" }, null);

            result.CheckForTesting(new List<List<string>>
            {
                new List<string> { "David", "1" }
            });
        }

        [Fact]
        public void TableDeleteRowsWhereConditionIsTrueDoubleTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height")
            };

            Table table = new Table("Test", columns);

            table.Insert(new List<string> { "1.60" });
            table.Insert(new List<string> { "1.80" });

            Condition condition = new Condition("Height", ">", "1.70");

            table.DeleteWhere(condition);

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "1.60" },
                new List<string> { "1.80" }
            });
        }

        





    }
}

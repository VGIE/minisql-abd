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

            List<string> row = new List<string> { "Annita", "1.85","19" };

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
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };
            Table table = new Table("TestTable", columns);


            table.Insert(new List<string> { "John", "1.78","17" });
            table.Insert(new List<string> { "Jane", "1.55","27" });

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "John", "1.78","17" },
                new List<string> { "Jane", "1.55", "27" }
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
            Assert.Null(table.ColumnByName(null));
        }

        [Fact]
        public void ColumnIndexByName_ShouldReturnFirstColumn()
        {
            Table table = Table.CreateTestTable();

            int indexName = table.ColumnIndexByName("Name");
            int indexHeight = table.ColumnIndexByName("Height");
            int indexAge = table.ColumnIndexByName("Age");
            int indexFake = table.ColumnIndexByName("Id");


            Assert.Equal(0, indexName);
            Assert.Equal(1, indexHeight);
            Assert.Equal(2, indexAge);
            Assert.Equal(-1, indexFake);
        }

        [Fact]
        public void ToStringTest()
        {
            Table table = Table.CreateTestTable();

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            table.AddRow(new Row(columns, new List<string> { "David", "1.80", "30" }));

            string esperado = "['Name','Height','Age']" +
                              "{'Rodolfo','1.62','25'}" +
                              "{'Maider','1.67','67'}" +
                              "{'Pepe','1.55','51'}" +
                              "{'David','1.80','30'}";
            Assert.Equal(esperado, table.ToString());
        }

        [Fact]
        public void ToStringTest2()
        {
            Table table = Table.CreateTestTable();

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            table.AddRow(new Row(columns, new List<string> { "Anne", "1.60", "21" }));

            string esperado = "['Name','Height','Age']" +
                              "{'Rodolfo','1.62','25'}" +
                              "{'Maider','1.67','67'}" +
                              "{'Pepe','1.55','51'}" +
                              "{'Anne','1.60','21'}";
            Assert.Equal(esperado, table.ToString());
        }

        [Fact]
        public void ToStringTest_EmptyTable()
        {
            Table table = Table.CreateTestTable();

            table.DeleteIthRow(0);
            table.DeleteIthRow(0);
            table.DeleteIthRow(0);

            string resultado = table.ToString();

            string esperado = "['Name','Height','Age']";

            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Delete1stRow()
        {
            Table table = Table.CreateTestTable();

            table.DeleteIthRow(0);

            string esperado = "['Name','Height','Age']{'Maider','1.67','67'}{'Pepe','1.55','51'}";
            Assert.Equal(esperado, table.ToString());
        }

        [Fact]
        public void DeleteLastRow()
        {
            Table table = Table.CreateTestTable();
            table.Insert(new List<string> { "First", "1.70", "20" });
            table.Insert(new List<string> { "Last", "1.80", "30" });

            table.DeleteIthRow(table.NumRows() - 1);

            Assert.Equal(4, table.NumRows());
            Assert.Equal("First", table.GetRow(3).Values[0]);
        }

        [Fact]
        public void DeleteIthRow_DeleteOutOfRangeRow()
        {
            Table table = Table.CreateTestTable();
            List<string> values = new List<string> { "4", "Test", "99" };
            table.Insert(values);

            table.DeleteIthRow(10);
            table.DeleteIthRow(-1);

            Assert.Equal(4, table.NumRows());

            Row resultRow = table.GetRow(3);
            Assert.Equal("4", resultRow.Values[0]);
            Assert.Equal("Test", resultRow.Values[1]);
        }

        [Fact]
        public void TableSelectWithoutConditionTest()
        {
            Table table = Table.CreateTestTable();

            Table result = table.Select(new List<string> { "Name", "Age" }, null);

            result.CheckForTesting(new List<List<string>>
            {
                new List<string> { "Rodolfo", "25" },
                new List<string> { "Maider", "67" },
                new List<string> { "Pepe", "51" }
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

            table.Insert(new List<string> { "David", "1.80", "30" });
            table.Insert(new List<string> { "Anne", "1.60", "25" });

            Condition condition = new Condition("Name", "=", "David");

            table.DeleteWhere(condition);

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "Rodolfo", "1.62", "25" },
                new List<string> { "Maider", "1.67", "67" },
                new List<string> { "Pepe", "1.55", "51" },
                new List<string> { "Anne", "1.60", "25" }
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

            Table result = table.Select(new List<string> { "Age", "Name" }, null);

            result.CheckForTesting(new List<List<string>>
            {
                new List<string> { "25", "Rodolfo" },
                new List<string> { "67", "Maider" },
                new List<string> { "51", "Pepe" }
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

        [Fact]
        public void TableUpdateRowsWhereConditionIsTrueTest()
        {
            Table table = Table.CreateTestTable();

            table.Insert(new List<string> { "1", "David" });
            table.Insert(new List<string> { "2", "Anne" });
            table.Insert(new List<string> { "3", "Danna" });

            List<SetValue> updates = new List<SetValue>()
            {
                new SetValue("Name", "Besma")
            };

            Condition condition = new Condition("Name", "=", "David");

            table.Update(updates, condition);

            table.CheckForTesting(new List<List<string>>
            {
                new List<string> { "1", "Besma" },
                new List<string> { "2", "Anne" },
                new List<string> { "3", "Danna" }
            });
        }
    }
}

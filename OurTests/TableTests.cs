using DbManager;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        
        private Table CreateTestTable()
        {
            string name = "TestTable";
            List<ColumnDefinition> columns = new List<ColumnDefinition>();

            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"));

            Table table = new Table(name, columns);

            return table;
        }

        [Fact]

        public void TestColumnByName()
        {
            Table table = CreateTestTable();

            ColumnDefinition result = table.ColumnByName("Nombre");

            Assert.NotNull(result);
            Assert.Equal("Nombre", result.Name);
        }

        [Fact]
        public void TestColumnIndexByName()
        {
            //Debe devolver indice 0

            Table table = CreateTestTable();

            int index = table.ColumnIndexByName("Nombre");

            Assert.Equal(0, index);
        }
        
        [Fact]

        public void TestToString()
        {
            //El formato debe ser correcto

            Table table = CreateTestTable();
            
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"));

            List<String> valores = new List<string> { "Rodolfo", "25" };

            Row row = new Row(columns ,valores);

            table.AddRow(row);

            string expectedResult = "['Nombre','Edad']{'Rodolfo','25'}";
            Assert.Equal(expectedResult, table.ToString());
        }

        [Fact]

        public void TestDeleteIthRow()
        {
            Table table = CreateTestTable();

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"));

            Row row1 = new Row(columns, new List<String> { "Rodolfo", "25" });
            Row row2 = new Row(columns, new List<String> { "Jacinto", "27" });

            table.AddRow(row1);
            table.AddRow(row2);

            table.DeleteIthRow(0);


            string expectedResult = "['Nombre','Edad']{'Jacinto','27'}";
            Assert.Equal(expectedResult, table.ToString());
        }
    }
}
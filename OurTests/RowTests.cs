using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        private Row CreateTestRow()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };
            List<String> rowValues = new List<string>()
            {
                "Jacinto", "37"
            };
            Row testRow = new Row(columns, rowValues);
            return testRow;
        }

        [Fact]
        public void Test1()
        {
            Row testRow = CreateTestRow();
            Assert.Equal("Jacinto", testRow.GetValue("Name"));
            Assert.Equal("37", testRow.GetValue("Age"));

            testRow.SetValue("Name", "Maider");

            Assert.Equal("Maider", testRow.GetValue("Name"));

            Assert.Null(testRow.GetValue("Nombre"));
        }
        [Fact]
        public void SetValueTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Subjects"),
                new ColumnDefinition(ColumnDefinition.DataType.Double,"Grade")
            };
            List<string> values = new List<string>()
            { "Administration","4,6"
            };
            Row row = new Row(columns, values);
            row.SetValue("Grade", "5,4");
            Assert.Equal("5,4", row.GetValue("Grade"));
            Assert.Equal("Administration", row.GetValue("Subjects"));
        }
        [Fact]
        public void GetValueNullTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Brand")
            };
            List<string> values = new List<string>()
            {
                "Primark"
            };
            Row row = new Row(columns, values);
            Assert.Null(row.GetValue("Prices"));
        }

        [Fact]
        public void IsTrueTest1()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Stock")
            };
            List<string> values = new List<string>() { "12" };
            Row row = new Row(columns, values);
            Condition conditionLess = new Condition("Stock", "<", "100");
            Condition conditionGreater = new Condition("Stock", ">", "100");
            Condition conditionEqual = new Condition("Stock", "=", "12");
            Assert.True(row.IsTrue(conditionLess));
            Assert.False(row.IsTrue(conditionGreater));
            Assert.True(row.IsTrue(conditionEqual));
        }
        [Fact]
        public void IsTrueTest2()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Status")
            };
            List<string> values = new List<string>() { "Active" };
            Row row = new Row(columns, values);
            Condition conditionEqual = new Condition("Status", "=", "Active");
            Condition conditionNotEqual = new Condition("Status", "!=", "Active");
            Assert.True(row.IsTrue(conditionEqual));
            Assert.False(row.IsTrue(conditionNotEqual));
        }
        [Fact]
        public void AsTextTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Name"),
                new ColumnDefinition(ColumnDefinition.DataType.String,"City")
            };
            List<string> values = new List<string>() { "Danna", "Vitoria-Gasteiz" };
            Row row = new Row(columns, values);
            string text = row.AsText();
            Assert.Equal("Danna:Vitoria-Gasteiz", text);
        }

        [Fact]
        public void AsTextEmpty()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            List<string> values = new List<string>();
            Row row = new Row(columns, values);
            string text = row.AsText();
            Assert.Equal("", text);

        }
        [Fact]
        public void AsTextNull()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            Row row = new Row(columns, null);
            string text = row.AsText();
            Assert.Equal("", text);

        }
        [Fact]
        public void AsTextandParse()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Name"),
                new ColumnDefinition(ColumnDefinition.DataType.String,"Time")

            };
            List<string> values = new List<string>() { "Danna", "14:54" };
            Row originalRow = new Row(columns, values);
            string text = originalRow.AsText();
            Row parsedRow = Row.Parse(columns, text);
            Assert.Equal("Danna:14[SEPARATOR]54", text);
            Assert.Equal("14:54", parsedRow.GetValue("Time"));

        }
        [Fact]
        public void ParseTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Name"),
                new ColumnDefinition(ColumnDefinition.DataType.String,"City")

            };
            string text = ("Danna:Vitoria-Gasteiz");
            Row parsedRoW = Row.Parse(columns, text);
            Assert.NotNull(parsedRoW);
            Assert.Equal(2, parsedRoW.Values.Count);
            Assert.Equal("Danna", parsedRoW.GetValue("Name"));
            Assert.Equal("Vitoria-Gasteiz", parsedRoW.GetValue("City"));
        }
        [Fact]
        public void ParseNullTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Name")
            };
            Row parsedRow = Row.Parse(columns, null);
            Assert.NotNull(parsedRow);
            Assert.Single(parsedRow.Values);
            Assert.Equal("", parsedRow.GetValue("Name"));
        }
    }
}
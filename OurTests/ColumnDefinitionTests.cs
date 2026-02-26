using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table

        private ColumnDefinition CreateStringColumn()
        {
            return new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
        }

        private ColumnDefinition CreateIntColumn()
        {
            return new ColumnDefinition(ColumnDefinition.DataType.Int, "Age");
        }

        private ColumnDefinition CreateEncodedColumn()
        {
            return new ColumnDefinition(ColumnDefinition.DataType.String, "A->B");
        }

        [Fact]
        public void ConstructorTest()
        {
            ColumnDefinition column = CreateStringColumn();

            Assert.Equal(ColumnDefinition.DataType.String, column.Type);
            Assert.Equal("Name", column.Name);
        }

        [Fact]
        public void AsTextTest1()
        {
            ColumnDefinition column = CreateIntColumn();

            string result = column.AsText();

            Assert.Equal("Int->Age", result);
        }

        [Fact]
        public void AsTextTest2()
        {
            ColumnDefinition column = CreateEncodedColumn();

            string result = column.AsText();

            Assert.Contains("[ARROW]", result);
        }

        [Fact]
        public void ParseTest1()
        {
            ColumnDefinition column = ColumnDefinition.Parse("Double->Salary");

            Assert.Equal(ColumnDefinition.DataType.Double, column.Type);
            Assert.Equal("Salary", column.Name);
        }

        [Fact]
        public void ParseTest2()
        {
            ColumnDefinition column = ColumnDefinition.Parse("String->A[ARROW]B");

            Assert.Equal(ColumnDefinition.DataType.String, column.Type);
            Assert.Equal("A->B", column.Name);
        }

        [Fact]
        public void ParseTest3()
        {
            ColumnDefinition original = CreateStringColumn();

            string text = original.AsText();
            ColumnDefinition parsed = ColumnDefinition.Parse(text);

            Assert.Equal(original.Type, parsed.Type);
            Assert.Equal(original.Name, parsed.Name);
        }
    }
}
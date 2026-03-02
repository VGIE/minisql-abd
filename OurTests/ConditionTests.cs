using DbManager;

namespace OurTests
{

    namespace OurTests
{
    public class ConditionTests
    {
        [Fact]
        public void Constructor()
        {
            var c = new Condition("age", "=", "10");
            Assert.Equal("age", c.ColumnName);
            Assert.Equal("=", c.Operator);
            Assert.Equal("10", c.LiteralValue);
        }

        [Fact]
        public void Int_Menor_ReturnsTrue_WhenValueIsSmaller()
        {
            var c = new Condition("age", "<", "10");
            Assert.True(c.IsTrue("5", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void Int_Menor_ReturnsFalse_WhenValueIsGreater()
        {
            var c = new Condition("age", "<", "10");
            Assert.False(c.IsTrue("15", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void Int_Igual_ReturnsTrue_WhenValuesAreEqual()
        {
            var c = new Condition("age", "=", "10");
            Assert.True(c.IsTrue("10", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void Int_Desigual_ReturnsFalse_WhenValuesAreEqual()
        {
            var c = new Condition("age", "!=", "10");
            Assert.False(c.IsTrue("10", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void Int_MenorOIgual_ReturnsTrue_WhenValueIsEqual()
        {
            var c = new Condition("age", "<=", "10");
            Assert.True(c.IsTrue("10", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void Int_MayorOIgual_ReturnsTrue_WhenValueIsGreater()
        {
            var c = new Condition("age", ">=", "10");
            Assert.True(c.IsTrue("15", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void Int_UsesNumericComparison()
        {
            var c = new Condition("num", "<", "10");
            Assert.True(c.IsTrue("9", ColumnDefinition.DataType.Int));
        }

        [Fact]
        public void String_Menor_ReturnsTrue_WhenValueComesFirst()
        {
            var c = new Condition("name", "<", "cd");
            Assert.True(c.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void String_Igual_ReturnsFalse_Distintos()
        {
            var c = new Condition("name", "=", "hello");
            Assert.False(c.IsTrue("world", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void String_NoIgual_ReturnsTrue_Distintos()
        {
            var c = new Condition("name", "!=", "hello");
            Assert.True(c.IsTrue("world", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void String_LComparacion()
        {
        
            var c = new Condition("num", ">", "10");
            Assert.True(c.IsTrue("9", ColumnDefinition.DataType.String));
        }
    }
    }
}
using static Mackerel.Macro;

namespace Mackerel.Tests;

public static partial class MackerelTests
{
    public class TextGenerationTests
    {
        [Fact]
        public void Empty_Macro_Is_Empty()
        {
            var actual = Empty(in Document.Empty).Length;

            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        [InlineData('"', "\"")]
        [InlineData(3.14, "3.14")]

        public void Str_Macro_Is_Text(object value, string expected)
        {
            var actual = Str(value)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Macro_Is_Text()
        {
            var expected = "TEST";
            var actual = Text(expected)(in Document.Empty);

            Assert.Equal(expected, actual);
        }
    }
}
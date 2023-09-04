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

        [Fact]
        public void LongText_Macro_Is_Text()
        {
            var expected = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            var actual = LongText(
                """
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut 

                   labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco 

                  laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in  
                     
                        voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat 
                                                                                                            
                       cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                """)(in Document.Empty);

            Assert.Equal(expected, actual);
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
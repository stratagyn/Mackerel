using System.Reflection;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using static Mackerel.Macro;

namespace Mackerel.Tests;

public static partial class MackerelTests
{
    public class StateManagementTests
    {
        [Fact]
        public void Bind_Macro_Is_Bound_Text()
        {
            var expected = "HELLO WORLD";
            var actual = Bind(HELLO,
                hello => Bind(WORLD,
                world => Text($"{hello} {world}"))
            )(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Block_Macro_Empty_Macros_Is_Empty_Text()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block()(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Block_Macro_Returns_Last_Macro_Text()
        {
            var doc = new Document(3);
            var expected = "WORLD";
            var actual = Block(
                Buffer(0),
                Write(TEST),
                Buffer(1),
                Write(HELLO),
                Buffer(2),
                Write(WORLD),
                Read(0),
                Read(1),
                Read(2))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Buffer_Macro_Changes_Output_Buffer()
        {
            var doc = new Document();
            var expected = "CHANNEL1";
            var actual = Block(
                Write(Text("CHANNEL0")),
                Buffer(1),
                Write(Text("CHANNEL1")),
                Read(1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Clear_Macro_Resets_Document()
        {
            var doc = new Document();
            var firstExpected = "GOODBYE WORLD";
            var secondExpected = "HELLO WORLD";
            var instr = Block(
                Buffer(1),
                Write(Text("GOODBYE WORLD")),
                Read(1));

            var actual = instr(in doc);

            Assert.Equal(firstExpected, actual);
            
            actual = Block(
                Clear,
                Write(Text("HELLO WORLD")),
                Read())(in doc);

            Assert.Equal(secondExpected, actual);
        }

        [Fact]
        public void Dedent_Macro_Decreases_Indent_Of_Context()
        {
            var doc = new Document();
            var expected = "    TEST";
            var actual = Block(
                Indent(8),
                Dedent(4),
                Write(TEST),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void If_Macro_False_Is_False_Text()
        {
            var expected = "WORLD";
            var actual = If(
                str => str == "BEST",
                TEST,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void If_Macro_True_Is_True_Text()
        {
            var expected = "HELLO";
            var actual = If(
                str => str == "TEST",
                TEST,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void If_Typed_Macro_False_Is_False_Text()
        {
            var expected = "WORLD";
            var actual = If(
                n => n > 0,
                0,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void If_Typed_Macro_True_Is_True_Text()
        {
            var expected = "HELLO";
            var actual = If(
                n => n > 0,
                1,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfN_Macro_False_Is_True_Text()
        {
            var expected = "HELLO";
            var actual = IfN(
                str => str == "BEST",
                TEST,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfN_Macro_True_Is_False_Text()
        {
            var expected = "WORLD";
            var actual = IfN(
                str => str == "TEST",
                TEST,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfN_Typed_Macro_False_Is_False_Text()
        {
            var expected = "HELLO";
            var actual = IfN(
                n => n > 0,
                0,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfN_Typed_Macro_True_Is_True_Text()
        {
            var expected = "WORLD";
            var actual = IfN(
                n => n > 0,
                1,
                HELLO,
                WORLD)(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfDef_Macro_Defined_Key_Is_True_Text()
        {
            var doc = new Document();
            var expected = "HELLO";
            var actual = Block(
                Set("i", 0),
                IfDef("i", HELLO, WORLD))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfDef_Macro_Undefined_Key_Is_False_Text()
        {
            var doc = new Document();
            var expected = "WORLD";
            var actual = IfDef("i", HELLO, WORLD)(doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfNDef_Macro_Defined_Key_Is_False_Text()
        {
            var doc = new Document();
            var expected = "WORLD";
            var actual = Block(
                Set("i", 0),
                IfNDef("i", HELLO, WORLD))(doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IfNDef_Macro_Undefined_Key_Is_True_Text()
        {
            var doc = new Document();
            var expected = "HELLO";
            var actual = IfNDef("i", HELLO, WORLD)(in doc);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        [InlineData('"', "\"")]
        [InlineData(3.14, "3.14")]

        public void Get_Macro_Is_Present_Key_Is_Object_Text(object value, string expected)
        {
            var doc = new Document();
            var actual = Block(
                Set("value", value),
                Get("value")
            )(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Macro_Missing_Key_Is_Empty()
        {
            var doc = new Document();
            var expected = "";
            var actual = Get("i")(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Typed_Macro_Present_Key_Is_Value()
        {
            var doc = new Document(new Dictionary<string, object>() { ["i"] = 10 });
            var expected = "10";
            var actual = Get<int>("i", i => Text($"{i}"))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Typed_Macro_Missing_Key_Is_Empty()
        {
            var doc = new Document();
            var expected = "";
            var actual = Get<int>("i", i => Text($"{i}"))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Typed_Macro_Wrong_Type_Is_Empty()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block(
                Set("i", 10),
                Get<int[]>("i", i => Text(string.Join("", i)))
            )(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Indent_Macro_Increases_Indent_Of_Context()
        {
            var doc = new Document();
            var expected = "    TEST";
            var actual = Block(
                Indent(4),
                Write(TEST),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Map_Macro_Is_Mapped_Text()
        {
            var expected = "HELLO!";
            var actual = Map(HELLO,
                hello => $"{hello}!")(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mutate_Macro_Is_Bound_Text()
        {
            var expected = "HELLO WORLD 1!";
            var actual = Mutate(1, n => Text($"HELLO WORLD {n}!"))(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mutate_Map_Macro_Is_Bound_Text()
        {
            var expected = "HELLO WORLD 11!";
            var actual = Mutate(
                Text("HELLO WORLD"),
                str => str.Length,
                n => Text($"HELLO WORLD {n}!"))(in Document.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_Macro_Is_Written_Text()
        {
            var doc = new Document();
            var expected = "TEST";
            var actual = Block(
                Write(TEST),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_Macro_Multiple_Buffers_Is_Concatenated_Text()
        {
            var doc = new Document();
            var expected = "HELLOWORLD";
            var actual = Block(
                Write(HELLO),
                Buffer(1),
                Write(WORLD),
                Read(0, 1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_Macro_Negative_Buffer_Is_Empty_Text()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block(
                Write(TEST),
                Read(-1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_Macro_Unavailable_Buffer_Is_Empty_Text()
        {
            var doc = new Document(1);
            var expected = "";
            var actual = Block(
                Write(TEST),
                Read(1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadLine_Macro_Is_Written_Text()
        {
            var doc = new Document();
            var expected = $"TEST{Environment.NewLine}";
            var actual = Block(
                Write(TEST),
                ReadLine())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadLine_Macro_Multiple_Buffers_Is_Concatenated_Text()
        {
            var doc = new Document();
            var expected = $"HELLO{Environment.NewLine}WORLD{Environment.NewLine}";
            var actual = Block(
                Write(HELLO),
                Buffer(1),
                Write(WORLD),
                ReadLine(0, 1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadLine_Macro_Negative_Buffer_Is_Empty_Text()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block(
                Write(TEST),
                ReadLine(-1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadLine_Macro_Unavailable_Buffer_Is_Empty_Text()
        {
            var doc = new Document(1);
            var expected = "";
            var actual = Block(
                Write(TEST),
                ReadLine(1))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Typed_Macro_Missing_Key_Is_Value()
        {
            var doc = new Document();
            var expected = "10";
            var actual = Block(
                Set("i", 10),
                Get<int>("i", i => Text($"{i}"))
            )(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Set_Macro_Present_Key_Does_Nothing()
        {
            var doc = new Document(new Dictionary<string, object>() { ["i"] = 10 });
            var expected = "10";
            var actual = Block(
                    Set("i", 9),
                    Get<int>("i", i => Text($"{i}")))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Unset_Macro_Present_Key_Removes_Value()
        {
            var doc = new Document(new Dictionary<string, object>() { ["i"] = 10 });
            var expected = "";
            var actual = Unset("i")(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Unset_Macro_Missing_Key_Does_Nothing()
        {
            var doc = new Document();
            var expected = "";
            var actual = Unset("i")(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_Macro_Present_Key_Is_Value()
        {
            var doc = new Document(new Dictionary<string, object>() { ["i"] = 10 });
            var expected = "9";
            var actual = Block(
                Update("i", 9),
                Get<int>("i", i => Text($"{i}"))
            )(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_Macro_Missing_Key_Throws_ArgumentException()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block(
                    Update("i", 10),
                    Get<int>("i", i => Text($"{i}")))(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Write_String_Macro_Is_Written_Text()
        {
            var doc = new Document(1);
            var expected = "HELLOWORLD";
            var actual = Block(
                Write("HELLO"),
                Write("WORLD"),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Write_Macro_Macro_Is_Written_Text()
        {
            var doc = new Document(1);
            var expected = "HELLOWORLD";
            var actual = Block(
                Write(HELLO),
                Write(WORLD),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Write_String_Macro_Negative_Buffer_Is_Empty_Text()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block(
                Buffer(-1),
                Write(HELLO),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Write_String_Macro_Too_Large_Buffer_Is_Empty_Text()
        {
            var doc = new Document(1);
            var expected = "";
            var actual = Block(
                Buffer(1),
                Write(HELLO),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteLine_Macro_Empty_Is_NewLine()
        {
            var doc = new Document(1);
            var expected = Environment.NewLine;
            var actual = Block(
                WriteLine(),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteLine_String_Macro_Is_Written_Text()
        {
            var doc = new Document(1);
            var expected = $"HELLO{Environment.NewLine}WORLD";
            var actual = Block(
                WriteLine(Text("HELLO")),
                Write(Text("WORLD")),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteLine_Macro_Macro_Is_Written_Text()
        {
            var doc = new Document(1);
            var expected = $"HELLO{Environment.NewLine}WORLD"; ;
            var actual = Block(
                WriteLine(HELLO),
                Write(WORLD),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteLine_String_Macro_Negative_Buffer_Is_Empty_Text()
        {
            var doc = new Document();
            var expected = "";
            var actual = Block(
                Buffer(-1),
                WriteLine(HELLO),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteLine_String_Macro_Too_Large_Buffer_Is_Empty_Text()
        {
            var doc = new Document(1);
            var expected = "";
            var actual = Block(
                Buffer(1),
                WriteLine(HELLO),
                Read())(in doc);

            Assert.Equal(expected, actual);
        }
    }
}

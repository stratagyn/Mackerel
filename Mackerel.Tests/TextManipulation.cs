using static Mackerel.Macro;

namespace Mackerel.Tests;

public static partial class MackerelTests
{
   public class TextManipulationTests
   {
      [Fact]
      public void Concat_Macro_Empty_Macros_Is_Empty()
      {
         var expected = "";
         var actual = Concat()(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Concat_Macro_Single_Macro_Is_Macro_Text()
      {
         var expected = "TEST";
         var actual = Concat(TEST)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Concat_Macro_Single_Macro_Is_Concatenated_Text()
      {
         var expected = "HELLOWORLD";
         var actual = Concat(HELLO, WORLD)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_No_Count_Macro_Is_Shortened_Text()
      {
         var expected = "TE";
         var actual = Delete(TEST, 2)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_No_Count_Negative_Index_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = Delete(TEST, -2)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_No_Count_Too_Large_Index_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = Delete(TEST, 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_Count_Macro_Is_Shortened_Text()
      {
         var expected = "TET";
         var actual = Delete(TEST, 2, 1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_Count_Negative_Index_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = Delete(TEST, -2, 1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_Count_Too_Large_Index_Count_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = Delete(TEST, 4, 1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Delete_Negative_Count_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = Delete(TEST, 2, -1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Join_String_Macro_Empty_Macros_Is_Empty()
      {
         var expected = "";
         var actual = Join(",")(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Join_String_Macro_Single_Macro_Is_Macro_Text()
      {
         var expected = "TEST";
         var actual = Join(",", TEST)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Join_String_Macro_Multiple_Macros_Is_Joined_Text()
      {
         var expected = "TEST1, TEST2, TEST3";
         var actual = Join(", ", Text("TEST1"), Text("TEST2"), Text("TEST3"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Join_Macro_Macro_Empty_Macros_Is_Empty()
      {
         var expected = "";
         var actual = Join(Text(","))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Join_Macro_Macro_Single_Macro_Is_Macro_Text()
      {
         var expected = "TEST";
         var actual = Join(Text(","), TEST)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Join_Macro_Macro_Multiple_Macros_Is_Joined_Text()
      {
         var expected = "HELLO WORLD";
         var actual = Join(Text(" "), HELLO, WORLD)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Lowercase_Macro_Is_Lowercase_Text()
      {
         var expected = "test";
         var actual = Lowercase(TEST)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadEnd_Empty_Macro_Is_Padded_Text()
      {
         var expected = "TEST    ";
         var actual = PadEnd(TEST, 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadEnd_String_Macro_Is_Padded_Text()
      {
         var expected = "TEST....";
         var actual = PadEnd(TEST, ".", 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadEnd_String_Negative_Count_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = PadEnd(TEST, ".", -1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadEnd_Macro_Macro_Is_Padded_Text()
      {
         var expected = "TEST....";
         var actual = PadEnd(TEST, Text("."), 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadEnd_Macro_Negative_Count_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = PadEnd(TEST, Text("."), -1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadStart_Empty_Macro_Is_Padded_Text()
      {
         var expected = "    TEST";
         var actual = PadStart(TEST, 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadStart_String_Macro_Is_Padded_Text()
      {
         var expected = "....TEST";
         var actual = PadStart(TEST, ".", 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadStart_String_Negative_Count_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = PadStart(TEST, ".", -1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadStart_Macro_Macro_Is_Padded_Text()
      {
         var expected = "....TEST";
         var actual = PadStart(TEST, Text("."), 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void PadStart_Macro_Negative_Count_Macro_Is_Text()
      {
         var expected = "TEST";
         var actual = PadStart(TEST, Text("."), -1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_No_Count_Macro_Is_Substring_Text()
      {
         var expected = "ST";
         var actual = Substring(TEST, 2)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_No_Count_Negative_Index_Macro_Is_Empty_Text()
      {
         var expected = "";
         var actual = Substring(TEST, -2)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_No_Count_Too_Large_Index_Macro_Is_Empty_Text()
      {
         var expected = "";
         var actual = Substring(TEST, 4)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_Count_Macro_Is_Shortened_Text()
      {
         var expected = "S";
         var actual = Substring(TEST, 2, 1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_Negative_Index_Macro_Is_Empty_Text()
      {
         var expected = "";
         var actual = Substring(TEST, -2, 1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_Too_Large_Index_Macro_Is_Empty_Text()
      {
         var expected = "";
         var actual = Substring(TEST, 4, 1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Substring_Negative_Count_Macro_Is_Empty_Text()
      {
         var expected = "";
         var actual = Substring(TEST, 2, -1)(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Titlecase_Macro_Is_Titlecase_Text()
      {
         var expected = "What A Wonderful World";
         var actual = Titlecase(Text("what a wonderful world"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Trim_Empty_Macro_Is_Trimmed_Text()
      {
         var expected = "TEST";
         var actual = Trim(Text("   TEST   "))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Trim_Char_Macro_Is_Trimmed_Text()
      {
         var expected = "ES";
         var actual = Trim(TEST, new char[] { 'T' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Trim_Chars_Macro_Is_Trimmed_Text()
      {
         var expected = "E";
         var actual = Trim(TEST, new char[] { 'T', 'S' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void TrimEnd_Empty_Macro_Is_Trimmed_Text()
      {
         var expected = "TEST";
         var actual = TrimEnd(Text("TEST   "))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void TrimEnd_Char_Macro_Is_Trimmed_Text()
      {
         var expected = "TES";
         var actual = TrimEnd(TEST, new char[] { 'T' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void TrimEnd_Chars_Macro_Is_Trimmed_Text()
      {
         var expected = "TE";
         var actual = TrimEnd(TEST, new char[] { 'T', 'S' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void TrimStart_Empty_Macro_Is_Trimmed_Text()
      {
         var expected = "TEST";
         var actual = TrimStart(Text("   TEST"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void TrimStart_Char_Macro_Is_Trimmed_Text()
      {
         var expected = "EST";
         var actual = TrimStart(TEST, new char[] { 'T' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void TrimStart_Chars_Macro_Is_Trimmed_Text()
      {
         var expected = "ST";
         var actual = TrimStart(TEST, new char[] { 'T', 'E' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Uppercase_Macro_Is_Uppercase_Text()
      {
         var expected = "TEST";
         var actual = Uppercase(Text("test"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }
   }
}
using static Mackerel.Macro;

namespace Mackerel.Tests;

public static partial class MackerelTests
{
   public class ReplacementTests
   {
      [Fact]
      public void Replace_Char_Char_Instruction_Is_Replaced_Text()
      {
         var expected = "RESR";
         var actual = Replace(TEST, 'T', 'R')(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Chars_Chars_Instruction_One_To_One_Is_Replaced_Text()
      {
         var expected = "REAR";
         var actual = Replace(TEST, new char[] { 'T', 'S' }, new char[] { 'R', 'A' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Chars_Chars_Instruction_Missing_Mapping_Is_Replaced_Text()
      {
         var expected = "RER";
         var actual = Replace(TEST, new char[] { 'T', 'S' }, new char[] { 'R' })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_String_String_Instruction_Is_Replaced_Text()
      {
         var expected = "RAESRA";
         var actual = Replace(TEST, "T", "RA")(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_String_Instruction_Instruction_Is_Replaced_Text()
      {
         var expected = "RAESRAER";
         var actual = Replace(Text("TESTER"), "T", Text("RA"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_String_String_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TAESTE";
         var actual = Replace(TEST, "T", t =>
         {
            pattern.MoveNext();

            return $"{t}{pattern.Current}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_String_String_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESAT";
         var actual = Replace(TEST, "S", s =>
         {
            pattern.MoveNext();

            return $"{s}{pattern.Current}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_String_String_Index_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TA0ESTE1";
         var actual = Replace(TEST, "T", (t, i) =>
         {
            pattern.MoveNext();

            return $"{t}{pattern.Current}{i}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_String_String_Index_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESA0T";
         var actual = Replace(TEST, "S", (s, i) =>
         {
            pattern.MoveNext();

            return $"{s}{pattern.Current}{i}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_String_Instruction_Is_Replaced_Text()
      {
         var expected = "RAESRA";
         var actual = Replace(TEST, T, "RA")(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_Instruction_Instruction_Is_Replaced_Text()
      {
         var expected = "RAESRA";
         var actual = Replace(TEST, T, Text("RA"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_String_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TAESTE";
         var actual = Replace(TEST, T, t =>
         {
            pattern.MoveNext();

            return $"{t}{pattern.Current}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_String_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESAT";
         var actual = Replace(TEST, S, s =>
         {
            pattern.MoveNext();

            return $"{s}{pattern.Current}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_Instruction_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TAESTE";
         var actual = Replace(TEST, T, t =>
         {
            pattern.MoveNext();

            return Text($"{t}{pattern.Current}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_Instruction_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESAT";
         var actual = Replace(TEST, S, s =>
         {
            pattern.MoveNext();

            return Text($"{s}{pattern.Current}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_String_Index_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TA0ESTE1";
         var actual = Replace(TEST, T, (t, i) =>
         {
            pattern.MoveNext();

            return $"{t}{pattern.Current}{i}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_String_Index_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESA0T";
         var actual = Replace(TEST, S, (s, i) =>
         {
            pattern.MoveNext();

            return $"{s}{pattern.Current}{i}";
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_Instruction_Index_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TA0ESTE1";
         var actual = Replace(TEST, T, (t, i) =>
         {
            pattern.MoveNext();

            return Text($"{t}{pattern.Current}{i}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Regex_Instruction_Index_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESA0T";
         var actual = Replace(TEST, S, (s, i) =>
         {
            pattern.MoveNext();

            return Text($"{s}{pattern.Current}{i}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Instruction_Instruction_Instruction_Is_Replaced_Text()
      {
         var expected = "RAESRA";
         var actual = Replace(TEST, Text("T"), Text("RA"))(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Instruction_Instruction_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TAESTE";
         var actual = Replace(TEST, Text("T"), t =>
         {
            pattern.MoveNext();

            return Text($"{t}{pattern.Current}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Instruction_Instruction_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESAT";
         var actual = Replace(TEST, Text("S"), s =>
         {
            pattern.MoveNext();

            return Text($"{s}{pattern.Current}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Instruction_Instruction_Index_Function_Instruction_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TA0ESTE1";
         var actual = Replace(TEST, Text("T"), (t, i) =>
         {
            pattern.MoveNext();

            return Text($"{t}{pattern.Current}{i}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }

      [Fact]
      public void Replace_Instruction_Instruction_Index_Function_Instruction_Middle_Text_Is_Replaced_Text()
      {
         var pattern = new string[] { "A", "E", "I", "O", "U" }.GetEnumerator();
         var expected = "TESA0T";
         var actual = Replace(TEST, Text("S"), (s, i) =>
         {
            pattern.MoveNext();

            return Text($"{s}{pattern.Current}{i}");
         })(in Document.Empty);

         Assert.Equal(expected, actual);
      }
   }
}
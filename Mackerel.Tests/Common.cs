using System.Text.RegularExpressions;
using static Mackerel.Macro;

namespace Mackerel.Tests;

public static partial class MackerelTests
{
   private static readonly Instruction HELLO = Text("HELLO");
   private static readonly Instruction TEST = Text("TEST");
   private static readonly Instruction WORLD = Text("WORLD");
   private static readonly Regex S = SRegex();
   private static readonly Regex T = TRegex();

   [GeneratedRegex("S")]
   private static partial Regex SRegex();

   [GeneratedRegex("T")]
   private static partial Regex TRegex();
}
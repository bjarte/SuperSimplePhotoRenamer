using System;
using System.Text;

namespace Renamer.Helpers
{
    public static class OutputHelpers
    {
        public static void WriteColumns(string[] inputs)
        {
            var stringBuilder = new StringBuilder();
            foreach (var input in inputs)
            {
                var output = PadToColumnWidth(input);
                stringBuilder.Append(output);
            }
            Console.WriteLine(stringBuilder.ToString());
        }

        private static string PadToColumnWidth(string input)
        {
            if (input.Length > Settings.ColumnWidth)
            {
                input = input.Substring(0, Settings.ColumnWidth - 3);
                input += "...";
            }

            while (input.Length < Settings.ColumnWidth)
            {
                input = $" {input}";
            }

            return $"{input}{Settings.ColumnSpace}";
        }
    }
}

using System;
using System.Globalization;
using System.Linq;

namespace MediadataServices.Helpers
{
    public static class StringHelpers
    {
        public static DateTime ConvertToDateTime(this string input)
        {
            var output = new DateTime();

            var inputDigits = new string(input.ToCharArray().Where(x => char.IsDigit(x)).ToArray());

            if (inputDigits.Length < 12)
            {
                return output;
            }

            var year = int.Parse(inputDigits.Substring(0, 4));
            var month = int.Parse(inputDigits.Substring(4, 2));
            var day = int.Parse(inputDigits.Substring(6, 2));

            var hour = int.Parse(inputDigits.Substring(8, 2));
            var minute = int.Parse(inputDigits.Substring(10, 2));
            var second = inputDigits.Length >= 14 ? int.Parse(inputDigits.Substring(12, 2)) : 0;

            var dateString = $"{year}-{month}-{day} {hour}:{minute}:{second}";

            if (input.Contains("UTC"))
            {
                DateTime.TryParse(dateString
                           , CultureInfo.CurrentCulture
                           , DateTimeStyles.AssumeUniversal
                           , out output);
            }
            else
            {
                DateTime.TryParse(dateString, out output);
            }

            return output;
        }
    }
}

using System.Text;

namespace API.Utility
{
    public static class Conversoes
    {
        public static string ByteArrayParaString(byte[] arr)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                builder.Append(arr[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string SomenteNumeros(string texto)
        {
            return string.Join("", System.Text.RegularExpressions.Regex.Split(texto, @"[^\d]"));
        }

        public static DateTime DateTimeBr(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }
    }
}

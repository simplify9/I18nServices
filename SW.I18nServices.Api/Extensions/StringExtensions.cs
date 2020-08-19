using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18nService
{
    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            if (str == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}

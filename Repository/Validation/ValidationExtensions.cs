using System;
using System.Linq;

namespace Repository.Extensions
{
    public class ValidationExtensions
    {
        public static bool isSpecialChar(string content)
        {
            bool result = content.Any(ch => Char.IsPunctuation(ch) || Char.IsSymbol(ch));

            return result;
        }
    }
}

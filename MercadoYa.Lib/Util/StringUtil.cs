using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Lib.Util
{
    public class StringUtil
    {
        public static bool AnyNullOfWhiteSpace(params string[] Strings)
        {
            return Strings.Any(x => string.IsNullOrWhiteSpace(x));
        }
        public static bool NoneNullOrWhiteSpace(params string[] Strings)
        {
            return !AnyNullOfWhiteSpace(Strings);
        }
        public static bool AllNullOrWhiteSpace(params string[] Strings)
        {
            return Strings.All(x => string.IsNullOrWhiteSpace(x));
        }
    }
}

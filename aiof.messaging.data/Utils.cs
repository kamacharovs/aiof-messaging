using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using JetBrains.Annotations;

namespace aiof.messaging.data
{
    public static partial class Utils
    {
        public static string ToSnakeCase(
            [NotNull] this string value)
        {
            return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}

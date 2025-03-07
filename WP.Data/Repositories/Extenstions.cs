using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public static class Extenstions
    {
        public static string ExtractMetaData(this string metavalue, string regexp )
        {
            //var match = Regex.Match(metavalue, @"s:\d+:\""(?<role>[^\""]+)\"";b:(?<value>\d);");
            var match = Regex.Match(metavalue, regexp);
            if (match.Success)
            {
                return match.Groups["role"].Value;
            }
            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Toolchest.Extensions
{
    public static class StringExtensions
    {
        public static string StripHtml(this string str)
        {
            return Regex.Replace(str, @"<[^>]+>", "");
        }

        public static string StripLinebreaks(this string str)
        {
            return Regex.Replace(str, @"[\r\n]", "", RegexOptions.Multiline);
        }

        public static string CompressWhitespace(this string str)
        {
            return Regex.Replace(str, @"\s+", " ").Trim();
        }

        public static string CleanMarkdown(this string str)
        {
            str = str.StripHtml();

            // strip carriage returns
            str = Regex.Replace(str, @"\r", "");

            // strip colons left behind by divs with classes
            str = Regex.Replace(str, @":::", "");

            // strip div classes
            str = Regex.Replace(str, @"\{\.[^}]+\}", "");

            // strip brackets that aren't links
            str = Regex.Replace(str, @"\[([^\]]+)\]([^\(])", "$1$2");

            // strip double spacing that isn't at the begining of a line
            str = Regex.Replace(str, @"(.) +", "$1 ");

            // strip trailing spaces
            str = Regex.Replace(str, @" +\n", "");

            // strip more than two newlines
            str = Regex.Replace(str, @"\n\n+", "\n\n", RegexOptions.Multiline);

            // remove locale from links
            str = str.Replace(@"/en-us/", "/");

            str = str.Trim('\n');

            return str;
        }

        public static string TruncateTo(this string str, int maxLength, string terminator = "...", bool clean = true)
        {
            string truncated;

            if(clean)
            {
                str = str
                    .StripHtml()
                    .StripLinebreaks()
                    .CompressWhitespace();
            }

            var max = maxLength - terminator.Length;
            if(str.Length < max)
            {
                truncated = str;
            }
            else
            {
                truncated = str.Substring(0, max - 1);

                // attempt to truncate back to logical space
                var lastSpaceIndex = truncated.LastIndexOf(" ");
                if(lastSpaceIndex > -1)
                {
                    truncated = str.Substring(0, lastSpaceIndex);
                }
                truncated += terminator;
            }
            return truncated;
        }
    }
}

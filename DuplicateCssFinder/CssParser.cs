namespace DuplicateCssFinder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The css parser.
    /// </summary>
    public static class CssParser
    {
        #region Constants

        /// <summary>
        /// The comment pattern.
        /// </summary>
        private const string CommentPattern = @"\/\*(.|\n|\r)+?\*\/";

        /// <summary>
        /// The new line pattern.
        /// </summary>
        private const string NewLinePattern = @"[\r\n]+";

        /// <summary>
        /// The property group.
        /// </summary>
        private const string PropertyGroup = "property";

        /// <summary>
        /// The rule pattern. It is based on http://stackoverflow.com/questions/236979/parsing-css-by-regex/2694121#2694121.
        /// </summary>
        private const string RulePattern = @"(?:(?:(?<selector>(?:[^,{]+)),?)*?)\{(?:(?<property>[^}:]+):?(?<value>[^};]+);?)*?\s*}";

        /// <summary>
        /// The selector group.
        /// </summary>
        private const string SelectorGroup = "selector";

        /// <summary>
        /// The value group.
        /// </summary>
        private const string ValueGroup = "value";

        #endregion

        #region Static Fields

        /// <summary>
        /// The comment regex.
        /// </summary>
        private static readonly Regex CommentRegex = new Regex(CommentPattern, RegexOptions.Compiled);

        /// <summary>
        /// The new line regex.
        /// </summary>
        private static readonly Regex NewLineRegex = new Regex(NewLinePattern, RegexOptions.Compiled);

        /// <summary>
        /// The style regex.
        /// </summary>
        private static readonly Regex RuleRegex = new Regex(RulePattern, RegexOptions.Compiled);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses the specified styles.
        /// </summary>
        /// <param name="s">
        /// A string containing a styles.
        /// </param>
        /// <returns>
        /// The list of styles.
        /// </returns>
        public static IEnumerable<CssRule> Parse(string s)
        {
            List<CssRule> styles = new List<CssRule>();

            if (!string.IsNullOrEmpty(s))
            {
                string withoutNewLines = NewLineRegex.Replace(s, " ");
                string withoutComments = CommentRegex.Replace(withoutNewLines, string.Empty);

                MatchCollection matches = RuleRegex.Matches(withoutComments);
                foreach (Match item in matches)
                {
                    CssRule rule = new CssRule
                                   {
                                       Selectors = item.Groups[SelectorGroup].Captures.Cast<Capture>()
                                                                             .Select(capture => capture.Value.Trim())
                                                                             .ToList()
                                   };

                    for (int i = 0; i < item.Groups[PropertyGroup].Captures.Count; i++)
                    {
                        rule.Declarations.Add(new CssDeclaration
                                              {
                                                  Property = item.Groups[PropertyGroup].Captures[i].Value.Trim(), 
                                                  Value = item.Groups[ValueGroup].Captures[i].Value.Trim()
                                              });
                    }

                    styles.Add(rule);
                }
            }

            return styles;
        }

        #endregion
    }
}
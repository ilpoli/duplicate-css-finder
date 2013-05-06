namespace DuplicateCssFinder
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Forms;
    using System.Windows.Media;

    using FlowDirection = System.Windows.FlowDirection;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the groups of duplicates.
        /// </summary>
        /// <param name="rules">
        /// The rules.
        /// </param>
        /// <returns>
        /// The groups of duplicates.
        /// </returns>
        private static List<List<CssDuplicate>> GetGroups(List<CssDuplicate> rules)
        {
            List<List<CssDuplicate>> groups = new List<List<CssDuplicate>>();

            for (int i = 0; i < rules.Count - 1; i++)
            {
                List<CssDuplicate> duplicates = null;
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (rules[j].Processed || !RuleCompare(rules[i].Rule, rules[j].Rule))
                    {
                        continue;
                    }

                    (duplicates ?? (duplicates = new List<CssDuplicate> { rules[i] })).Add(rules[j]);
                    rules[j].Processed = true;
                }

                rules[i].Processed = true;

                if (duplicates != null)
                {
                    groups.Add(duplicates);
                }
            }

            return groups;
        }

        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="directory">
        /// The directory with rules.
        /// </param>
        /// <returns>
        /// The rules.
        /// </returns>
        private static List<CssDuplicate> GetRules(string directory)
        {
            List<CssDuplicate> rules = new List<CssDuplicate>();

            // For additional filtering.
            Regex searchRegex = new Regex(@".*?(?<!Combined)\.css", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string[] paths = Directory.GetFiles(directory, "*.css", SearchOption.AllDirectories);
            foreach (string path in paths.Where(path => searchRegex.IsMatch(path)))
            {
                rules.AddRange(CssParser.Parse(File.ReadAllText(path)).Select(rule => new CssDuplicate { Rule = rule, Path = path }));
            }

            return rules.OrderBy(duplicate => duplicate.Rule.Declarations.Count).ToList();
        }

        /// <summary>
        /// Gets the width of the string.
        /// </summary>
        /// <param name="s">
        /// The string.
        /// </param>
        /// <param name="paragraph">
        /// The paragraph.
        /// </param>
        /// <returns>
        /// The width.
        /// </returns>
        private static double GetStringWidth(string s, Paragraph paragraph)
        {
            if (paragraph == null)
            {
                throw new ArgumentNullException("paragraph");
            }

            FormattedText text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(paragraph.FontFamily, paragraph.FontStyle, paragraph.FontWeight, paragraph.FontStretch), paragraph.FontSize, Brushes.Black);
            return text.Width;
        }

        /// <summary>
        /// Rules the compare.
        /// </summary>
        /// <param name="a">
        /// The first rule to compare.
        /// </param>
        /// <param name="b">
        /// The second rule to compare.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="CssRule"/> is equal to this instance; otherwise, <c>false</c>
        /// </returns>
        private static bool RuleCompare(CssRule a, CssRule b)
        {
            if (a.Declarations.Count == 0 || b.Declarations.Count == 0)
            {
                return false;
            }

            int count = a.Declarations.Count(d1 => b.Declarations.Any(d2 => d2.Equals(d1)));

            // Fuzzy comparison for big declarations.
            if (Math.Min(a.Declarations.Count, b.Declarations.Count) > 2 && Math.Abs(a.Declarations.Count - b.Declarations.Count) < 2)
            {
                return count == a.Declarations.Count || count == b.Declarations.Count;
            }

            return a.Declarations.Count == b.Declarations.Count && a.Declarations.Count == count;
        }

        /// <summary>
        /// Builds the document.
        /// </summary>
        /// <param name="groups">
        /// The groups.
        /// </param>
        private void BuildDocument(List<List<CssDuplicate>> groups)
        {
            double width = 0;
            FlowDocument document = new FlowDocument();
            document.Blocks.Add(new Paragraph(new Run(string.Format("Count of groups: {0}.", groups.Count))) { Style = (Style)this.Resources["Paragraph"] });

            foreach (List<CssDuplicate> duplicates in groups)
            {
                Paragraph paragraph = new Paragraph { Style = (Style)this.Resources["Paragraph"] };
                document.Blocks.Add(paragraph);

                foreach (CssDuplicate duplicate in duplicates)
                {
                    if (paragraph.Inlines.Any())
                    {
                        paragraph.Inlines.Add(new LineBreak());
                        paragraph.Inlines.Add(new LineBreak());
                    }

                    paragraph.Inlines.Add(new Run(duplicate.Path) { Style = (Style)this.Resources["Path"] });
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(new Run(duplicate.Rule.ToString()));

                    // Calculates the width of the longest line to avoid text wrapping.
                    width = Math.Max(Math.Max(GetStringWidth(duplicate.Path, paragraph), GetStringWidth(duplicate.Rule.ToString(), paragraph)), width);
                }
            }

            this.Viewer.Document = document;
            this.Viewer.Document.MinPageWidth = width + 70;
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"D:\Intake\Source\Wilco.Presentation.Web\Intake\Styles";

            // dialog.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult dialogResult = dialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.BuildDocument(GetGroups(GetRules(dialog.SelectedPath)));
            }
            else
            {
                this.Close();
            }
        }

        #endregion
    }
}

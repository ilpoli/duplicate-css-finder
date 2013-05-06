namespace DuplicateCssFinder
{
    /// <summary>
    /// The css duplicate.
    /// </summary>
    public class CssDuplicate
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CssDuplicate"/> is processed.
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        public CssRule Rule { get; set; }

        #endregion
    }
}
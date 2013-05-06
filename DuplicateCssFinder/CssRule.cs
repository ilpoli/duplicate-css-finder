namespace DuplicateCssFinder
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The css rule.
    /// </summary>
    public class CssRule
    {
        #region Fields

        /// <summary>
        /// The declarations.
        /// </summary>
        private List<CssDeclaration> declarations;

        /// <summary>
        /// The selectors.
        /// </summary>
        private List<string> selectors;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the declarations.
        /// </summary>
        public List<CssDeclaration> Declarations
        {
            get
            {
                return this.declarations ?? (this.declarations = new List<CssDeclaration>());
            }

            set
            {
                this.declarations = value;
            }
        }

        /// <summary>
        /// Gets or sets the selectors.
        /// </summary>
        public List<string> Selectors
        {
            get
            {
                return this.selectors ?? (this.selectors = new List<string>());
            }

            set
            {
                this.selectors = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(", ", this.Selectors));
            sb.Append(" { ");
            sb.Append(string.Join(" ", this.Declarations));
            sb.Append(this.Declarations.Count > 0 ? " " : string.Empty);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion
    }
}

namespace DuplicateCssFinder
{
    /// <summary>
    /// The css declaration.
    /// </summary>
    public class CssDeclaration
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((CssDeclaration)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Property != null ? this.Property.GetHashCode() : 0) * 397) ^ (this.Value != null ? this.Value.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: {1};", this.Property, this.Value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="CssDeclaration"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(CssDeclaration other)
        {
            return string.Equals(this.Property, other.Property) && string.Equals(this.Value, other.Value);
        }

        #endregion
    }
}

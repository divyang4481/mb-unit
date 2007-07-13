using System;

namespace MbUnit.Framework.Core.Attributes
{
    /// <summary>
    /// A decorator attributes applies contributions to a primary object generated by some other
    /// pattern attribute.  Occasionally the order in which decorators are applied is significant
    /// so this type provides a <see cref="Order" /> property to specify an explicit ordering
    /// when required.
    /// </summary>
    public abstract class DecoratorPatternAttribute : Attribute
    {
        private int order = 0;

        /// <summary>
        /// Gets or sets the order in which the decorator should be applied.
        /// Decorators with lower order indices values are applied before decorators with
        /// higher ones.  In the case of a tie, an arbitrary choice is
        /// made among decorators with the same order index to determine the order in
        /// which they will be processed.
        /// </summary>
        /// <value>
        /// The default order index is 0.
        /// </value>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        /// <summary>
        /// Sorts decorators by priority order.
        /// </summary>
        /// <param name="decoratorPatternAttributes">The array to sort</param>
        public static void Sort(object[] decoratorPatternAttributes)
        {
            Array.Sort(decoratorPatternAttributes, DecoratorOrderComparer.Instance);
        }
    }
}

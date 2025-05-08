using System;
using System.Reflection;

namespace Pike.OneS
{
    /// <summary>
    /// Represent <see cref="OneSValueTable"/> row
    /// </summary>
    public class OneSValueTableRow: OneSBaseComObject
    {
        /// <summary>
        /// Create an instance of <see cref="OneSValueTableRow"/>
        /// </summary>
        /// <param name="comValueTableRow">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        internal OneSValueTableRow(dynamic comValueTableRow)
        {
            if (comValueTableRow == null) throw new ArgumentNullException(nameof(comValueTableRow));

            ComObject = comValueTableRow;
        }

        /// <summary>
        /// Get or set value by index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Value</returns>
        public object this[int index]
        {
            get => ComObject.Get(index);
            set => ComObject.Set(index, value);
        }

        /// <summary>
        /// Get or set value by column name
        /// </summary>
        /// <param name="name">Column name</param>
        /// <returns>Value</returns>
        public object this[string name]
        {
            get =>
                ComObject.GetType().InvokeMember(name,
                    BindingFlags.Public | BindingFlags.GetProperty,
                    null, ComObject,
                    null);
            set
            {
                ComObject.GetType().InvokeMember(name,
                    BindingFlags.Public | BindingFlags.SetProperty,
                    null, ComObject,
                    new[] {value} );
            }
        }
    }
}

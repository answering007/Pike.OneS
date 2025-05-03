using System;
using System.Collections;
using System.Collections.Generic;

namespace Pike.OneS
{
    /// <summary>
    /// Represent the collection of a <see cref="OneSValueTableColumn"/>
    /// </summary>
    public class OneSValueTableColumnCollection: OneSBaseComObject, IEnumerable<OneSValueTableColumn>
    {
        /// <summary>
        /// Create an instance of <see cref="OneSValueTableColumnCollection"/>
        /// </summary>
        /// <param name="comValueTableColumnCollection">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        internal OneSValueTableColumnCollection(dynamic comValueTableColumnCollection)
        {
            if (comValueTableColumnCollection == null) throw new ArgumentNullException(nameof(comValueTableColumnCollection));

            ComObject = comValueTableColumnCollection;
        }

        /// <summary>
        /// Get the value by it's index
        /// </summary>
        /// <param name="index">Index of the value</param>
        /// <returns>Table row of type <see cref="OneSValueTableColumn"/></returns>
        public OneSValueTableColumn this[int index] => new OneSValueTableColumn(ComObject.Get(index));

        /// <summary>
        /// Get th number of elements in the <see cref="OneSValueTableColumnCollection"/>
        /// </summary>
        public int Count => ComObject.Count();


        /// <summary>
        /// <see cref="IEnumerable{T}"/> implementation
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<OneSValueTableColumn> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        /// <summary>
        /// <see cref="IEnumerable"/> implementation
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Pike.OneS
{
    /// <summary>
    /// 1C value table collection
    /// </summary>
    public class OneSValueTable: OneSBaseComObject, IEnumerable<OneSValueTableRow>
    {
        const string Name = "ValueTable";

        /// <summary>
        /// Create an instance of <see cref="OneSValueTable"/>
        /// </summary>
        /// <param name="comValueTable">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        public OneSValueTable(dynamic comValueTable)
        {
            if (comValueTable == null) throw new ArgumentNullException(nameof(comValueTable));
            if (!Marshal.IsComObject(comValueTable)) throw new ArgumentException("comValueTable must be a 1C COM object");

            ComObject = comValueTable;
        }

        /// <summary>
        /// Create an instance of <see cref="OneSValueTable"/>
        /// </summary>
        /// <param name="connector">1C connector</param>
        public OneSValueTable(OneSConnector connector)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (!connector.IsConnected) throw new InvalidOperationException("Connector is not connected");

            ComObject = connector.ComObject.NewObject(Name);
        }

        /// <summary>
        /// Get the value by it's index
        /// </summary>
        /// <param name="index">Index of the value</param>
        /// <returns>Table row of type <see cref="OneSValueTableRow"/></returns>
        public OneSValueTableRow this[int index] => new OneSValueTableRow(ComObject.Get(index));

        /// <summary>
        /// Get th number of elements in the <see cref="OneSValueTable"/>
        /// </summary>
        public int Count => ComObject.Count();

        /// <summary>
        /// Get columns collection
        /// </summary>
        public OneSValueTableColumnCollection Columns => new OneSValueTableColumnCollection(ComObject.Columns);

        /// <summary>
        /// <see cref="IEnumerable{T}"/> implementation
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<OneSValueTableRow> GetEnumerator()
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

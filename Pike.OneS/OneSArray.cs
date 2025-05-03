using System;
using System.Collections;

namespace Pike.OneS
{
    /// <summary>
    /// Represent 1C array
    /// </summary>
    public class OneSArray: OneSBaseComObject, IEnumerable
    {
        const string Name = "Array";

        /// <summary>
        /// Create an instance of <see cref="OneSArray"/>
        /// </summary>
        /// <param name="comArray">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        internal OneSArray(dynamic comArray)
        {
            if (comArray == null) throw new ArgumentNullException(nameof(comArray));

            ComObject = comArray;
        }

        /// <summary>
        /// Create an instance of <see cref="OneSArray"/>
        /// </summary>
        /// <param name="connector">1C connector</param>
        public OneSArray(OneSConnector connector)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (!connector.IsConnected) throw new InvalidOperationException("Connector is not connected");

            ComObject = connector.ComObject.NewObject(Name);
        }

        /// <summary>
        /// Get index upper bound
        /// </summary>
        public int UBound => ComObject.UBound();

        /// <summary>
        /// Get th number of elements in the <see cref="OneSArray"/>
        /// </summary>
        public int Count => ComObject.Count();

        /// <summary>
        /// Insert value into the <see cref="OneSArray"/>
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">Value to insert</param>
        public void Insert(int index, object value = null)
        {
            ComObject.Insert(index, value);
        }

        /// <summary>
        /// Add value to the <see cref="OneSArray"/>
        /// </summary>
        /// <param name="value">Value to add</param>
        public void Add(object value = null)
        {
            ComObject.Add(value);
        }

        /// <summary>
        /// Find the value
        /// </summary>
        /// <param name="value">Value to find</param>
        /// <returns>Index of the value</returns>
        public int Find(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var rst = ComObject.Find(value) as int?;
            return rst ?? -1;
        }

        /// <summary>
        /// Clear all values from the <see cref="OneSArray"/>
        /// </summary>
        public void Clear()
        {
            ComObject.Clear();
        }

        /// <summary>
        /// Get or set the value by it's index
        /// </summary>
        /// <param name="index">Index of the value</param>
        /// <returns>Value</returns>
        public object this[int index]
        {
            get { return ComObject.Get(index); }
            set { ComObject.Set(index, value); }
        }

        /// <summary>
        /// Delete the value by it's index
        /// </summary>
        /// <param name="index">Index of the value</param>
        public void Delete(int index)
        {
            ComObject.Delete(index);
        }

        /// <summary>
        /// <see cref="IEnumerable"/> implementation
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }
    }
}

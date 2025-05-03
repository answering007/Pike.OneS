using System;
using System.Collections.Generic;
using System.Linq;

namespace Pike.OneS
{
    /// <summary>
    /// Stores collection of <see cref="OneSQueryResultColumn"/>
    /// </summary>
    public class OneSQueryResultColumnsCollection : IEnumerable<OneSQueryResultColumn>
    {
        readonly Dictionary<string, OneSQueryResultColumn> _columns = new Dictionary<string, OneSQueryResultColumn>();
        readonly List<string> _names = new List<string>();

        /// <summary>
        /// Create an instance of <see cref="OneSQueryResultColumnsCollection"/>
        /// </summary>
        /// <param name="columns">Collection of columns</param>
        internal OneSQueryResultColumnsCollection(IEnumerable<OneSQueryResultColumn> columns)
        {
            foreach (var column in columns)
            {
                _columns.Add(column.Name, column);
                _names.Add(column.Name);
            }
        }
        
        /// <summary>
        /// True of column with specific name is found in the column collection; otherwise false
        /// </summary>
        /// <param name="key">Column name</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key can't be null or empty");

            return _columns.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key
        /// </summary>
        /// <param name="key">Column name</param>
        /// <param name="column">When this method returns, contains the value associated with the specified key, 
        /// if the key is found; otherwise, the default value for the type of the value parameter</param>
        /// <returns>True of column with specific name is found in the column collection; otherwise false</returns>
        public bool TryGetValue(string key, out OneSQueryResultColumn column)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key can't be null or empty");

            return _columns.TryGetValue(key, out column);
        }

        /// <summary>
        /// Number of columns in the collection
        /// </summary>
        public int Count => _columns.Count;

        /// <summary>
        /// Get the column by name
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Associated column</returns>
        public OneSQueryResultColumn this[string columnName]
        {
            get
            {
                if (!ContainsKey(columnName)) throw new KeyNotFoundException();
                return _columns[columnName];
            }
        }

        /// <summary>
        /// Get the column by index
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns>Associated column</returns>
        public OneSQueryResultColumn this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0) throw new IndexOutOfRangeException("columnIndex < 0");
                if (columnIndex >= Count) throw new IndexOutOfRangeException("columnIndex >= Count");

                return _columns.ElementAt(columnIndex).Value;
            }
        }

        /// <summary>
        /// Get index for the column
        /// </summary>
        /// <param name="column">Column to find</param>
        /// <returns>Zero-based column index</returns>
        public int IndexOf(OneSQueryResultColumn column)
        {
            return IndexOf(column.Name);
        }

        /// <summary>
        /// Get index by the column name
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns>Zero-based column index</returns>
        public int IndexOf(string columnName)
        {
            if (columnName == null) throw new ArgumentNullException(nameof(columnName));
            if (!_columns.ContainsKey(columnName)) throw new KeyNotFoundException("Can't found the column");

            return _names.IndexOf(columnName);
        }

        /// <summary>
        /// Implementation of <see cref="IEnumerable{T}"/> interface
        /// </summary>
        /// <returns>Enumerator of the column collection</returns>
        public IEnumerator<OneSQueryResultColumn> GetEnumerator()
        {
            return _columns.Values.GetEnumerator();
        }

        /// <summary>
        /// Implementation of <see cref="System.Collections.IEnumerator"/> interface
        /// </summary>
        /// <returns>Enumerator of the column collection</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

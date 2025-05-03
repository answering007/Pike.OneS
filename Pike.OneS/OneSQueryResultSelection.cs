using System;
using System.Collections;

namespace Pike.OneS
{
    /// <summary>
    /// Query result Iteration object
    /// </summary>
    public class OneSQueryResultSelection : OneSBaseComObject, IEnumerable, IEnumerator
    {
        readonly OneSQueryResultColumnsCollection _columns;

        /// <summary>
        /// Create an instance of <see cref="OneSQueryResultSelection"/>
        /// </summary>
        /// <param name="result">Source query result</param>
        public OneSQueryResultSelection(OneSQueryResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            ComObject = result.ComObject.Выбрать();
            _columns = result.Columns;
        }

        /// <summary>
        /// Get value from current record by the column index
        /// </summary>
        /// <param name="index">Column index</param>
        /// <returns>Value from the current record</returns>
        public object this[int index]
        {
            get
            {
                var value = ComObject.Get(index);
                if (value == null) return DBNull.Value;
                if (DBNull.Value.Equals(value)) return DBNull.Value;

                if (_columns[index].ManagedType == typeof(bool))
                    return (bool)value;
                if (_columns[index].ManagedType == typeof(DateTime))
                    return (DateTime)value;
                if (_columns[index].ManagedType == typeof(decimal))
                    return (decimal)value;
                return (string)value;
            }
        }

        /// <summary>
        /// Get record count from query result
        /// </summary>
        public int Count => ComObject.Count();

        /// <summary>
        /// Get the array of values from the current row
        /// </summary>
        public object[] CurrentRow
        {
            get
            {
                var values = new object[_columns.Count];
                for (var i = 0; i < _columns.Count; i++)
                    values[i] = this[i];
                return values;
            }
        }

        /// <summary>
        /// <see cref="IEnumerable"/> interface implementation
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        #region IEnumerator

        /// <summary>
        /// Get the array of values from the current row (<see cref="IEnumerator"/> implementation)
        /// </summary>
        public object Current => CurrentRow;

        /// <summary>
        /// Get next record from query iteration. True, if next record is available; otherwise false
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            return ComObject.Next();
        }

        /// <summary>
        /// Move to the top of the result
        /// </summary>
        public void Reset()
        {
            ComObject.Reset();
        } 

        #endregion
    }
}

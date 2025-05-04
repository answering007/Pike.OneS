using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Collections;

namespace Pike.OneS.Data
{
    /// <summary>
    /// An <see cref="DbParameterCollection"/> implementation
    /// </summary>
    public class OneSDbParameterCollection : DbParameterCollection
    {
        readonly SortedList<string, OneSDbParameter> _list = new SortedList<string, OneSDbParameter>();

        /// <summary>
        /// Specifies the number of items in the collection
        /// </summary>
        public override int Count => _list.Count;

        /// <summary>
        /// Specifies whether the collection is a fixed size. Current value is false
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Specifies whether the collection is read-only. Current value is false
        /// </summary>
        public override bool IsReadOnly => false;

        /// <summary>
        /// Specifies whether the collection is synchronized. Current value is true
        /// </summary>
        public override bool IsSynchronized => true;

        /// <summary>
        /// Specifies the <see cref="object"/> to be used to synchronize access to the collection
        /// </summary>
        public override object SyncRoot { get; } = new object();

        /// <summary>
        /// Adds the specified <see cref="OneSDbParameter"/> object to the <see cref="OneSDbParameterCollection"/>
        /// </summary>
        /// <param name="value">The value of the <see cref="OneSDbParameter"/> to add to the collection</param>
        /// <returns>The index of the <see cref="OneSDbParameter"/> object in the collection</returns>
        public override int Add(object value)
        {
            return Add((OneSDbParameter)value);
        }

        /// <summary>
        /// Adds the specified <see cref="OneSDbParameter"/> object to the <see cref="OneSDbParameterCollection"/>
        /// </summary>
        /// <param name="parameter">The value of the <see cref="OneSDbParameter"/> to add to the collection</param>
        /// <returns>The index of the <see cref="OneSDbParameter"/> object in the collection</returns>
        public int Add(OneSDbParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (string.IsNullOrWhiteSpace(parameter.ParameterName)) throw new ArgumentException("ParameterName property can't be null or empty");
            if (_list.ContainsKey(parameter.ParameterName)) throw new ArgumentException("The given key is already exist");
                        
            _list.Add(parameter.ParameterName, parameter);
            return _list.Count - 1;
        }

        /// <summary>
        /// Adds the parameter by its name and value
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>The index of the <see cref="OneSDbParameter"/> object in the collection</returns>
        public int Add(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException("parameterName can't be null or empty");
            return Add(new OneSDbParameter() { ParameterName = parameterName, Value = value });
        }

        /// <summary>
        /// Adds an array of items with the specified values to the current parameters collection
        /// </summary>
        /// <param name="values">An array of values of type <see cref="OneSDbParameter"/> to add to the collection</param>
        public override void AddRange(Array values)
        {
            AddRange(values.Cast<OneSDbParameter>());
        }

        /// <summary>
        /// Adds an array of items with the specified values to the current parameters collection
        /// </summary>
        /// <param name="parameters">A collection of values of type <see cref="OneSDbParameter"/> to add to the collection</param>
        public void AddRange(IEnumerable<OneSDbParameter> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
                Add(parameter);
        }

        /// <summary>
        /// Remove all parameters from the current collection
        /// </summary>
        public override void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Indicates whether a <see cref="OneSDbParameter"/> with the specified name exists in the collection
        /// </summary>
        /// <param name="value">The name of the <see cref="OneSDbParameter"/> to look for in the collection</param>
        /// <returns>true if the <see cref="OneSDbParameter"/> is in the collection; otherwise false</returns>
        public override bool Contains(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value can't be null or empty");

            return _list.ContainsKey(value);
        }

        /// <summary>
        /// Indicates whether a <see cref="OneSDbParameter"/> exists in the collection
        /// </summary>
        /// <param name="value">The <see cref="OneSDbParameter"/> to look for</param>
        /// <returns>true if the <see cref="OneSDbParameter"/> is in the collection; otherwise false</returns>
        public override bool Contains(object value)
        {
            return Contains((OneSDbParameter)value);
        }

        /// <summary>
        /// Indicates whether a <see cref="OneSDbParameter"/> exists in the collection
        /// </summary>
        /// <param name="parameter">The <see cref="OneSDbParameter"/> to look for</param>
        /// <returns>true if the <see cref="OneSDbParameter"/> is in the collection; otherwise false</returns>
        public bool Contains(OneSDbParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (string.IsNullOrWhiteSpace(parameter.ParameterName)) throw new ArgumentException("ParameterName property can't be null or empty");

            return _list.ContainsKey(parameter.ParameterName);
        }

        /// <summary>
        /// Copies an array of items to the collection starting at the specified index
        /// </summary>
        /// <param name="array">The array of items to copy to the collection</param>
        /// <param name="index">The index in the collection to copy the items</param>
        public override void CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if ((index < 0) || (index > array.Length)) throw new ArgumentOutOfRangeException(nameof(index));
            if ((array.Length - index) < _list.Count) throw new ArgumentException("(array.Length - index) < _list.Count");

            var q = _list.Values.ToArray();
            for (var i = index; i < Count; i++)
                array.SetValue(q[i], index++);
        }

        /// <summary>
        /// Exposes the GetEnumerator() method, which supports a simple iteration over a collection by a .NET Framework data provider
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection</returns>
        public override IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Returns the index of the <see cref="OneSDbParameter"/> object with the specified name
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="OneSDbParameter"/> object in the collection</param>
        /// <returns>The index of the <see cref="OneSDbParameter"/> object with the specified name</returns>
        public override int IndexOf(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException("parameterName can't be null or empty");

            return _list.IndexOfKey(parameterName);
        }

        /// <summary>
        /// Returns the index of the <see cref="OneSDbParameter"/> object
        /// </summary>
        /// <param name="parameter">The <see cref="OneSDbParameter"/> object in the collection</param>
        /// <returns>The index of the <see cref="OneSDbParameter"/> object with the specified name</returns>
        public int IndexOf(OneSDbParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return IndexOf(parameter.ParameterName);
        }

        /// <summary>
        /// Returns the index of the <see cref="OneSDbParameter"/> object
        /// </summary>
        /// <param name="value">The <see cref="OneSDbParameter"/> object in the collection</param>
        /// <returns>The index of the <see cref="OneSDbParameter"/> object with the specified name</returns>
        public override int IndexOf(object value)
        {
            return IndexOf((OneSDbParameter)value);
        }

        /// <summary>
        /// Inserts the specified index of the <see cref="OneSDbParameter"/> object with the specified name into the collection at the specified index
        /// </summary>
        /// <param name="index">The index at which to insert the <see cref="OneSDbParameter"/> object</param>
        /// <param name="value">The <see cref="OneSDbParameter"/> object to insert into the collection</param>
        public override void Insert(int index, object value)
        {
            Add((OneSDbParameter)value);
        }

        /// <summary>
        /// Removes the <see cref="OneSDbParameter"/> object from the collection
        /// </summary>
        /// <param name="value">The <see cref="OneSDbParameter"/> object to remove</param>
        public override void Remove(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var parameter = (OneSDbParameter)value;
            RemoveAt(parameter.ParameterName);
        }

        /// <summary>
        /// Removes the <see cref="OneSDbParameter"/> object with the specified name from the collection
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="OneSDbParameter"/> object to remove</param>
        public override void RemoveAt(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException("parameterName can't be null or empty");

            _list.Remove(parameterName);
        }

        /// <summary>
        /// Removes the <see cref="OneSDbParameter"/> object at the specified from the collection
        /// </summary>
        /// <param name="index">The index where the <see cref="OneSDbParameter"/> object is located</param>
        public override void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Returns <see cref="OneSDbParameter"/> the object with the specified name
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="OneSDbParameter"/> in the collection</param>
        /// <returns>The <see cref="OneSDbParameter"/> object with the specified name</returns>
        protected override DbParameter GetParameter(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException("parameterName can't be null or empty");

            return _list[parameterName];
        }

        /// <summary>
        /// Returns the <see cref="OneSDbParameter"/> object at the specified index in the collection
        /// </summary>
        /// <param name="index">The index of the <see cref="OneSDbParameter"/> in the collection</param>
        /// <returns>The <see cref="OneSDbParameter"/> object at the specified index in the collection</returns>
        protected override DbParameter GetParameter(int index)
        {
            return _list.ElementAt(index).Value;
        }

        /// <summary>
        /// Sets the <see cref="OneSDbParameter"/> object with the specified name to a new value
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="OneSDbParameter"/> object in the collection</param>
        /// <param name="value">The new <see cref="OneSDbParameter"/> value</param>
        protected override void SetParameter(string parameterName, DbParameter value)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException("parameterName can't be null or empty");
            if (value == null) throw new ArgumentNullException(nameof(value));
            var parameter = (OneSDbParameter)value;
            if (string.IsNullOrWhiteSpace(parameter.ParameterName))
                parameter.ParameterName = parameterName;
            _list[parameterName] = parameter;
        }

        /// <summary>
        /// Sets the <see cref="OneSDbParameter"/> object at the specified index to a new value
        /// </summary>
        /// <param name="index">The index where the <see cref="OneSDbParameter"/> object is located</param>
        /// <param name="value">The new <see cref="OneSDbParameter"/> value</param>
        protected override void SetParameter(int index, DbParameter value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var name = GetParameter(index).ParameterName;
            SetParameter(name, value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        public IEnumerable<OneSDbParameter> Values => _list.Values;
    }
}

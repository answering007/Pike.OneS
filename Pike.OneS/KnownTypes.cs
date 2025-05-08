using System;
using System.Collections;
using System.Collections.Generic;

namespace Pike.OneS
{
    /// <summary>
    /// Known 1C types dictionary. Key: 1C string type name. Values: managed type
    /// </summary>
    public class KnownTypes: IEnumerable<KeyValuePair<string, Type>>
    {
        /// <summary>
        /// String type name
        /// </summary>
        public const string StringType = "string";

        static KnownTypes _values;

        /// <summary>
        /// Current instance of <see cref="KnownTypes"/>
        /// </summary>
        public static KnownTypes Values => _values ?? (_values = new KnownTypes());

        readonly Dictionary<string, Type> _dictionary;

        KnownTypes()
        {
            _dictionary = new Dictionary<string, Type>
            {
                {StringType, typeof (string)},
                {"datetime", typeof (DateTime)},
                {"decimal", typeof (decimal)},
                {"boolean", typeof (bool)},
            };
        }

        /// <summary>
        /// 1C type names collection
        /// </summary>
        public IEnumerable<string> Keys => _dictionary.Keys;

        /// <summary>
        /// Determines whether dictionary contain 1C type
        /// </summary>
        /// <param name="key">1C type name</param>
        /// <returns>True if type is contained; otherwise False</returns>
        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key can't be null or empty");

            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Get managed type by 1C name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Type this[string key] => _dictionary[key];

        /// <summary>
        /// <see cref="IEnumerable{T}"/> interface implementation
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
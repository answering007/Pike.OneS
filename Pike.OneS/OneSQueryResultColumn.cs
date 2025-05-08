using System;
namespace Pike.OneS
{
    /// <summary>
    /// Store information about 1C query result column
    /// </summary>
    public class OneSQueryResultColumn
    {
        /// <summary>
        /// Column name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Column managed type (string, DateTime, decimal or boolean)
        /// </summary>
        public Type ManagedType { get; }

        /// <summary>
        /// True if <see cref="ManagedType"/> is defined; otherwise false
        /// </summary>
        public bool IsDefined => ManagedType != null;

        /// <summary>
        /// Column index
        /// </summary>
        public int InternalIndex { get; internal set; }

        /// <summary>
        /// Create an instance of <see cref="OneSQueryResultColumn"/>
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="managedType">Column managed data type</param>
        internal OneSQueryResultColumn(string name, Type managedType)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name can't be null or empty");

            Name = name;
            ManagedType = managedType;
        }

        /// <summary>
        /// Create undefined column
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="index">Column index</param>
        /// <returns></returns>
        internal static OneSQueryResultColumn Unknown(string name, int index)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name can't be null or empty");
            
            return new OneSQueryResultColumn(name, null) {InternalIndex = index};
        }

        /// <summary>
        /// Represent query result column as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Name={Name}; Type={(IsDefined ? ManagedType.Name : "Unknown")}";
        }
    }
}

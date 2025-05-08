using System;
using System.Data.Common;
using System.Data;

namespace Pike.OneS.Data
{
    /// <summary>
    /// Represents a parameter to a <see cref="OneSDbCommand"/>
    /// </summary>
    public class OneSDbParameter : DbParameter
    {
        /// <summary>
        /// Gets or sets the <see cref="DbType"/> of the parameter
        /// </summary>
        public override DbType DbType { get; set; } = DbType.Object;

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
        /// Supported type is input-only
        /// </summary>
        public override ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter accepts null values.
        /// Default value is false
        /// </summary>
        public override bool IsNullable { get; set; } = false;

        string _parameterName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the parameter. Can't be renamed after first set
        /// </summary>
        public override string ParameterName
        {
            get => _parameterName;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value can't be null or empty");
                if (!string.IsNullOrWhiteSpace(_parameterName)) throw new ArgumentException("ParameterName is already defined");

                _parameterName = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the parameter
        /// </summary>
        public override int Size { get; set; }

        /// <summary>
        /// Gets or sets the name of the source column mapped to the <see cref="DataSet"/> and used for loading or returning the <see cref="Value"/>
        /// </summary>
        public override string SourceColumn { get; set; }

        /// <summary>
        /// Sets or gets a value which indicates whether the source column is nullable
        /// </summary>
        public override bool SourceColumnNullMapping { get; set; } = false;

        /// <summary>
        /// Gets or sets the <see cref="DataRowVersion"/> to use when you load <see cref="Value"/>
        /// </summary>
        public override DataRowVersion SourceVersion { get; set; } = DataRowVersion.Current;

        object _value;
        /// <summary>
        /// Gets or sets the value of the parameter
        /// </summary>
        public override object Value
        {
            get => _value;
            set
            {
                _value = value;
                DbType = ParseDbType(_value);
                Size = GetSize(DbType, _value);
            }
        }
        /// <summary>
        /// Resets the DbType property to its original settings
        /// </summary>
        public override void ResetDbType()
        {
            DbType = DbType.Object;
        }

        /// <summary>
        /// Get size of the primitive type
        /// </summary>
        /// <param name="type">Primitive type supported by <see cref="OneSQueryResult"/></param>
        /// <param name="value">Object for the <see cref="string"/> data type</param>
        /// <returns>Size of the type</returns>
        static int GetSize(DbType type, object value = null)
        {
            switch (type)
            {
                case DbType.String:
                    return ((string) value)?.Length ?? 0;
                case DbType.DateTime:
                    return 8;
                case DbType.Decimal:
                    return sizeof(decimal);
                case DbType.Boolean:
                    return sizeof(bool);
                case DbType.Int32:
                    return sizeof(int);
                default:
                    throw new SystemException("Unknown data type");
            }
        }

        /// <summary>
        /// Parse <see cref="System.Data.DbType"/> from <see cref="TypeCode"/>
        /// </summary>
        /// <param name="typeCode">Source <see cref="TypeCode"/></param>
        /// <returns>Associated <see cref="System.Data.DbType"/></returns>
        static DbType ParseDbType(TypeCode typeCode)
        {
            switch (typeCode)
            {                
                case TypeCode.String:
                    return DbType.String;
                case TypeCode.DateTime:
                    return DbType.DateTime;
                case TypeCode.Decimal:
                    return DbType.Decimal;
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Int32:
                    return DbType.Int32;
                default:
                    throw new SystemException("Value is of unknown data type");
            }
        }

        /// <summary>
        /// Get associated <see cref="System.Data.DbType"/> from the object
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <returns>Associated <see cref="System.Data.DbType"/></returns>
        static DbType ParseDbType(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return ParseDbType(Type.GetTypeCode(obj.GetType()));
        }
    }
}

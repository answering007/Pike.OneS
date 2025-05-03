using System;
using System.Collections.Generic;
using System.Data;

namespace Pike.OneS.Data
{
    /// <summary>
    /// Ones extension methods
    /// </summary>
    public static class OneSDbHelper
    {
        static readonly DbType[] SupportedDbTypes = { DbType.String, DbType.DateTime, DbType.Decimal, DbType.Boolean, DbType.Int32 };

        /// <summary>
        /// Get the OneS primitive types supported by the <see cref="OneSQueryResult"/>
        /// </summary>
        public static IEnumerable<DbType> SupportedTypes => SupportedDbTypes;

        /// <summary>
        /// Get size of the primitive type
        /// </summary>
        /// <param name="type">Primitive type supported by <see cref="OneSQueryResult"/></param>
        /// <param name="value">Object for the <see cref="string"/> data type</param>
        /// <returns>Size of the type</returns>
        public static int GetSize(this DbType type, object value = null)
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
        /// Parse <see cref="DbType"/> from <see cref="TypeCode"/>
        /// </summary>
        /// <param name="typeCode">Source <see cref="TypeCode"/></param>
        /// <returns>Associated <see cref="DbType"/></returns>
        public static DbType ParseDbType(this TypeCode typeCode)
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
        /// Get associated <see cref="DbType"/> from <see cref="Type"/>
        /// </summary>
        /// <param name="type">Source type</param>
        /// <returns>Associated <see cref="DbType"/></returns>
        public static DbType ParseDbType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return Type.GetTypeCode(type).ParseDbType();
        }

        /// <summary>
        /// Get associated <see cref="DbType"/> from the object
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <returns>Associated <see cref="DbType"/></returns>
        public static DbType ParseDbType(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.GetType().ParseDbType();
        }
    }
}

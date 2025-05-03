using System;
using System.Runtime.InteropServices;

namespace Pike.OneS
{
    /// <summary>
    /// 1C connector
    /// </summary>
    public class OneSConnector : OneSBaseComObject
    {
        const string DefaultProgId = "V83.ComConnector";
        dynamic _comConnector;

        /// <summary>
        /// Create an instance of <see cref="OneSConnector"/> with the specific program identifier (ProgId)
        /// </summary>
        /// <param name="progId">Program identifier</param>
        public OneSConnector(string progId = DefaultProgId)
        {
            if (string.IsNullOrWhiteSpace(progId)) throw new ArgumentException("progId can't be null or empty");

            var comType = Type.GetTypeFromProgID(progId, true);
            _comConnector = Activator.CreateInstance(comType);
        }

        /// <summary>
        /// Opens a database connection with the property settings specified by the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">A string used to open a 1C database</param>
        public void Connect(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("connectionString can't be null or empty");
            
            ComObject = _comConnector.Connect(connectionString);
        }

        /// <summary>
        /// Opens a database connection with the property settings specified by the <paramref name="builder"/>
        /// </summary>
        /// <param name="builder">Connection string builder</param>
        public void Connect(OneSConnectionStringBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            Connect(builder.ConnectionString);
        }

        /// <summary>
        /// True if connected; otherwise false
        /// </summary>
        public bool IsConnected => ComObject != null;

        /// <summary>
        /// Set the maximum number of simultaneous objects of the external connection through the COM-connection manager.
        /// The number of simultaneous connections include the number of connections in the external connections pool. The default value of 0 (not limited).
        /// Sets a property only when the object was created in Multi-threaded Apartment (MTA)
        /// </summary>
        public int MaxConnections { set { _comConnector.MaxConnections = value; }}

        /// <summary>
        /// Set the maximum number of connections with the information base which may simultaneously be in pool
        /// </summary>
        public int PoolCapacity { set { _comConnector.PoolCapacity = value; } }

        /// <summary>
        /// Set the maximum time to stay connection unused. After this time the unused connection is released.
        /// </summary>
        public TimeSpan PoolTimeout { set { _comConnector.PoolTimeout = value.TotalSeconds; } }

        /// <summary>
        /// Implementation of <see cref="IDisposable"/> interface
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (_comConnector == null) return;
            Marshal.FinalReleaseComObject(_comConnector);
            _comConnector = null;
        }

        /// <summary>
        /// Return <see cref="OneSValueTable"/> with tables structures, indexes, database fields in terms of 1C
        /// </summary>
        /// <param name="array">Array of metadata objects name or Array of metadata objects</param>
        /// <param name="isDbType">True to return <see cref="OneSValueTable"/> in terms of database; false - in terms of 1C</param>
        /// <returns>Object of type <see cref="OneSValueTable"/> with storage info</returns>
        public OneSValueTable GetDbStorageStructureInfo(OneSArray array = null, bool isDbType = false)
        {
            if (!IsConnected) throw new InvalidOperationException("Connector is not connected");

            return new OneSValueTable(ComObject.GetDBStorageStructureInfo(array?.ComObject, isDbType));
        }

        /// <summary>
        /// Create an instance of <see cref="OneSXmlWriter"/>
        /// </summary>
        /// <returns>New instance of <see cref="OneSXmlWriter"/> </returns>
        public OneSXmlWriter CreateOneSxmlWriter()
        {
            if (!IsConnected) throw new InvalidOperationException("Connector is not connected");

            return new OneSXmlWriter(ComObject.NewObject(OneSXmlWriter.Name));
        }

        /// <summary>
        /// Serialize 1C COM object as XML string
        /// </summary>
        /// <param name="comObject">Object to serialize</param>
        /// <returns>XML string</returns>
        internal string SerializeToXml(dynamic comObject)
        {
            if (comObject == null) throw new ArgumentNullException(nameof(comObject));
            if (!Marshal.IsComObject(comObject)) throw new ArgumentException("comObject must be a 1C COM object");

            var comXmlWriter = ComObject.NewObject("XMLWriter");
            comXmlWriter.SetString();
            var comSerializer = ComObject.XDTOSerializer;
            comSerializer.WriteXML(comXmlWriter, comObject);
            var rst = comXmlWriter.Close();

            Marshal.FinalReleaseComObject(comSerializer);
            Marshal.FinalReleaseComObject(comXmlWriter);

            return rst;
        }

        /// <summary>
        /// Serialize <see cref="OneSValueTable"/> as XML string
        /// </summary>
        /// <param name="table">Table to serialize</param>
        /// <returns>XML string</returns>
        public string SerializeToXml(OneSValueTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            return SerializeToXml(table.ComObject);
        }
    }
}

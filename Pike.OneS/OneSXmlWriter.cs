using System;

namespace Pike.OneS
{
    /// <summary>
    /// 1C class to organize sequential recording of documents and XML fragments
    /// </summary>
    public class OneSXmlWriter : OneSBaseComObject
    {
        /// <summary>
        /// Name of the 1C object
        /// </summary>
        public const string Name = "XMLWriter";

        /// <summary>
        /// Create an instance of <see cref="OneSXmlWriter"/>
        /// </summary>
        /// <param name="comOneSxmlWriter">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        internal OneSXmlWriter(dynamic comOneSxmlWriter)
        {
            if (comOneSxmlWriter == null) throw new ArgumentNullException(nameof(comOneSxmlWriter));

            ComObject = comOneSxmlWriter;
        }

        /// <summary>
        /// Initializes an object to output the resulting XML to a string
        /// </summary>
        public void SetString()
        {
            if (ComObject == null) throw new ObjectDisposedException("OneSxmlWriter");

            ComObject.SetString();
        }

        /// <summary>
        /// Finalize writing XML text
        /// </summary>
        /// <returns>XML as text</returns>
        public string Close()
        {
            if (ComObject == null) throw new ObjectDisposedException("OneSxmlWriter");

            return ComObject.Close();
        }
    }
}

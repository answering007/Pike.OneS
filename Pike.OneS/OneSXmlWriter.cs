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
        /// <param name="comOneSXmlWriter">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        internal OneSXmlWriter(dynamic comOneSXmlWriter)
        {
            if (comOneSXmlWriter == null) throw new ArgumentNullException(nameof(comOneSXmlWriter));

            ComObject = comOneSXmlWriter;
        }
    }
}

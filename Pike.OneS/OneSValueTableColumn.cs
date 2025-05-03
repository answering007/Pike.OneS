using System;

namespace Pike.OneS
{
    /// <summary>
    /// Represent column of the <see cref="OneSValueTable"/>
    /// </summary>
    public class OneSValueTableColumn: OneSBaseComObject
    {
        /// <summary>
        /// Create an instance of <see cref="OneSValueTableColumn"/>
        /// </summary>
        /// <param name="comValueTableColumn">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        internal OneSValueTableColumn(dynamic comValueTableColumn)
        {
            if (comValueTableColumn == null) throw new ArgumentNullException(nameof(comValueTableColumn));

            ComObject = comValueTableColumn;
        }

        /// <summary>
        /// Get or set column title
        /// </summary>
        public string Title
        {
            get { return ComObject.Title; }
            set { ComObject.Title = value; }
        }

        /// <summary>
        /// Get or set column name
        /// </summary>
        public string Name
        {
            get { return ComObject.Name; }
            set { ComObject.Name = value; }
        }

        /// <summary>
        /// Get or set column width
        /// </summary>
        public int Width
        {
            get { return ComObject.Width; }
            set { ComObject.Width = value; }
        }
    }
}

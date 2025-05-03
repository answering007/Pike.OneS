namespace Pike.OneS
{
    /// <summary>
    /// Stores the value and associated column of the specified <see cref="OneSQueryResult"/>
    /// </summary>
    public class OneSQueryResultValue
    {
        /// <summary>
        /// Column
        /// </summary>
        public OneSQueryResultColumn Column { get; internal set; }

        /// <summary>
        /// Value
        /// </summary>
        public object Value { get; internal set; }
    }
}

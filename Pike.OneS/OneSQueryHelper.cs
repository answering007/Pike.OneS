using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pike.OneS
{
    /// <summary>
    /// Contains common extension methods
    /// </summary>
    public static class OneSHelper
    {
        /// <summary>
        /// Start date <see cref="OneSQuery"/> parameter
        /// </summary>
        public const string DateBeginParameterName = "&Начало";
        /// <summary>
        /// End date <see cref="OneSQuery"/> parameter
        /// </summary>
        public const string DateEndParameterName = "&Окончание";

        /// <summary>
        /// Execute query and returns <see cref="DataTable"/> as a result
        /// </summary>
        /// <param name="connector">Query connector. Should be connected</param>
        /// <param name="queryText">Query text</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Data table</returns>
        public static DataTable QueryResult(this OneSConnector connector, string queryText,
            params KeyValuePair<string, object>[] parameters)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (string.IsNullOrWhiteSpace(queryText)) throw new ArgumentException("queryText can't be null or empty");
            if (!connector.IsConnected) throw new ArgumentException("Connector should be connected");

            DataTable dataTable;
            using (var query = new OneSQuery(connector))
            {
                query.Text = queryText;
                foreach (var pair in parameters)
                    query.SetParameter(pair.Key, pair.Value);
                using (var queryResult = query.Execute())
                    dataTable = queryResult.ToDataTable();
            }
            return dataTable;
        }

        /// <summary>
        /// Execute date range query and returns <see cref="DataTable"/> as a result
        /// </summary>
        /// <param name="connector">Query connector. Should be connected</param>
        /// <param name="queryText">Query text. Should contains <see cref="DateBeginParameterName"/> as <see cref="DateEndParameterName"/></param>
        /// <param name="startDateTime">Query start date. Associated with <see cref="DateBeginParameterName"/> in query</param>
        /// <param name="endDateTime">Query end date. Associated with <see cref="DateEndParameterName"/> in query</param>
        /// <returns>Data table</returns>
        public static DataTable QueryDateRangeResult(this OneSConnector connector, string queryText, DateTime startDateTime,
            DateTime endDateTime)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (!connector.IsConnected) throw new ArgumentException("Connector should be connected");
            if (string.IsNullOrWhiteSpace(queryText)) throw new ArgumentException("queryText can't be null or empty");
            if (!queryText.Contains(DateBeginParameterName)) throw new ArgumentException("queryText doesn't contain Begin date mark");
            if (!queryText.Contains(DateEndParameterName)) throw new ArgumentException("queryText doesn't contain End date mark");
            if (endDateTime <= startDateTime) throw new ArgumentException("endDateTime <= startDateTime");

            var rst = connector.QueryResult(queryText,
                new KeyValuePair<string, object>(DateBeginParameterName.Remove(0, 1), startDateTime),
                new KeyValuePair<string, object>(DateEndParameterName.Remove(0, 1), endDateTime));
            return rst;
        }

        /// <summary>
        /// Execute date range query and returns <see cref="DataTable"/> as a result. Initial query date range is splitted by <paramref name="queryDaysPeriod"/>.
        /// </summary>
        /// <param name="connector">Query connector. Should be connected</param>
        /// <param name="queryText">Query text. Should contains <see cref="DateBeginParameterName"/> as <see cref="DateEndParameterName"/></param>
        /// <param name="startDateTime">Query start date. Associated with <see cref="DateBeginParameterName"/> in query</param>
        /// <param name="endDateTime">Query end date. Associated with <see cref="DateEndParameterName"/> in query</param>
        /// <param name="queryDaysPeriod"> Maximum number of days in splitted parts</param>
        /// <returns>Data table</returns>
        public static DataTable QueryDateRangeResult(this OneSConnector connector, string queryText,
            DateTime startDateTime,
            DateTime endDateTime, int queryDaysPeriod)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (!connector.IsConnected) throw new ArgumentException("Connector should be connected");
            if (string.IsNullOrWhiteSpace(queryText)) throw new ArgumentException("queryText can't be null or empty");
            if (!queryText.Contains(DateBeginParameterName)) throw new ArgumentException("queryText doesn't contain Begin date mark");
            if (!queryText.Contains(DateEndParameterName)) throw new ArgumentException("queryText doesn't contain End date mark");
            if (endDateTime <= startDateTime) throw new ArgumentException("endDateTime <= startDateTime");
            if (queryDaysPeriod <= 0) throw new ArgumentException("queryDaysPeriod <= 0");

            var tuple = GetPeriods(startDateTime, endDateTime, TimeSpan.FromDays(queryDaysPeriod));
            var first = tuple[0];
            var rest = tuple.Skip(1);

            var rst = connector.QueryDateRangeResult(queryText, first.Item1, first.Item2);
            foreach (var period in rest)
            {
                var periodResult = connector.QueryDateRangeResult(queryText, period.Item1, period.Item2);
                rst.Merge(periodResult);
            }
            return rst;
        }

        /// <summary>
        /// Splits initial date range to <paramref name="splitPeriod"/>
        /// </summary>
        /// <param name="startDateTime">Start date</param>
        /// <param name="endDateTime">End date</param>
        /// <param name="splitPeriod">Split part</param>
        /// <returns>Array of start-end tuple</returns>
        public static Tuple<DateTime, DateTime>[] GetPeriods(DateTime startDateTime, DateTime endDateTime, TimeSpan splitPeriod)
        {
            if (endDateTime <= startDateTime) throw new ArgumentException("endDateTime <= startDateTime");
            
            var totalPeriod = (endDateTime - startDateTime).Ticks;
            var periods = (totalPeriod % splitPeriod.Ticks == 0)
                ? totalPeriod / splitPeriod.Ticks
                : totalPeriod / splitPeriod.Ticks + 1;

            var rst = new Tuple<DateTime, DateTime>[periods];
            var currentStart = startDateTime;
            for (var i = 0; i < periods; i++)
            {
                var currentEnd = (currentStart.Add(splitPeriod) > endDateTime)
                    ? endDateTime
                    : currentStart.Add(splitPeriod);
                rst[i] = new Tuple<DateTime, DateTime>(currentStart, currentEnd);
                currentStart = currentEnd;
            }
            return rst.ToArray();
        }
    }
}

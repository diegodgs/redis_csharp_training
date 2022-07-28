using NRedisTimeSeries.DataTypes;
using StackExchange.Redis;
using NRedisTimeSeries.Commands.Enums;
using Fynance.Result;



namespace NRedisTimeSeries.RedisTimeSeriesAdapter
{
    /// <summary>
    /// Examples for NRedisTimeSeries API for adding new sample to time series.
    /// </summary>
    internal class RedisTimeSeries
    {

        private static void AddByQuoteType(string symbol, string quote_type, FyQuote[] quotes)
        {
            var time_series = new List<(string, TimeStamp, double)>();
            string ts_complete_name = $"{symbol}:{quote_type}";

            RedisTimeSeries.CreateTimeSeries(ts_complete_name, symbol, quote_type);

            foreach (var item in quotes)
            {
                long unixTime = (long)item.Period.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                TimeStamp ts = new TimeStamp(unixTime);

                Decimal value = (Decimal)(item.GetType().GetProperty(quote_type)!.GetValue(item)!);

                time_series.Add(
                    (
                        ts_complete_name,
                        ts,
                        Decimal.ToDouble(value)
                    )
                );
            }

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            db.TimeSeriesMAdd(time_series);
            redis.Close();
        }

        /// <summary>
        /// Add Quote Historical data
        /// </summary>
        /// <param name="symbol">Quote symbol</param>
        /// <param name="quotes">Historical data</param>
        public static void AddQuoteHistoricalData(string symbol, FyQuote[] quotes)
        {
            string[] quote_types = {
                "Low", "Close", "Open", "High", "AdjClose", "Volume"
            };

            foreach (var quote_type in quote_types)
            {
                RedisTimeSeries.AddByQuoteType(symbol, quote_type, quotes);
            }

        }

        /// <summary>
        /// Delete all Redis Database
        /// </summary>
        public static void DeleteAllDatabase()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();
            db.Execute("FLUSHALL");
            redis.Close();
        }

        /// <summary>
        /// Create a new TimeSeries on Redis Database
        /// </summary>
        /// <param name="ts_complete_name"> A valid Redis key name</param>
        /// <param name="symbol">Quote Symbol</param>
        /// <param name="quote_type">Quote Type</param>
        public static void CreateTimeSeries(string ts_complete_name, string symbol, string quote_type)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            var labels = new List<TimeSeriesLabel> {
                new TimeSeriesLabel("symbol", symbol),
                new TimeSeriesLabel("type", quote_type),
            };

            db.TimeSeriesCreate(
                ts_complete_name,
                labels: labels,
                duplicatePolicy: TsDuplicatePolicy.LAST
            );

            redis.Close();
        }
    }
}

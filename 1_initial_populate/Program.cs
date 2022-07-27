using Fynance;
using NRedisTimeSeries.RedisTimeSeriesAdapter;
using Microsoft.Extensions.Configuration;

// .Net 6.0 Format

// Remove all data from Time Series Database
RedisTimeSeries.DeleteAllDatabase();

// Load app config json file to load valid Tickers
IConfigurationRoot _config = new ConfigurationBuilder()
                                    .AddJsonFile("./appsettings.json", false, true).Build();

List<string> tickers = _config.GetSection("tickers").Get<List<string>>();

// Get Market data from Yahoo Finance and store on redistimeseries
foreach(var item in tickers){

    var result = Ticker.Build()
                            .SetSymbol(item)
                            .SetPeriod(Period.TenYears)
                            .SetInterval(Interval.OneDay)
                            .Get();

    RedisTimeSeries.AddQuoteHistoricalData(result.Symbol, result.Quotes);
}
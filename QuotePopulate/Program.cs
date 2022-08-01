using Fynance;
using NRedisTimeSeries.RedisTimeSeriesAdapter;
using Microsoft.Extensions.Configuration;

// .Net 6.0 Format

// Load app config json file to load valid Tickers
IConfigurationRoot _config = new ConfigurationBuilder()
                                    .AddJsonFile("./appsettings.json", false, true).Build();

List<string> tickers = _config.GetSection("Tickers").Get<List<string>>();
string redisHost= _config.GetValue<string>("RedisHost"); // "Information"

var redisAdapter = new RedisTimeSeries(redisHost);

// Remove all data from Time Series Database
redisAdapter.DeleteAllDatabase();

// Get Market data from Yahoo Finance and store on redistimeseries
foreach(var item in tickers){

    var result = Ticker.Build()
                            .SetSymbol(item)
                            .SetPeriod(Period.TenYears)
                            .SetInterval(Interval.OneDay)
                            .Get();

    redisAdapter.AddQuoteHistoricalData(result.Symbol, result.Quotes);
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;  

namespace NRedisTimeSeries.RedisTimeSeriesAdapter.Tests
{
    [TestClass]
    public class TimeSeriesAdapter
    {
        private readonly RedisTimeSeries _adapter;

        public TimeSeriesAdapter()
        {
            _adapter = new RedisTimeSeries("localhost");
        }

        [TestMethod]
        public void CallBlaBlaBla()
        {
            var mock = new Mock<RedisTimeSeries>();

 
            Assert.IsFalse(false, "Is True");
        }
    }
}

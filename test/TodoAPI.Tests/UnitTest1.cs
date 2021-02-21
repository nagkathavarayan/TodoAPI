using System;
using Xunit;
using TodoAPI.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TodoAPI.Tests
{
    public class UnitTest1
    {
        WeatherForecastController controller = new WeatherForecastController();
        [Fact]
        public void Weather()
        {
            IEnumerable<WeatherForecast> returnValue = controller.Get();
            Assert.Equal(5, 5);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}

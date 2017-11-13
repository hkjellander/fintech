using Archive.Api.Controllers;
using Archive.Api.Models;
using Archive.IntegrationTests.Util;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Archive.IntegrationTests
{
    public class LogControllerIntegrationTest
    {
        [Fact]
        public void GetAllReturnsTwoObjects()
        {
            // Arrange
            var mockContext = new Mock<LogContext>();
            mockContext.Setup(db => db.Logs).Returns(MockDbSet(GetTestData()));
            ILogger<LogController> logger = LogUtil<LogController>.GetLogger();
            var controller = new LogController(mockContext.Object, logger);

            // Act
            var logs = controller.GetAll();

            // Assert
            Assert.Equal(2, logs.Count());
            var logsList = logs.ToList();
            Assert.Equal(0, logsList[0].Id);
            Assert.Equal(1, logsList[1].Id);
        }

        private IQueryable<LogEntry> GetTestData()
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data["foo"] = "bar";
            var entries = new List<LogEntry>();
            entries.Add(new LogEntry()
            {
                Id = 0,
                Json = JsonConvert.SerializeObject(data),
            });
            data["foo"] = "baz";
            entries.Add(new LogEntry()
            {
                Id = 1,
                Json = JsonConvert.SerializeObject(data),
            });
            return entries.AsQueryable();
        }

        // Helper method for easier mocking of DbSet.
        private static DbSet<T> MockDbSet<T>(IEnumerable<T> list) where T : class, new()
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(queryableList.GetEnumerator());
            return dbSetMock.Object;
        }
    }
}

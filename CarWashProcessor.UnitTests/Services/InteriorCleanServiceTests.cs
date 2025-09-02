using CarWashProcessor.Models;
using CarWashProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Immutable;

namespace CarWashProcessor.UnitTests.Services
{
    [TestClass]
    public class InteriorCleanServiceTests
    {
        private Mock<ILogger<InteriorCleanService>>? _loggerMock = null;
        private InteriorCleanService? _washService = null;
        private CarJob? _carJob = null;

        [TestInitialize]
        public void TestInit()
        {
            _loggerMock = new Mock<ILogger<InteriorCleanService>>();
            _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, new ImmutableArray<EServiceAddon>());
        }

        [TestMethod]
        public void Ctor_WhenLoggerIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _washService = new InteriorCleanService(null!));
            Assert.AreEqual("logger", exception.ParamName);
        }

        [TestMethod]
        public void Ctor_WhenAllArgumentsProvided_Succeeds()
        {
            _washService = new InteriorCleanService(_loggerMock!.Object);
            Assert.IsTrue(_washService is not null);
        }

        [TestMethod]
        public async Task CleanInteriorAsync_WhenCarJobIsNull_ThrowsArgumentNullException()
        {
            _washService = new InteriorCleanService(_loggerMock!.Object);
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _washService.CleanInteriorAsync(null!));
            Assert.AreEqual("carJob", exception.ParamName);
        }

        [TestMethod]
        public async Task CleanInteriorAsync_WhenCalled_PerformsWashAndLogs()
        {
            // Arrange
            _washService = new InteriorCleanService(_loggerMock!.Object);

            // Act
            await _washService.CleanInteriorAsync(_carJob!);

            // Assert
            // TODO: This approach could be modified by creating a custom ILogger implementation that records logs to a list and asserting against that.
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Interior has been cleaned for customer " + _carJob!.CustomerId)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}

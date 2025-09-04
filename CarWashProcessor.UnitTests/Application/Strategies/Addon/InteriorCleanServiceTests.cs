// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

using CarWashProcessor.Application.Strategies.Addons;
using CarWashProcessor.Models;

using Microsoft.Extensions.Logging;

using Moq;

namespace CarWashProcessor.UnitTests.Application.Strategies.Addon
{
    [TestClass]
    public class InteriorCleanServiceTests
    {
        private Mock<ILogger<InteriorCleanService>>? _loggerMock = null;
        private InteriorCleanService? _addonService = null;
        private CarJob? _carJob = null;

        [TestInitialize]
        public void TestInit()
        {
            _loggerMock = new Mock<ILogger<InteriorCleanService>>();
            _loggerMock.Setup(m => m.IsEnabled(LogLevel.Information)).Returns(true);
            _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, []);
        }

        [TestMethod]
        public void Ctor_WhenLoggerIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _addonService = new InteriorCleanService(null!));
            Assert.AreEqual("logger", exception.ParamName);
        }

        [TestMethod]
        public void Ctor_WhenAllArgumentsProvided_Succeeds()
        {
            _addonService = new InteriorCleanService(_loggerMock!.Object);
            Assert.IsTrue(_addonService is not null);
        }

        [TestMethod]
        public async Task PerformAddonAsync_WhenCarJobIsNull_ThrowsArgumentNullException()
        {
            _addonService = new InteriorCleanService(_loggerMock!.Object);
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _addonService.PerformAddonAsync(null!));
            Assert.AreEqual("carJob", exception.ParamName);
        }

        [TestMethod]
        public async Task PerformAddonAsync_WhenCalled_PerformsWashAndLogs()
        {
            // Arrange
            _addonService = new InteriorCleanService(_loggerMock!.Object);

            // Act
            await _addonService.PerformAddonAsync(_carJob!);

            // Assert
            // TODO: This approach could be modified by creating a custom ILogger implementation that records logs to a list and asserting against that.
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString()!.Contains("InteriorClean") &&
                        v.ToString()!.Contains(_carJob!.CustomerId.ToString())),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}

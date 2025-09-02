using CarWashProcessor.Models;
using CarWashProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Immutable;

namespace CarWashProcessor.UnitTests.Services
{
    [TestClass]
    public class HandWaxAndShineServiceTests
    {
        private Mock<ILogger<HandWaxAndShineService>>? _loggerMock = null;
        private HandWaxAndShineService? _washService = null;
        private CarJob? _carJob = null;

        [TestInitialize]
        public void TestInit()
        {
            _loggerMock = new Mock<ILogger<HandWaxAndShineService>>();
            _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, new ImmutableArray<EServiceAddon>());
        }

        [TestMethod]
        public void Ctor_WhenAllArgumentsProvided_Succeeds()
        {
            _washService = new HandWaxAndShineService(_loggerMock!.Object);
            Assert.IsTrue(_washService is not null);
        }

        [TestMethod]
        public async Task HandWaxAndShineAsync_WhenCalled_PerformsWashAndLogs()
        {
            // Arrange
            _washService = new HandWaxAndShineService(_loggerMock!.Object);

            // Act
            await _washService.HandWaxAndShineAsync(_carJob!);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Hand waxed and shined for customer")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}

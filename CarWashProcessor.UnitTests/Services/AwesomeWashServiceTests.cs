// TODO: Consistent file header containing copyright.

using CarWashProcessor.Models;
using CarWashProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Immutable;

namespace CarWashProcessor.UnitTests.Services;

[TestClass]
public class AwesomeWashServiceTests
{
    private Mock<ILogger<AwesomeWashService>>? _loggerMock = null;
    private AwesomeWashService? _washService = null;
    private CarJob? _carJob = null;

    [TestInitialize]
    public void TestInit()
    {
        _loggerMock = new Mock<ILogger<AwesomeWashService>>();
        _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, new ImmutableArray<EServiceAddon>());
    }

    [TestMethod]
    public void Ctor_WhenAllArgumentsProvided_Succeeds()
    {
        _washService = new AwesomeWashService(_loggerMock!.Object);
        Assert.IsTrue(_washService is not null);
    }

    [TestMethod]
    public async Task DoAwesomeWashAsync_WhenCalled_PerformsWashAndLogs()
    {
        // Arrange
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Act
        await _washService.DoAwesomeWashAsync(_carJob!);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Awesome wash performed for customer")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

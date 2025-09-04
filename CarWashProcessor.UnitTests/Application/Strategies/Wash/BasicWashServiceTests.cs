// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

using CarWashProcessor.Application.Strategies.Wash;
using CarWashProcessor.Models;

using Microsoft.Extensions.Logging;

using Moq;

using System.Collections.Immutable;

namespace CarWashProcessor.UnitTests.Application.Strategies.Wash;

[TestClass]
public class BasicWashServiceTests
{
    private Mock<ILogger<BasicWashService>>? _loggerMock = null;
    private BasicWashService? _washService = null;
    private CarJob? _carJob = null;

    [TestInitialize]
    public void TestInit()
    {
        _loggerMock = new Mock<ILogger<BasicWashService>>();
        _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, []);
    }

    [TestMethod]
    public void Ctor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsException<ArgumentNullException>(() => _washService = new BasicWashService(null!));
        Assert.AreEqual("logger", exception.ParamName);
    }

    [TestMethod]
    public void Ctor_WhenAllArgumentsProvided_Succeeds()
    {
        _washService = new BasicWashService(_loggerMock!.Object);
        Assert.IsTrue(_washService is not null);
    }

    [TestMethod]
    public async Task PerformWashAsync_WhenCarJobIsNull_ThrowsArgumentNullException()
    {
        _washService = new BasicWashService(_loggerMock!.Object);
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _washService.PerformWashAsync(null!));
        Assert.AreEqual("carJob", exception.ParamName);
    }

    [TestMethod]
    public async Task PerformWashAsync_WhenCalled_PerformsWashAndLogs()
    {
        // Arrange
        _washService = new BasicWashService(_loggerMock!.Object);

        // Act
        await _washService.PerformWashAsync(_carJob!);

        // Assert
        // TODO: This approach could be modified by creating a custom ILogger implementation that records logs to a list and asserting against that.
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Basic wash performed for customer " + _carJob!.CustomerId)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

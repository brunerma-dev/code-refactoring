// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

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
        // Common setup
        _loggerMock = new Mock<ILogger<AwesomeWashService>>();
        _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, new ImmutableArray<EServiceAddon>());
    }

    [TestMethod]
    public void Ctor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.ThrowsException<ArgumentNullException>(() => _washService = new AwesomeWashService(null!));
        Assert.AreEqual("logger", exception.ParamName);
    }

    [TestMethod]
    public void Ctor_WhenAllArgumentsProvided_Succeeds()
    {
        // Act
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Assert
        Assert.IsTrue(_washService is not null);
    }

    [TestMethod]
    public void Key_Property_ReturnsExpectedValue()
    {
        // Act
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Assert
        Assert.AreEqual(EServiceWash.Awesome, _washService.Key);
    }

    [TestMethod]
    public async Task DoAwesomeWashAsync_WhenCarJobIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _washService.DoAwesomeWashAsync(null!));

        // Assert
        Assert.AreEqual("carJob", exception.ParamName);
    }

    [TestMethod]
    public async Task DoAwesomeWashAsync_WhenCalled_PerformsWashAndLogs()
    {
        // Arrange
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Act
        await _washService.DoAwesomeWashAsync(_carJob!);

        // Assert
        // TODO: This approach could be modified by creating a custom ILogger implementation that records logs to a list and asserting against that.
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Awesome wash performed for customer " + _carJob!.CustomerId)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [TestMethod]
    public async Task PerformWashAsync_WhenCarJobIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _washService!.PerformWashAsync(null!));

        // Assert
        Assert.AreEqual("carJob", exception.ParamName);
    }

    [TestMethod]
    public async Task PerformWashAsync_WhenCalled_PerformsWashAndLogs()
    {
        // Arrange
        _washService = new AwesomeWashService(_loggerMock!.Object);

        // Act
        await _washService.PerformWashAsync(_carJob!);

        // Assert
        // TODO: This approach could be modified by creating a custom ILogger implementation that records logs to a list and asserting against that.
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Awesome wash performed for customer " + _carJob!.CustomerId)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

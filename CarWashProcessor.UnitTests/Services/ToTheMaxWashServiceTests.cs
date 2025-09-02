using CarWashProcessor.Models;
using CarWashProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Immutable;

namespace CarWashProcessor.UnitTests.Services;

[TestClass]
public class ToTheMaxWashServiceTests
{
    private Mock<ILogger<ToTheMaxWashService>>? _loggerMock = null;
    private ToTheMaxWashService? _washService = null;
    private CarJob? _carJob = null;

    [TestInitialize]
    public void TestInit()
    {
        _loggerMock = new Mock<ILogger<ToTheMaxWashService>>();
        _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, new ImmutableArray<EServiceAddon>());
    }

    [TestMethod]
    public void Ctor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.ThrowsException<ArgumentNullException>(() => _washService = new ToTheMaxWashService(null!));
        Assert.AreEqual("logger", exception.ParamName);
    }

    [TestMethod]
    public void Ctor_WhenAllArgumentsProvided_Succeeds()
    {
        _washService = new ToTheMaxWashService(_loggerMock!.Object);
        Assert.IsTrue(_washService is not null);
    }

    [TestMethod]
    public async Task DoAwesomeWashAsync_c_WhenCarJobIsNull_ThrowsArgumentNullException()
    {
        _washService = new ToTheMaxWashService(_loggerMock!.Object);
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _washService.DoToTheMaxWashAsync(null!));
        Assert.AreEqual("carJob", exception.ParamName);
    }

    [TestMethod]
    public async Task DoAwesomeWashAsync_WhenCalled_PerformsWashAndLogs()
    {
        // Arrange
        _washService = new ToTheMaxWashService(_loggerMock!.Object);

        // Act
        await _washService.DoToTheMaxWashAsync(_carJob!);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> To The Max wash performed for customer")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

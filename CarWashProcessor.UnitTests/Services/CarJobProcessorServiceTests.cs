using CarWashProcessor.Models;
using CarWashProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Immutable;

namespace CarWashProcessor.UnitTests.Services
{
    [TestClass]
    public class CarJobProcessorServiceTests
    {
        /* BE ADVISED:
         * This entire test class will simplify as classes are refactored to use interfaces instead
         * of concrete implementations. You shouldn't have to mock nested dependencies, only those 
         * that are injected. As refactoring continues, these will be simplified over progressive 
         * pull requests.
         */
        private Mock<BasicWashService>? _basicWashServiceMock;
        private Mock<ILogger<BasicWashService>>? _basicWashServiceLoggerMock;
        private Mock<AwesomeWashService>? _awesomeWashServiceMock;
        private Mock<ILogger<AwesomeWashService>>? _awesomeWashServiceLoggerMock;
        private Mock<ToTheMaxWashService>? _toTheMaxWashServiceMock;
        private Mock<ILogger<ToTheMaxWashService>>? _toTheMaxWashServiceLoggerMock;
        private Mock<TireShineService>? _tireShineServiceMock;
        private Mock<ILogger<TireShineService>>? _tireShineServiceLoggerMock;
        private Mock<InteriorCleanService>? _interiorCleanServiceMock;
        private Mock<ILogger<InteriorCleanService>>? _interiorCleanServiceLoggerMock;
        private Mock<HandWaxAndShineService>? _handWashAndShineServiceMock;
        private Mock<ILogger<HandWaxAndShineService>>? _handWashAndShineServiceLoggerMock;
        private CarJobProcessorService? _processorService;
        private CarJob? _carJob;

        [TestInitialize]
        public void TestInit()
        {
            _basicWashServiceLoggerMock = new Mock<ILogger<BasicWashService>>();
            _basicWashServiceMock = new Mock<BasicWashService>(MockBehavior.Strict, _basicWashServiceLoggerMock.Object);
            _awesomeWashServiceLoggerMock = new Mock<ILogger<AwesomeWashService>>();
            _awesomeWashServiceMock = new Mock<AwesomeWashService>(MockBehavior.Strict, _awesomeWashServiceLoggerMock.Object);
            _toTheMaxWashServiceLoggerMock = new Mock<ILogger<ToTheMaxWashService>>();
            _toTheMaxWashServiceMock = new Mock<ToTheMaxWashService>(MockBehavior.Strict, _toTheMaxWashServiceLoggerMock.Object);
            _tireShineServiceLoggerMock = new Mock<ILogger<TireShineService>>();
            _tireShineServiceMock = new Mock<TireShineService>(MockBehavior.Strict, _tireShineServiceLoggerMock.Object);
            _interiorCleanServiceLoggerMock = new Mock<ILogger<InteriorCleanService>>();
            _interiorCleanServiceMock = new Mock<InteriorCleanService>(MockBehavior.Strict, _interiorCleanServiceLoggerMock.Object);
            _handWashAndShineServiceLoggerMock = new Mock<ILogger<HandWaxAndShineService>>();
            _handWashAndShineServiceMock = new Mock<HandWaxAndShineService>(MockBehavior.Strict, _handWashAndShineServiceLoggerMock.Object);

            _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, ImmutableArray<EServiceAddon>.Empty);
        }

        [TestMethod]
        public void Ctor_WhenAllArgumentsProvided_Succeeds()
        {
            // Act
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);

            // Assert
            Assert.IsTrue(_processorService is not null);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCalledWithBasicWash_PerformsBasicWash()
        {
            // Arrange
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, ImmutableArray<EServiceAddon>.Empty);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _basicWashServiceLoggerMock!.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Basic wash performed for customer")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCalledWithAwesomeWash_PerformsAwesomeWash()
        {
            // Arrange
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Awesome, ImmutableArray<EServiceAddon>.Empty);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _awesomeWashServiceLoggerMock!.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Awesome wash performed for customer")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCalledWithToTheMaxWash_PerformsToTheMaxWash()
        {
            // Arrange
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.ToTheMax, ImmutableArray<EServiceAddon>.Empty);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _toTheMaxWashServiceLoggerMock!.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> To The Max wash performed for customer")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCarJobServiceAddonContainsTireShine_CallsTireShineServiceAddOn()
        {
            // Arrange
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, [EServiceAddon.TireShine]);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _tireShineServiceLoggerMock!.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Tires have been shined for customer")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCarJobServiceAddonContainsInteriorClean_CallsInteriorCleanServiceAddOn()
        {
            // Arrange
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, [EServiceAddon.InteriorClean]);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _interiorCleanServiceLoggerMock!.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("--> Interior has been cleaned for customer")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCarJobServiceAddonContainsHandWaxAndShine_CallsHandWaxAndShineServiceAddOn()
        {
            // Arrange
            _processorService = new CarJobProcessorService(_basicWashServiceMock!.Object, _awesomeWashServiceMock!.Object, _toTheMaxWashServiceMock!.Object, _tireShineServiceMock!.Object, _interiorCleanServiceMock!.Object, _handWashAndShineServiceMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, [EServiceAddon.HandWaxAndShine]);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _handWashAndShineServiceLoggerMock!.Verify(
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

// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

using CarWashProcessor.Application.Abstractions.Resolution;
using CarWashProcessor.Domain.Abstractions.Services;
using CarWashProcessor.Models;
using CarWashProcessor.Services;

using Moq;

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
        private Mock<IServiceResolver<EServiceWash, IWashServiceStrategy>>? _washServiceResolverMock;
        private Mock<IWashServiceStrategy>? _washServiceStrategyMock;
        private Mock<IServiceResolver<EServiceAddon, IAddonServiceStrategy>>? _addonServiceResolverMock;
        private Mock<IAddonServiceStrategy>? _addonServiceStrategyMock;

        
        private CarJobProcessorService? _processorService;
        private CarJob? _carJob;

        [TestInitialize]
        public void TestInit()
        {
            _washServiceResolverMock = new Mock<IServiceResolver<EServiceWash, IWashServiceStrategy>>(MockBehavior.Strict);
            _washServiceStrategyMock = new Mock<IWashServiceStrategy>(MockBehavior.Strict);
            _addonServiceResolverMock = new Mock<IServiceResolver<EServiceAddon, IAddonServiceStrategy>>(MockBehavior.Strict);
            _addonServiceStrategyMock = new Mock<IAddonServiceStrategy>(MockBehavior.Strict);
            _carJob = new CarJob(8675309, ECarMake.Ford, EServiceWash.Awesome, []);
        }

        [TestMethod]
        public void Ctor_WhenAllArgumentsProvided_Succeeds()
        {
            // Act
            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);

            // Assert
            Assert.IsTrue(_processorService is not null);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCalledWithBasicWash_PerformsBasicWash()
        {
            // Arrange
            _washServiceResolverMock!.Setup(m => m.Resolve(EServiceWash.Basic)).Returns(_washServiceStrategyMock!.Object);
            _washServiceStrategyMock!.Setup(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, []);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _washServiceResolverMock.Verify(m => m.Resolve(EServiceWash.Basic), Times.Once);
            _washServiceStrategyMock!.Verify(m=> m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCalledWithAwesomeWash_PerformsAwesomeWash()
        {
            // Arrange
            _washServiceResolverMock!.Setup(m => m.Resolve(EServiceWash.Awesome)).Returns(_washServiceStrategyMock!.Object);
            _washServiceStrategyMock!.Setup(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Awesome, []);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _washServiceResolverMock.Verify(m => m.Resolve(EServiceWash.Awesome), Times.Once);
            _washServiceStrategyMock!.Verify(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCalledWithToTheMaxWash_PerformsToTheMaxWash()
        {
            // Arrange
            _washServiceResolverMock!.Setup(m => m.Resolve(EServiceWash.ToTheMax)).Returns(_washServiceStrategyMock!.Object);
            _washServiceStrategyMock!.Setup(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.ToTheMax, []);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _washServiceResolverMock.Verify(m => m.Resolve(EServiceWash.ToTheMax), Times.Once);
            _washServiceStrategyMock!.Verify(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCarJobServiceAddonContainsTireShine_CallsTireShineServiceAddOn()
        {
            // Arrange
            _washServiceResolverMock!.Setup(m => m.Resolve(It.IsAny<EServiceWash>())).Returns(_washServiceStrategyMock!.Object);
            _washServiceStrategyMock!.Setup(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            _addonServiceResolverMock!.Setup(m => m.Resolve(EServiceAddon.TireShine)).Returns(_addonServiceStrategyMock!.Object);
            _addonServiceStrategyMock!.Setup(m => m.PerformAddonAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, [EServiceAddon.TireShine]);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _washServiceResolverMock.Verify(m => m.Resolve(It.IsAny<EServiceWash>()), Times.Once);
            _washServiceStrategyMock.Verify(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);

            _addonServiceResolverMock.Verify(m => m.Resolve(EServiceAddon.TireShine), Times.Once);
            _addonServiceStrategyMock!.Verify(m => m.PerformAddonAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCarJobServiceAddonContainsInteriorClean_CallsInteriorCleanServiceAddOn()
        {
            // Arrange
            _washServiceResolverMock!.Setup(m => m.Resolve(It.IsAny<EServiceWash>())).Returns(_washServiceStrategyMock!.Object);
            _washServiceStrategyMock!.Setup(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            _addonServiceResolverMock!.Setup(m => m.Resolve(EServiceAddon.InteriorClean)).Returns(_addonServiceStrategyMock!.Object);
            _addonServiceStrategyMock!.Setup(m => m.PerformAddonAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, [EServiceAddon.InteriorClean]);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _washServiceResolverMock.Verify(m => m.Resolve(It.IsAny<EServiceWash>()), Times.Once);
            _washServiceStrategyMock.Verify(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);

            _addonServiceResolverMock.Verify(m => m.Resolve(EServiceAddon.InteriorClean), Times.Once);
            _addonServiceStrategyMock!.Verify(m => m.PerformAddonAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessCarJobAsync_WhenCarJobServiceAddonContainsHandWaxAndShine_CallsHandWaxAndShineServiceAddOn()
        {
            // Arrange
            _washServiceResolverMock!.Setup(m => m.Resolve(It.IsAny<EServiceWash>())).Returns(_washServiceStrategyMock!.Object);
            _washServiceStrategyMock!.Setup(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            _addonServiceResolverMock!.Setup(m => m.Resolve(EServiceAddon.HandWaxAndShine)).Returns(_addonServiceStrategyMock!.Object);
            _addonServiceStrategyMock!.Setup(m => m.PerformAddonAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            _processorService = new CarJobProcessorService(_washServiceResolverMock!.Object, _addonServiceResolverMock!.Object);
            var carJob = new CarJob(123456, ECarMake.Toyota, EServiceWash.Basic, [EServiceAddon.HandWaxAndShine]);

            // Act
            await _processorService.ProcessCarJobAsync(carJob);

            // Assert
            _washServiceResolverMock.Verify(m => m.Resolve(It.IsAny<EServiceWash>()), Times.Once);
            _washServiceStrategyMock.Verify(m => m.PerformWashAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);

            _addonServiceResolverMock.Verify(m => m.Resolve(EServiceAddon.HandWaxAndShine), Times.Once);
            _addonServiceStrategyMock!.Verify(m => m.PerformAddonAsync(It.IsAny<CarJob>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

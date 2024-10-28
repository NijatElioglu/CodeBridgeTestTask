using AutoMapper;
using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Create;
using CodeBridgeTestTask.Application.Logging;
using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CodeBridgeTestTask.Application.Tests.Features.Dog.Commands.Create
{
    public class DogCommandHandlerTests
    {
        private readonly Mock<IDogRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly CreateDogCommandHandler _commandHandler;

        public DogCommandHandlerTests()
        {
            _mockRepo = new Mock<IDogRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();

            _commandHandler = new CreateDogCommandHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnDogId_WhenDogIsCreatedSuccessfully()
        {
            Console.WriteLine("Running Handle_ShouldReturnDogId_WhenDogIsCreatedSuccessfully");

            var command = new CreateDogCommand
            {
                Name = "Buddy",
                TailLenght = 5,
                Weight = 20
            };

            var dog = new Dogs { Id = 1 };
            _mockMapper.Setup(m => m.Map<Dogs>(It.IsAny<CreateDogCommand>())).Returns(dog);
            _mockRepo.Setup(r => r.GetByNameAsync(command.Name)).ReturnsAsync((Dogs)null);
            _mockRepo.Setup(r => r.AddAsync(dog)).Returns(Task.CompletedTask);

            var result = await _commandHandler.Handle(command, CancellationToken.None);

            Console.WriteLine($"Dog created successfully with ID: {result}");
            Assert.Equal(dog.Id, result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Dogs>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenTailLengthIsNegative()
        {
            Console.WriteLine("Running Handle_ShouldThrowArgumentException_WhenTailLengthIsNegative");

            var command = new CreateDogCommand
            {
                Name = "Buddy",
                TailLenght = -1,
                Weight = 20
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
            _mockLogger.Verify(l => l.LogInformation("TailLength cannot be negative."), Times.Once);
            Console.WriteLine("TailLength cannot be negative exception thrown as expected.");
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenWeightIsNegative()
        {
            Console.WriteLine("Running Handle_ShouldThrowArgumentException_WhenWeightIsNegative");

            var command = new CreateDogCommand
            {
                Name = "Buddy",
                TailLenght = 5,
                Weight = -5
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
            _mockLogger.Verify(l => l.LogInformation("Weight cannot be negative."), Times.Once);
            Console.WriteLine("Weight cannot be negative exception thrown as expected.");
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenDogNameAlreadyExists()
        {
            var command = new CreateDogCommand
            {
                Name = "Buddy",
                TailLenght = 5,
                Weight = 20
            };

            var existingDog = new Dogs { Name = "Buddy" };
            _mockRepo.Setup(r => r.GetByNameAsync(command.Name)).ReturnsAsync(existingDog);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
            Assert.Equal($"'{command.Name}' Name is already exist.", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidJsonException_WhenRequestIsNull()
        {
            Console.WriteLine("Running Handle_ShouldThrowInvalidJsonException_WhenRequestIsNull");

            CreateDogCommand command = null;

            await Assert.ThrowsAsync<InvalidJsonException>(() => _commandHandler.Handle(command, CancellationToken.None));
            Console.WriteLine("Invalid JSON exception thrown as expected.");
        }
    }
}

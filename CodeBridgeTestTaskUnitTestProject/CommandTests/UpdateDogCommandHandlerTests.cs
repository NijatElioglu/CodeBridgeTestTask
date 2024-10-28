using AutoMapper;
using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Update;
using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class DogCommandHandlerTests
{
    private readonly Mock<IDogRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<UpdateDogCommandHandler>> _mockLogger;
    private readonly UpdateDogCommandHandler _commandHandler;

    public DogCommandHandlerTests()
    {
        _mockRepo = new Mock<IDogRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<UpdateDogCommandHandler>>();
        _commandHandler = new UpdateDogCommandHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidJsonException_WhenRequestIsNull()
    {
        UpdateDogCommand command = null;

        var exception = await Assert.ThrowsAsync<InvalidJsonException>(() => _commandHandler.Handle(command, CancellationToken.None));
        Assert.Equal("The request body is null or invalid.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenTailLengthIsNegative()
    {
        var command = new UpdateDogCommand { TailLength = -1 };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
        Assert.Equal("TailLength cannot be negative.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenWeightIsNegative()
    {
        var command = new UpdateDogCommand { Weight = -1 };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
        Assert.Equal("Weight cannot be negative.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenDogNotFound()
    {
        var command = new UpdateDogCommand { Id = 1, Name = "Buddy" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((Dogs)null);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
        Assert.Equal("Dog with ID 1 not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenDogNameAlreadyExists()
    {
        var command = new UpdateDogCommand { Id = 2, Name = "Buddy" };
        var existingDog = new Dogs { Id = 1, Name = "Buddy" };

        _mockRepo.Setup(repo => repo.GetByNameAsync(command.Name)).ReturnsAsync(existingDog);
        _mockRepo.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(new Dogs { Id = command.Id });

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _commandHandler.Handle(command, CancellationToken.None));
        Assert.Equal("A dog with the name 'Buddy' already exists.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldUpdateDog_WhenRequestIsValid()
    {
        var command = new UpdateDogCommand { Id = 1, Name = "Buddy", TailLength = 5, Weight = 20 };
        var existingDog = new Dogs { Id = command.Id, Name = "OldName" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(existingDog);
        _mockRepo.Setup(repo => repo.GetByNameAsync(command.Name)).ReturnsAsync((Dogs)null); 
        _mockMapper.Setup(m => m.Map(command, existingDog));

        var result = await _commandHandler.Handle(command, CancellationToken.None);

        Assert.Equal(existingDog.Id, result);
        _mockRepo.Verify(repo => repo.UpdateAsync(existingDog), Times.Once);
    }
}

using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Delete;
using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Repositories;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CodeBridgeTestTask.Application.Tests.Features.Dog.Commands.Delete
{
    public class SoftDeleteDogCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<Dogs>> _repositoryMock;
        private readonly SoftDeleteDogCommandHandler _handler;

        public SoftDeleteDogCommandHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Dogs>>();
            _handler = new SoftDeleteDogCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUnit_WhenDogIsDeletedSuccessfully()
        {
            
            var command = new SoftDeleteDogCommand { DogId = 1 };

            
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
            _repositoryMock.Verify(r => r.Remove(command.DogId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidJsonException_WhenRequestIsNull()
        {
            
            SoftDeleteDogCommand command = null;

           var exception= await Assert.ThrowsAsync<InvalidJsonException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("The request body is null or invalid.", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenInvalidOperationExceptionIsThrown()
        {
           
            var command = new SoftDeleteDogCommand { DogId = 1 };
            _repositoryMock.Setup(r => r.Remove(command.DogId)).ThrowsAsync(new InvalidOperationException("Invalid operation."));

          
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Error occurred while soft deleting the dog: Invalid operation.", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUnexpectedExceptionIsThrown()
        {
            var command = new SoftDeleteDogCommand { DogId = 1 };
            _repositoryMock.Setup(r => r.Remove(command.DogId)).ThrowsAsync(new Exception("An unexpected error occurred."));

            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("An unexpected error occurred: An unexpected error occurred.", exception.Message);
        }
    }
}

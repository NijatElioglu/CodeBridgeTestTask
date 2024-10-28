using AutoMapper;
using CodeBridgeTestTask.Application.DTO.Dog;
using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Application.Features.Dog.Queries.GetAll;
using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;


namespace CodeBridgeTestTaskUnitTestProject.QueryTests
{
    public class GetDogsQueryHandlerTests
    {
        private readonly Mock<IDogRepository> _mockDogRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetDogsQueryHandler>> _mockLogger;
        private readonly GetDogsQueryHandler _handler;

        public GetDogsQueryHandlerTests()
        {
            _mockDogRepository = new Mock<IDogRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetDogsQueryHandler>>();
            _handler = new GetDogsQueryHandler(_mockDogRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnDogDTOs_WhenDogsAreFound()
        {
            
            var dogs = new List<Dogs>
            {
                new Dogs { Name = "Rex", Weight = 30.5f, TailLenght = 20 },
                new Dogs { Name = "Max", Weight = 25.0f, TailLenght = 18 }
            };

            var dogDTOs = new List<DogDTO>
            {
                new DogDTO { Name = "Rex", Weight = 30.5f, TailLenght = 20 },
                new DogDTO { Name = "Max", Weight = 25.0f, TailLenght = 18 }
            };

            _mockDogRepository
                .Setup(repo => repo.GetAllWithSortingAndPagingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(dogs.AsQueryable());

            _mockMapper
                .Setup(m => m.Map<List<DogDTO>>(It.IsAny<List<Dogs>>()))
                .Returns(dogDTOs);

            var query = new GetDogsQuery
            {
                Attribute = "Name",
                Order = "asc",
                PageNumber = 1,
                PageSize = 10
            };

           
            var result = await _handler.Handle(query, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoDogsFound()
        {
            
            _mockDogRepository
                .Setup(repo => repo.GetAllWithSortingAndPagingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Enumerable.Empty<Dogs>().AsQueryable());

            var query = new GetDogsQuery
            {
                Attribute = "Name",
                Order = "asc",
                PageNumber = 1,
                PageSize = 10
            };

           
            var result = await _handler.Handle(query, CancellationToken.None);

            
            Assert.Empty(result);
            _mockLogger.Verify(logger => logger.LogWarning("No dogs found in the database."), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidJsonException_WhenRequestIsNull()
        {
           
            GetDogsQuery query = null;
            await Assert.ThrowsAsync<InvalidJsonException>(() => _handler.Handle(query, CancellationToken.None));
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), "An error occurred while retrieving dogs."), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionOccurs()
        {
           
            var exception = new System.Exception("Some error");

            _mockDogRepository
                .Setup(repo => repo.GetAllWithSortingAndPagingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(exception);

            var query = new GetDogsQuery
            {
                Attribute = "Name",
                Order = "asc",
                PageNumber = 1,
                PageSize = 10
            };


            await Assert.ThrowsAsync<System.Exception>(() => _handler.Handle(query, CancellationToken.None));


            _mockLogger.Verify(logger => logger.LogError(exception, "An error occurred while retrieving dogs."), Times.Once);
        }
      


    }
}


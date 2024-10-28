using AutoMapper;
using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Application.Logging;
using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Interfaces;
using MediatR;

namespace CodeBridgeTestTask.Application.Features.Dog.Commands.Create
{
    public class CreateDogCommandHandler : IRequestHandler<CreateDogCommand, int>
    {
        private readonly IDogRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CreateDogCommandHandler(IDogRepository repository, IMapper mapper, ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateDogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    throw new InvalidJsonException("The request body is null or invalid.");
                }

                if (request.TailLenght < 0)
                {
                    _logger.LogInformation("TailLength cannot be negative.");
                    throw new ArgumentException("TailLength cannot be negative.");
                }

                if (request.Weight < 0)
                {
                    _logger.LogInformation("Weight cannot be negative.");
                    throw new ArgumentException("Weight cannot be negative.");
                }
                var existingDog = await _repository.GetByNameAsync(request.Name);
                if (existingDog != null)
                {
                    _logger.LogInformation($"A dog with the name '{request.Name}' already exists.");
                    throw new ArgumentException($"'{request.Name}' Name is already exsist.");
                }
                var dog = _mapper.Map<Dogs>(request); 
                await _repository.AddAsync(dog); 
           
                return dog.Id; 
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the dog.",ex);
                throw;
            }
        }
    }
}

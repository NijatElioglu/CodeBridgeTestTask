using AutoMapper;
using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodeBridgeTestTask.Application.Features.Dog.Commands.Update
{
    public class UpdateDogCommandHandler : IRequestHandler<UpdateDogCommand, int>
    {
        private readonly IDogRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDogCommandHandler> _logger;

        public UpdateDogCommandHandler(IDogRepository repository, IMapper mapper, ILogger<UpdateDogCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(UpdateDogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    throw new InvalidJsonException("The request body is null or invalid.");
                }

                if (request.TailLength < 0)
                {
                    throw new ArgumentException("TailLength cannot be negative.");
                }

                if (request.Weight < 0)
                {
                    throw new ArgumentException("Weight cannot be negative.");
                }


                var existingDog = await _repository.GetByNameAsync(request.Name);
                if (existingDog != null && existingDog.Id != request.Id)
                {
                    throw new ArgumentException($"A dog with the name '{request.Name}' already exists.");
                }

                var dog = await _repository.GetByIdAsync(request.Id);
                if (dog == null)
                {
                    throw new ArgumentException($"Dog with ID {request.Id} not found.");
                }


                _mapper.Map(request, dog);
                await _repository.UpdateAsync(dog);

                return dog.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating the dog.", ex);
                throw;
            }
        }
    }
}

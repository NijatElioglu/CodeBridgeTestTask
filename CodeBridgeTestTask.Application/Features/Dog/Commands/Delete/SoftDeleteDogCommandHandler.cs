using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Repositories;
using MediatR;
namespace CodeBridgeTestTask.Application.Features.Dog.Commands.Delete
{
    public class SoftDeleteDogCommandHandler : IRequestHandler<SoftDeleteDogCommand, Unit>
    {
        private readonly IGenericRepository<Dogs> _repository;

        public SoftDeleteDogCommandHandler(IGenericRepository<Dogs> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(SoftDeleteDogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    throw new InvalidJsonException("The request body is null or invalid.");
                }
                await _repository.Remove(request.DogId);
                return Unit.Value;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Error occurred while soft deleting the dog: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}

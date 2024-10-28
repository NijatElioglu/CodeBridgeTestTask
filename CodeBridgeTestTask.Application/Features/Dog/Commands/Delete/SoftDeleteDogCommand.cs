using MediatR;

namespace CodeBridgeTestTask.Application.Features.Dog.Commands.Delete
{
    public class SoftDeleteDogCommand : IRequest<Unit>
    {
        public int DogId { get; set; }
    }
}

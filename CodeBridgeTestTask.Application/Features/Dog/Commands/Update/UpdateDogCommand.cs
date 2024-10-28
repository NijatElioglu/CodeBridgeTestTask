using MediatR;

namespace CodeBridgeTestTask.Application.Features.Dog.Commands.Update
{
    public class UpdateDogCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double TailLength { get; set; }
        public double Weight { get; set; }
    }
}

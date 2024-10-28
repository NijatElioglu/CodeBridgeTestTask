using MediatR;
using System.ComponentModel.DataAnnotations;
namespace CodeBridgeTestTask.Application.Features.Dog.Commands.Create
{
    public class CreateDogCommand:IRequest<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public float TailLenght { get; set; }
        [Required]
        public float Weight { get; set; }
    }
}

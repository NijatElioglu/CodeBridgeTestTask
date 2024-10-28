using CodeBridgeTestTask.Application.DTO.Dog;
using MediatR;

namespace CodeBridgeTestTask.Application.Features.Dog.Queries.GetAll
{
    public class GetDogsQuery : IRequest<List<DogDTO>> 
    {
        public string Attribute { get; set; } = "name"; 
        public string Order { get; set; } = "asc";       
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

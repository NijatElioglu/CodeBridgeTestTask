using AutoMapper;
using CodeBridgeTestTask.Application.DTO.Dog;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Create;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Update;
using CodeBridgeTestTask.Core.Entities;

namespace CodeBridgeTestTask.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateDogCommand, Dogs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()).ReverseMap();
              CreateMap<DogDTO, Dogs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()).ReverseMap();
            CreateMap<UpdateDogCommand, Dogs>()
           .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}

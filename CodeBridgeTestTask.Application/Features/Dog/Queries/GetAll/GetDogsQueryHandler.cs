using AutoMapper;
using CodeBridgeTestTask.Application.DTO.Dog;
using CodeBridgeTestTask.Application.Exceptions;
using CodeBridgeTestTask.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CodeBridgeTestTask.Application.Features.Dog.Queries.GetAll
{
    public class GetDogsQueryHandler : IRequestHandler<GetDogsQuery, List<DogDTO>>
    {
        private readonly IDogRepository _dogsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDogsQueryHandler> _logger;

        public GetDogsQueryHandler(IDogRepository dogsRepository, IMapper mapper, ILogger<GetDogsQueryHandler> logger)
        {
            _dogsRepository = dogsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<DogDTO>> Handle(GetDogsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInformation("Retrieving sorted and paginated dogs from the database.");

                if (request == null)
                {
                    throw new InvalidJsonException("The request body is null or invalid.");
                }
                var dogs = await _dogsRepository.GetAllWithSortingAndPagingAsync(request.Attribute, request.Order, request.PageNumber, request.PageSize);

                if (dogs == null || !dogs.Any())
                {
                    _logger.LogWarning("No dogs found in the database.");
                    return new List<DogDTO>();
                }

                var dogDTOs = _mapper.Map<List<DogDTO>>(dogs.ToList());

                _logger.LogInformation($"{dogDTOs.Count} dogs retrieved successfully.");

                return dogDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving dogs.");
                throw;
            }
        }

    }
}

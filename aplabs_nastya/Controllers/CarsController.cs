using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aplabs_nastya.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CarController(IRepositoryManager repository, ILoggerManager
       logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetCars()
        {
            var cars = _repository.Car.GetAllCars(trackChanges: false);
            var carsDto = _mapper.Map<IEnumerable<CarDto>>(cars);
            return Ok(carsDto);
        }
    }
}

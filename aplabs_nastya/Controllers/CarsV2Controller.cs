using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aplabs_nastya.Controllers
{
    [Route("api/{v:apiversion}/cars")]
    [ApiController]
    public class CarV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CarV2Controller(IRepositoryManager repository, ILoggerManager
       logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpOptions]
        public IActionResult GetCarsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS");
            return Ok();
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _repository.Car.GetAllCarsAsync(trackChanges: false);
            var carsDto = _mapper.Map<IEnumerable<CarDto>>(cars);
            return Ok(carsDto);
        }
    }
}

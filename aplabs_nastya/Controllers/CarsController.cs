using aplabs_nastya.ModelBinders;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id}", Name = "CarById")]
        public IActionResult GetCar(Guid id)
        {
            var car = _repository.Car.GetCar(id, trackChanges: false);
            if (car == null)
            {
                _logger.LogInfo($"Car with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var carDto = _mapper.Map<CarDto>(car);
                return Ok(carDto);
            }
        }

        [HttpPost]
        public IActionResult CreateCar([FromBody] CarForCreationDto car)
        {
            if (car == null)
            {
                _logger.LogError("CarForCreationDto object sent from client is null.");
            return BadRequest("CarForCreationDto object is null");
            }
            var carEntity = _mapper.Map<Car>(car);
            _repository.Car.CreateCar(carEntity);
            _repository.Save();
            var carToReturn = _mapper.Map<CarDto>(carEntity);
            return CreatedAtRoute("CarById", new { id = carToReturn.Id },
            carToReturn);
        }

        [HttpGet("collection/({ids})", Name = "CarCollection")]
        public IActionResult GetCarCollection([ModelBinder(BinderType =
        typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var carEntities = _repository.Car.GetByIds(ids, trackChanges: false);
            if (ids.Count() != carEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var carsToReturn =
           _mapper.Map<IEnumerable<CarDto>>(carEntities);
            return Ok(carsToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateCarCollection([FromBody]
        IEnumerable<CarForCreationDto> carCollection)
        {
            if (carCollection == null)
            {
                _logger.LogError("Car collection sent from client is null.");
                return BadRequest("Car collection is null");
            }
            var carEntities = _mapper.Map<IEnumerable<Car>>(carCollection);
            foreach (var car in carEntities)
            {
                _repository.Car.CreateCar(car);
            }
            _repository.Save();
            var carCollectionToReturn =
            _mapper.Map<IEnumerable<CarDto>>(carEntities);
            var ids = string.Join(",", carCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CarCollection", new { ids },
            carCollectionToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(Guid id)
        {
            var client = _repository.Client.GetClient(id, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Client.DeleteClient(client);
            _repository.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCar(Guid id, [FromBody] CarForUpdateDto car)
        {
            if (car == null)
            {
                _logger.LogError("CarForUpdateDto object sent from client is null.");
                return BadRequest("CarForUpdateDto object is null");
            }
            var carEntity = _repository.Car.GetCar(id, trackChanges: true);
            if (carEntity == null)
            {
                _logger.LogInfo($"Car with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(car, carEntity);
            _repository.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCar(Guid id,
        [FromBody] JsonPatchDocument<CarForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var carEntity = _repository.Car.GetCar(id, trackChanges: true);
            if (carEntity == null)
            {
                _logger.LogInfo($"Car with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var carToPatch = _mapper.Map<CarForUpdateDto>(carEntity);
            patchDoc.ApplyTo(carToPatch);
            _mapper.Map(carToPatch, carEntity);
            _repository.Save();
            return NoContent();
        }
    }
}

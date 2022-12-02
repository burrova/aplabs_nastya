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
    [ApiExplorerSettings(GroupName = "v1")]
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

        /// <summary>
        /// Получает список всех машин
        /// </summary>
        /// <returns>Список машин</returns>.
        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _repository.Car.GetAllCarsAsync(trackChanges: false);
            var carsDto = _mapper.Map<IEnumerable<CarDto>>(cars);
            return Ok(carsDto);
        }

        /// <summary>
        /// Получает машину по ID
        /// </summary>
        /// <returns>Машина</returns>.
        [HttpGet("{id}", Name = "CarById")]
        public async Task<IActionResult> GetCar(Guid id)
        {
            var car = await _repository.Car.GetCarAsync(id, trackChanges: false);
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

        /// <summary>
        /// Создает вновь созданную машину
        /// </summary>
        /// <param name="car"></param>.
        /// <returns>Вновь созданная машина</returns>.
        /// <response code="201"> Возвращает только что созданный элемент</response>.
        /// <response code="400"> Если элемент равен null</response>.
        /// <responce code="422"> Если модель недействительна</responce>.
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CarForCreationDto car)
        {
            if (car == null)
            {
                _logger.LogError("CarForCreationDto object sent from client is null.");
            return BadRequest("CarForCreationDto object is null");
            }
            var carEntity = _mapper.Map<Car>(car);
            _repository.Car.CreateCar(carEntity);
            await _repository.SaveAsync();
            var carToReturn = _mapper.Map<CarDto>(carEntity);
            return CreatedAtRoute("CarById", new { id = carToReturn.Id },
            carToReturn);
        }

        /// <summary>
        /// Получает коллекцию машин
        /// </summary>
        /// <returns>Список машин</returns>.
        [HttpGet("collection/({ids})", Name = "CarCollection")]
        public async Task<IActionResult> GetCarCollection([ModelBinder(BinderType =
        typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var carEntities = await _repository.Car.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != carEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var carsToReturn =
           _mapper.Map<IEnumerable<CarDto>>(carEntities);
            return Ok(carsToReturn);
        }

        /// <summary>
        /// Создает коллекцию машин
        /// </summary>
        /// <returns>Вновь созданная коллекция машин</returns>.
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCarCollection([FromBody]
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
            await _repository.SaveAsync();
            var carCollectionToReturn =
            _mapper.Map<IEnumerable<CarDto>>(carEntities);
            var ids = string.Join(",", carCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CarCollection", new { ids },
            carCollectionToReturn);
        }

        /// <summary>
        /// Удаляет машину
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var client = await _repository.Client.GetClientAsync(id, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Client.DeleteClient(client);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Изменяет машину (стирая старые свойства)
        /// </summary>
        /// <returns>Измененная машина</returns>.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(Guid id, [FromBody] CarForUpdateDto car)
        {
            if (car == null)
            {
                _logger.LogError("CarForUpdateDto object sent from client is null.");
                return BadRequest("CarForUpdateDto object is null");
            }
            var carEntity = await _repository.Car.GetCarAsync(id, trackChanges: true);
            if (carEntity == null)
            {
                _logger.LogInfo($"Car with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(car, carEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Изменяет машину (сохраняя старые свойства)
        /// </summary>
        /// <returns>Измененная машина</returns>.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateCar(Guid id,
        [FromBody] JsonPatchDocument<CarForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var carEntity = await _repository.Car.GetCarAsync(id, trackChanges: true);
            if (carEntity == null)
            {
                _logger.LogInfo($"Car with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var carToPatch = _mapper.Map<CarForUpdateDto>(carEntity);
            patchDoc.ApplyTo(carToPatch);
            _mapper.Map(carToPatch, carEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

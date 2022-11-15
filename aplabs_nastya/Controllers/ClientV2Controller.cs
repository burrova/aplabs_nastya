using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aplabs_nastya.Controllers
{
    [Route("api/{v:apiversion}/clients")]
    [ApiController]
    public class ClientV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ClientV2Controller(IRepositoryManager repository, ILoggerManager
       logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpOptions]
        public IActionResult GetClientsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS");
            return Ok();
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _repository.Client.GetAllClientsAsync(trackChanges: false);
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(clientsDto);
        }
    }
}

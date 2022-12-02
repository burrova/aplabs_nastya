using aplabs_nastya.ModelBinders;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace aplabs_nastya.Controllers
{
    [Route("api/clients")]
    [ApiController, Authorize(Roles = "Administrator")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ClientController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ClientController(IRepositoryManager repository, ILoggerManager
       logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _repository.Client.GetAllClientsAsync(trackChanges: false);
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(clientsDto);
        }
        [HttpGet("{id}", Name = "ClientById")]
        public async Task<IActionResult> GetClientAsync(Guid id)
        {
            var client = await _repository.Client.GetClientAsync(id, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var clientDto = _mapper.Map<ClientDto>(client);
                return Ok(clientDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientForCreationDto client)
        {
            if (client == null)
            {
                _logger.LogError("ClientForCreationDto object sent from client is null.");
            return BadRequest("ClientForCreationDto object is null");
            }
            var clientEntity = _mapper.Map<Client>(client);
            _repository.Client.CreateClient(clientEntity);
            await _repository.SaveAsync();
            var clientToReturn = _mapper.Map<ClientDto>(clientEntity);
            return CreatedAtRoute("ClientById", new { id = clientToReturn.Id },
            clientToReturn);
        }

        [HttpGet("collection/({ids})", Name = "ClientCollection")]
        public async Task<IActionResult> GetClientCollection([ModelBinder(BinderType =
        typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var clientEntities = await _repository.Client.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != clientEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var clientsToReturn =
           _mapper.Map<IEnumerable<ClientDto>>(clientEntities);
            return Ok(clientsToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateClientCollection([FromBody]
        IEnumerable<ClientForCreationDto> clientCollection)
        {
            if (clientCollection == null)
            {
                _logger.LogError("Client collection sent from client is null.");
                return BadRequest("Client collection is null");
            }
            var clientEntities = _mapper.Map<IEnumerable<Client>>(clientCollection);
            foreach (var client in clientEntities)
            {
                _repository.Client.CreateClient(client);
            }
            await _repository.SaveAsync();
            var clientCollectionToReturn =
            _mapper.Map<IEnumerable<ClientDto>>(clientEntities);
            var ids = string.Join(",", clientCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("ClientCollection", new { ids },
            clientCollectionToReturn);
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientForUpdateDto client)
        {
            if (client == null)
            {
                _logger.LogError("ClientForUpdateDto object sent from client is null.");
                return BadRequest("ClientForUpdateDto object is null");
            }
            var clientEntity = await _repository.Client.GetClientAsync(id, trackChanges: true);
            if (clientEntity == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(client, clientEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateClient(Guid id,
        [FromBody] JsonPatchDocument<ClientForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var clientEntity = await _repository.Client.GetClientAsync(id, trackChanges: true);
            if (clientEntity == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var clientToPatch = _mapper.Map<ClientForUpdateDto>(clientEntity);
            patchDoc.ApplyTo(clientToPatch);
            _mapper.Map(clientToPatch, clientEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

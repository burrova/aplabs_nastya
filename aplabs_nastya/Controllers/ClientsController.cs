﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aplabs_nastya.Controllers
{
    [Route("api/clients")]
    [ApiController]
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
        public IActionResult GetClients()
        {
            var clients = _repository.Client.GetAllClients(trackChanges: false);
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(clientsDto);
        }
        [HttpGet("{id}")]
        public IActionResult GetClient(Guid id)
        {
            var client = _repository.Client.GetClient(id, trackChanges: false);
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
    }
}

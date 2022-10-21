﻿using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToListAsync();

        public async Task<Client> GetClientAsync(Guid clientId, bool trackChanges) => await FindByCondition(c
        => c.Id.Equals(clientId), trackChanges).SingleOrDefaultAsync();

        public void CreateClient(Client client) => Create(client);
        public async Task<IEnumerable<Client>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeleteClient(Client client)
        {
            Delete(client);
        }
    }
}

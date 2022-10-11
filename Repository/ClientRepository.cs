using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public IEnumerable<Client> GetAllClients(bool trackChanges) =>
        FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToList();

        public Client GetClient(Guid clientId, bool trackChanges) => FindByCondition(c
        => c.Id.Equals(clientId), trackChanges).SingleOrDefault();

        public void CreateClient(Client client) => Create(client);
        public IEnumerable<Client> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
FindByCondition(x => ids.Contains(x.Id), trackChanges).ToList();
    }
}

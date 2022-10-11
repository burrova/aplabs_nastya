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
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public IEnumerable<Car> GetAllCars(bool trackChanges) =>
        FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToList();
        public Car GetCar(Guid carId, bool trackChanges) => FindByCondition(c
=> c.Id.Equals(carId), trackChanges).SingleOrDefault();
    }
}

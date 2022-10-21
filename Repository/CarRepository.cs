using Contracts;
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
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToListAsync();
        public async Task<Car> GetCarAsync(Guid carId, bool trackChanges) => await FindByCondition(c
        => c.Id.Equals(carId), trackChanges).SingleOrDefaultAsync();
        public void CreateCar(Car car) => Create(car);
        public async Task<IEnumerable<Car>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeleteCar(Car car)
        {
            Delete(car);
        }
    }
}

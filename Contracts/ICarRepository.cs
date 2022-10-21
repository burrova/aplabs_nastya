using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync(bool trackChanges);
        Task<Car> GetCarAsync(Guid carId, bool trackChanges);
        void CreateCar(Car car);
        Task<IEnumerable<Car>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCar(Car car);
    }
}

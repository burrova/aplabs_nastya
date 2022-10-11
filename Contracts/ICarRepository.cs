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
        IEnumerable<Car> GetAllCars(bool trackChanges);
        Car GetCar(Guid carId, bool trackChanges);
        void CreateCar(Car car);
        IEnumerable<Car> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    }
}

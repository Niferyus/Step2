using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id, CancellationToken ct);
        Task<List<T>> GetAll(CancellationToken ct);
        Task Create(T entity, CancellationToken ct);
        Task Update(T entity, CancellationToken ct);
        Task Delete(T entity, CancellationToken ct);
    }
}

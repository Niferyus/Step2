using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email, CancellationToken ct = default);
        Task Add(User user, CancellationToken ct = default);
    }
}

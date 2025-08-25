using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.PasswordHasher
{
    public class PasswordHasherService : IPasswordHasher
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool Control(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

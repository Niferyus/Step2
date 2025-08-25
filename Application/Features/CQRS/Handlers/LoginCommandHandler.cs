using Application.Dtos;
using Application.Features.CQRS.Commands;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CQRS.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;

        public LoginCommandHandler(IUserRepository users, IPasswordHasher hasher, IJwtService jwt)
        {
            _users = users;
            _hasher = hasher;
            _jwt = jwt;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _users.GetByEmail(request.Email, ct)
                       ?? throw new UnauthorizedAccessException("Invalid credentials.");

            if (!_hasher.Control(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var token = _jwt.GenerateToken(user);
            return new AuthResponse
            {
                AccessToken = token,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}

using Application.Dtos;
using Application.Features.CQRS.Commands;
using Application.Interfaces;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CQRS.Handlers
{
    public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;
        private readonly IUnitOfWork _uow;

        public RegisterUserCommandHandler(IUserRepository users, IPasswordHasher hasher, IJwtService jwt, IUnitOfWork uow)
        {
            _users = users;
            _hasher = hasher;
            _jwt = jwt;
            _uow = uow;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken ct)
        {
            var exists = await _users.GetByEmail(request.Email, ct);
            if (exists is not null) throw new InvalidOperationException("Email already registered.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = _hasher.Hash(request.Password)
            };

            await _users.Add(user, ct);
            await _uow.SaveChanges(ct);

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

using Application.Dtos;
using Application.Features.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            _logger.LogInformation("User registration attempt for email: {Email}", request.Email);

            var command = new RegisterUserCommand
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            _logger.LogInformation("User registration completed for email: {Email}", request.Email);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            _logger.LogInformation("User login attempt for email: {Email}", request.Email);

            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            _logger.LogInformation("User login completed for email: {Email}", request.Email);

            return Ok(result);
        }
    }
}

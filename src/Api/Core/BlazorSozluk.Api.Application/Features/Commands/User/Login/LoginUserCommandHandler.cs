using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetSingleAsync(i => i.EmailAddres == request.EmailAddress);
            if (dbUser == null)
            {
                throw new DatabaseValidationExcepiton("User not found");
            }
            var pass = PasswordEncryptor.Encrypt(request.Password);
            if (dbUser.Password != pass)
            {
                throw new DatabaseValidationExcepiton("Password is wrong");
            }
            if (!dbUser.EmailConfirmed)
            {
                throw new DatabaseValidationExcepiton("Email not yet confirmed");

            }

            var result = mapper.Map<LoginUserViewModel>(dbUser);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,dbUser.Id.ToString()),
                new Claim(ClaimTypes.Email,dbUser.EmailAddres),
                new Claim(ClaimTypes.Name,dbUser.UserName),
                new Claim(ClaimTypes.GivenName,dbUser.FirstName),
                new Claim(ClaimTypes.Surname,dbUser.LastName),
            };
            result.Token = GenerateToken(claims);
            return result;
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims,
                expires: expiry,
                signingCredentials: creds,
                notBefore: DateTime.Now);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}

using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {


        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId.HasValue)
            {
                new DatabaseValidationExcepiton("not null");
            }
            var dbUser = await userRepository.GetByIdAsync(request.UserId.Value);
            if (dbUser is null)
            {
                throw new DatabaseValidationExcepiton("User not found");
            }
            var encPass = PasswordEncryptor.Encrypt(request.OldPassword);
            if (dbUser.Password != encPass)
            {
                throw new DatabaseValidationExcepiton("Old Password is wrong");
            }
            dbUser.Password = encPass;

            await userRepository.UpdateAsync(dbUser);
            return true;
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var ecistsUser = await userRepository.GetSingleAsync(i => i.EmailAddres == request.EmailAddress);
            if (ecistsUser is not null)
            {
                throw new DatabaseValidationExcepiton("Email already exists");
            }
            var dbUser = mapper.Map<Domain.Models.User>(request);

            var rows = await userRepository.AddAsync(dbUser);


            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddres

                };
                QueueFactory.SendMessageToExchange(
                    obj: @event,
                    exchangeType: SozlukConstants.DefaultExcanhgeType,
                    queeueName: SozlukConstants.UserEmailChangeQueueName,
                    exchangeName: SozlukConstants.UserEmailExchangeName);
            }
            return dbUser.Id;

        }
    }
}

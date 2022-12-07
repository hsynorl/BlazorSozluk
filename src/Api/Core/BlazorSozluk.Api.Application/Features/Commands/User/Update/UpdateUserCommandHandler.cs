using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);

            if (dbUser == null)
            {
                throw new DatabaseValidationExcepiton("User not found");
            }

            var dbEmailAddress = dbUser.EmailAddres;
            var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

            mapper.Map(request, dbUser);
            var rows = await userRepository.UpdateAsync(dbUser);




            if (emailChanged && rows > 0)
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
                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);
            }





            //email confimerd
            return request.Id;
        }
    }
}

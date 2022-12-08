using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.Entry.CreateVote
{
    public class CreateEnrtyVoteCommandHandler : IRequestHandler<CreateEnrtyVoteCommand, bool>
    {
        public Task<bool> Handle(CreateEnrtyVoteCommand request, CancellationToken cancellationToken)
        {

            QueueFactory.SendMessageToExchange(
                exchangeName: SozlukConstants.VoteExchangeName,
                exchangeType: SozlukConstants.DefaultExcanhgeType,
                queeueName: SozlukConstants.CreateEntryVoteQueueName,
                obj: new CreateEntryVoteEvent()
                {
                    CratedBy = request.CreatedBy,
                    EntryId = request.EntryId,
                    VoteType = request.VoteType
                });
            return Task.FromResult(true);
        }
    }
}

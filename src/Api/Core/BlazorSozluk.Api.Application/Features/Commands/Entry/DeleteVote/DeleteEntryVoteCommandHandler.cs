using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.Entry.DeleteVote
{
    public class DeleteEntryVoteCommandHandler : IRequestHandler<DeleteEntryVoteCommand, bool>
    {
        public Task<bool> Handle(DeleteEntryVoteCommand request, CancellationToken cancellationToken)
        {

            QueueFactory.SendMessageToExchange(
                exchangeName: SozlukConstants.VoteExchangeName,
                exchangeType: SozlukConstants.DefaultExcanhgeType,
                queeueName: SozlukConstants.DeleteEntryVoteQueueName,
                obj: new DeleteEntryVoteEvent()
                {
                    UserId = request.UserId,
                    EntryId = request.EntryId
                });
            return Task.FromResult(true);
        }
    }
}

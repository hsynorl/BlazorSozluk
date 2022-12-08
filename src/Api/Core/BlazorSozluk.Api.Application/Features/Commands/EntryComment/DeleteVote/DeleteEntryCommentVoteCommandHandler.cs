using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorSozluk.Common.Events.EntryComment;

namespace BlazorSozluk.Api.Application.Features.Commands.EntryComment.DeleteVote
{
    public class DeleteEntryCommentVoteCommandHandler : IRequestHandler<DeleteEntryCommentVoteCommand, bool>
    {
        public Task<bool> Handle(DeleteEntryCommentVoteCommand request, CancellationToken cancellationToken)
        {

            QueueFactory.SendMessageToExchange(
                exchangeName: SozlukConstants.VoteExchangeName,
                exchangeType: SozlukConstants.DefaultExcanhgeType,
                queeueName: SozlukConstants.DeleteEntryCommentVoteQueueName,
                obj: new DeleteEntryCommentVoteEvent()
                {
                    EntryCommentId = request.EntryCommentId,
                    UserId = request.UserId
                });
            return Task.FromResult(true);
        }
    }
}

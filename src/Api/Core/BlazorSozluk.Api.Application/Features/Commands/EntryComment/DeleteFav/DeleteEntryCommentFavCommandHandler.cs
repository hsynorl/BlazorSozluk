using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using MediatR;
using BlazorSozluk.Common.Events.EntryComment;

namespace BlazorSozluk.Api.Application.Features.Commands.EntryComment.DeleteFav
{
    public class DeleteEntryCommentFavCommandHandler : IRequestHandler<DeleteEntryCommentFavCommand, bool>
    {
        public Task<bool> Handle(DeleteEntryCommentFavCommand request, CancellationToken cancellationToken)
        {

            QueueFactory.SendMessageToExchange(
                exchangeName: SozlukConstants.FavExchangeName,
                exchangeType: SozlukConstants.DefaultExcanhgeType,
                queeueName: SozlukConstants.CreateEntryCommentFavQueueName,
                obj: new DeleteEnrtyCommentFavEvent()
                {
                    UserId = request.UserId,
                    EntryCommentId = request.EntryCommentId
                });
            return Task.FromResult(true);
        }
    }
}

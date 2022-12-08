using BlazorSozluk.Common.Events.Entry;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.Entry.DeleteFav
{
    public class DeleteEntryCommandHandler : IRequestHandler<DeleteEntryCommand, bool>
    {
        public Task<bool> Handle(DeleteEntryCommand request, CancellationToken cancellationToken)
        {

            QueueFactory.SendMessageToExchange(
                exchangeName: SozlukConstants.FavExchangeName,
                exchangeType: SozlukConstants.DefaultExcanhgeType,
                queeueName: SozlukConstants.DeleteEntryFavQueueName,
                obj: new DeleteEntryFavEvent()
                {
                    UserId = request.UserId,
                    EntryId = request.EntryId
                });
            return Task.FromResult(true);
        }
    }
}

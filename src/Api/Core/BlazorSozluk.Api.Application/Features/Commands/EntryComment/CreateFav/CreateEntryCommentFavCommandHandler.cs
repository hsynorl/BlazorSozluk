﻿using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.EntryComment;
using BlazorSozluk.Common.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.EntryComment.CreateFav
{
    public class CreateEntryCommentFavCommandHandler : IRequestHandler<CreateEntryCommentFavCommand, bool>
    {
        public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.FavExchangeName,
                                               exchangeType: SozlukConstants.DefaultExcanhgeType,
                                               queeueName: SozlukConstants.CreateEntryCommentFavQueueName,
                                               obj: new CreateEntryCommentFavEvent()
                                               {
                                                   CratedBy = request.UserId,
                                                   EntryCommnetId = request.EntryCommentId
                                               });

            return await Task.FromResult(true);
        }
    }
}

﻿using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Infrastructure.Extensions;
using BlazorSozluk.Common.Models.Page;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetMainPageEntries
{
    public class GetMainPageEntriesQueryHandler : IRequestHandler<GetMainPageEntriesQuery, PagedViewModel<GetEntryDetailViewModel>>
    {
        private readonly IEntryRepository entryRepository;
        public GetMainPageEntriesQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> Handle(GetMainPageEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();
            query = query.Include(i => i.EntryFavorites)
                       .Include(i => i.CreatedBy)
                       .Include(i => i.EntryVotes);

            var list = query.Select(i => new GetEntryDetailViewModel()
            {
                Id = i.Id,
                Subject = i.Subject,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(k => k.CreatedById == request.UserId),
                FavoritedCount = i.EntryFavorites.Count,
                CreatedDate = i.CrerateDate,
                CreatedByUserName = i.CreatedBy.UserName,
                VoteType =
                        request.UserId.HasValue && i.EntryFavorites.Any(k => k.CreatedById == request.UserId)
                        ? i.EntryVotes.FirstOrDefault(l => l.CreatedById == request.UserId).VoteType
                        : Common.ViewModels.VoteType.None
            });

            var entries = await list.GetPaged(request.Page, request.PageSize);
            return entries;
        }
    }
}


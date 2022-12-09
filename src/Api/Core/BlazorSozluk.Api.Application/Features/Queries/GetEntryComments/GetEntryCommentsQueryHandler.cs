using BlazorSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using BlazorSozluk.Api.Application.Interfaces;
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

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
    {
        private readonly IEntryCommentRepository entryCommentRepository;
        public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
        {
            this.entryCommentRepository = entryCommentRepository;
        }

        public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = entryCommentRepository.AsQueryable();
            query = query.Include(i => i.EntryCommnetFavorites)
                       .Include(i => i.CreatedBy)
                       .Include(i => i.EntryCommentVotes)
                       .Where(i => i.EntryId == request.EntryId);

            var list = query.Select(i => new GetEntryCommentsViewModel()
            {
                Id = i.Id,

                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryCommnetFavorites.Any(k => k.CreatedById == request.UserId),
                FavoritedCount = i.EntryCommnetFavorites.Count,
                CreateDate = i.CrerateDate,
                CreatedByUserName = i.CreatedBy.UserName,
                VoteType =
                        request.UserId.HasValue && i.EntryCommnetFavorites.Any(k => k.CreatedById == request.UserId)
                        ? i.EntryCommentVotes.FirstOrDefault(l => l.CreatedById == request.UserId).VoteType
                        : Common.ViewModels.VoteType.None
            });

            var entries = await list.GetPaged(request.Page, request.PageSize);
            return entries;
        }
    }
}

using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntryDetail
{
    public class GetEntryDetailQueryHandler : IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>
    {
        private readonly IEntryRepository entryRepository;

        public GetEntryDetailQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        async Task<GetEntryDetailViewModel> IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>.Handle(GetEntryDetailQuery request, CancellationToken cancellationToken)
        {

            var query = entryRepository.AsQueryable();
            query = query.Include(i => i.EntryFavorites)
                       .Include(i => i.CreatedBy)
                       .Include(i => i.EntryVotes)
                       .Where(i => i.Id == request.EntryId);

            var list = query.Select(i => new GetEntryDetailViewModel()
            {
                Id = i.Id,
                Subject = i.Subject,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue &&
                i.EntryFavorites.Any(k => k.CreatedById == request.UserId),
                FavoritedCount = i.EntryFavorites.Count,
                CreatedDate = i.CrerateDate,
                CreatedByUserName = i.CreatedBy.UserName,
                VoteType =
                        request.UserId.HasValue && i.EntryFavorites.Any(k => k.CreatedById == request.UserId)
                        ? i.EntryVotes.FirstOrDefault(l => l.CreatedById == request.UserId).VoteType
                        : Common.ViewModels.VoteType.None
            });
            return await list.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        }


    }
}

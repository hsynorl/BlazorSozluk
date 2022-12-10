using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.SerachBarSubject
{
    public class SearchEntryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
    {
        private readonly IEntryRepository entryRepository;

        public SearchEntryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
        {
            var result = entryRepository.Get
                (i => EF.Functions.Like(i.Subject, $"%{request.SearchText}%"))
                .Select(i => new SearchEntryViewModel() { Subject = i.Subject, Id = i.Id });

            return await result.ToListAsync(cancellationToken);
        }
    }
}

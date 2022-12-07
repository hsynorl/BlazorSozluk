using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorSozluk.Infrastructure.Persistence.Repositories
{
    public class EntryCommentRepostory : GenericRepository<EntryComment>, IEntryCommentRepository
    {
        public EntryCommentRepostory(BlazorSozlukContext dbContext) : base(dbContext)
        {
        }
    }
}

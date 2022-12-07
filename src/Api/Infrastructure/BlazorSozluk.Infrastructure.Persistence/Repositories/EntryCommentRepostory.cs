using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorSozluk.Infrastructure.Persistence.Repositories
{
    public class EntryCommentRepostory : GenericRepository<EntryComment>, IEntryCommentRepository
    {
        public EntryCommentRepostory(DbContext dbContext) : base(dbContext)
        {
        }
    }
}

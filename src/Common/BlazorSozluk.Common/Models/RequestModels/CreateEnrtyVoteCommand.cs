using BlazorSozluk.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Models.RequestModels
{
    public class CreateEnrtyVoteCommand : IRequest<bool>
    {

        public Guid EntryId { get; set; }

        public Guid CreatedBy { get; set; }
        public VoteType VoteType { get; set; }

        public CreateEnrtyVoteCommand(Guid entryId, Guid createdBy, VoteType voteType)
        {
            EntryId = entryId;
            CreatedBy = createdBy;
            VoteType = voteType;
        }
    }
}

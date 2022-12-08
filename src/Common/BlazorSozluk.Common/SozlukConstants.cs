using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common
{
    public class SozlukConstants
    {
        public const string RabbitMQHost = "localhost";
        public const string DefaultExcanhgeType = "direct";


        public const string UserEmailExchangeName = "UserExchange";
        public const string UserEmailChangeQueueName = "UserEmailChangedQueue";

        public const string FavExchangeName = "FavExchange";
        public const string VoteExchangeName = "VoteExchange";

        public const string CreateEntryCommentFavQueueName = "CreateEntryCommentFavQueue";
        public const string CreateEntryCommentVoteQueueName = "CreateEntryCommentVoteQueue";
        public const string CreateEntryFavQueueName = "CreateEntryFavQueue";
        public const string CreateEntryVoteQueueName = "CreateEntryVoteQueue";


        public const string DeleteEntryCommentVoteQueueName = "DeleteEntryCommentVoteQueue";
        public const string DeleteEntryCommentFavQueueName = "DeleteEntryCommentFavQueue";
        public const string DeleteEntryVoteQueueName = "DeleteEntryVoteQueue";
        public const string DeleteEntryFavQueueName = "DeleteEntryFavQueue";




    }
}

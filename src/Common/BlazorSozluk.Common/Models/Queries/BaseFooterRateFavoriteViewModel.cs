using BlazorSozluk.Common.ViewModels;

namespace BlazorSozluk.Common.Models.Queries
{
    public class BaseFooterRateFavoriteViewModel:BaseFooterFavoriteViewModel
    {
        public VoteType VoteType { get; set; }
    }

}

using BlazorSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetUserDetail
{
    public class UserDetailQuery : IRequest<UserDetailViewModel>
    {
        private Guid id;

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public UserDetailQuery(Guid userId, string userName = null)
        {
            UserId = userId;
            UserName = userName;
        }


    }
}

using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Models.Queries;
using MediatR;

namespace BlazorSozluk.Api.Application.Features.Queries.GetUserDetail
{
    public class UserDetailHandler : IRequestHandler<UserDetailQuery, UserDetailViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserDetailHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserDetailViewModel> Handle(UserDetailQuery request, CancellationToken cancellationToken)
        {
            Domain.Models.BaseEntity dbUser = null;
            if (request.UserId != Guid.Empty)
            {
                dbUser = await userRepository.GetByIdAsync(request.UserId);
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                dbUser = await userRepository.GetSingleAsync(i => i.UserName == request.UserName);
            }
            return mapper.Map<UserDetailViewModel>(dbUser);
        }
    }
}

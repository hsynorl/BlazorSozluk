using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using MediatR;

namespace BlazorSozluk.Api.Application.Features.Commands.User.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailConfirmationRepository emailConfirmationRepository;

        public ConfirmEmailCommandHandler(IUserRepository userRepository, IEmailConfirmationRepository emailConfirmationRepository)
        {
            this.userRepository = userRepository;
            this.emailConfirmationRepository = emailConfirmationRepository;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var confirmation = await emailConfirmationRepository.GetByIdAsync(request.ConfirmationId);

            if (confirmation is null)
            {
                throw new DatabaseValidationExcepiton("COnfirmation not found");

            }
            var dbUser = await userRepository.GetSingleAsync(
                i => i.EmailAddres == confirmation.NewEmailAddress
                );
            if (dbUser is null)
            {
                throw new DatabaseValidationExcepiton("User not found with this email");
            }
            if (dbUser.EmailConfirmed)
            {
                throw new DatabaseValidationExcepiton("Email Addres is already confirmed");
            }
            dbUser.EmailConfirmed = true;
            await userRepository.UpdateAsync(dbUser);
            return true;
        }
    }
}

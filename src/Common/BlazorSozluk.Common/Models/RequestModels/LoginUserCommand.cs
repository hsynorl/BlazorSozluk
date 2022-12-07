using BlazorSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Models.RequestModels
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string email, string password)
        {
            EmailAddress = email;
            Password = password;
        }
        public LoginUserCommand()
        {

        }
    }
}

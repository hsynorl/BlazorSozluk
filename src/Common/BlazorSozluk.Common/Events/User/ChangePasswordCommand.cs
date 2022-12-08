using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Events.User
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public Guid? UserId { get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public ChangePasswordCommand(Guid? userId, string oldPassword, string newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}

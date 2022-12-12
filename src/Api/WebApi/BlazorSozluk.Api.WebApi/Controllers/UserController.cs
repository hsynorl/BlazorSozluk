using BlazorSozluk.Api.Application.Features.Commands.User.ConfirmEmail;
using BlazorSozluk.Api.Application.Features.Queries.GetUserDetail;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await mediator.Send(new UserDetailQuery(id));
            return Ok(user);
        }

        [HttpGet]
        [Route("UserName/{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var user = await mediator.Send(new UserDetailQuery(Guid.Empty, userName));
            return Ok(user);
        }













        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            //Gunyaruk_Yorulmaz94@hotmail.com
            //pass=247B75697AB471CEF66CC12D5B23CF99
            var query = await mediator.Send(loginUserCommand);
            return Ok(query);

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var guid = await mediator.Send(command);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Confirm")]
        public async Task<IActionResult> EmailConfirm(Guid id)
        {
            var guid = await mediator.Send(new ConfirmEmailCommand()
            {
                ConfirmationId = id
            });

            return Ok(guid);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            if (!changePasswordCommand.UserId.HasValue)
            {
                changePasswordCommand.UserId = UserId;
            }
            var guid = await mediator.Send(changePasswordCommand);


            return Ok(guid);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CreateUserCommand command)
        {
            var guid = await mediator.Send(command);

            return Ok(guid);
        }
    }
}

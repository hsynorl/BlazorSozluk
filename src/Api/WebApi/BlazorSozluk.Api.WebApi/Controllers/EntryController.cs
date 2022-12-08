using BlazorSozluk.Api.Application.Features.Queries.GetEntries;
using BlazorSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : BaseController
    {
        private readonly IMediator _mediator;

        public EntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetEntries")]
        public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery getEntriesQuery)
        {
            var query = await _mediator.Send(getEntriesQuery);
            return Ok(query);
        }


        [HttpGet]
        [Route("GetMainPageEntries")]
        public async Task<IActionResult> GetMainPageEntries([FromQuery] int page, int pageSize)
        {
            var query = await _mediator.Send(new GetMainPageEntriesQuery(UserId, page, pageSize));
            return Ok(query);
        }




        [HttpPost]
        [Route("CreateEntry")]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand createEntryCommand)
        {
            if (!createEntryCommand.CreatedById.HasValue)
            {
                createEntryCommand.CreatedById = UserId;
            }
            var result = await _mediator.Send(createEntryCommand);
            return Ok(result);

        }

        [HttpPost]
        [Route("CreateEntryComment")]
        public async Task<IActionResult> CreateEntryComment([FromBody] CreateEntryCommentCommand createEntryCommentCommand)
        {
            if (!createEntryCommentCommand.CreatedById.HasValue)
            {
                createEntryCommentCommand.CreatedById = UserId;
            }
            var result = await _mediator.Send(createEntryCommentCommand);
            return Ok(result);

        }


    }
}

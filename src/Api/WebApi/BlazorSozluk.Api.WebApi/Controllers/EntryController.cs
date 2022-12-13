using BlazorSozluk.Api.Application.Features.Queries.GetEntries;
using BlazorSozluk.Api.Application.Features.Queries.GetEntryComments;
using BlazorSozluk.Api.Application.Features.Queries.GetEntryDetail;
using BlazorSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using BlazorSozluk.Api.Application.Features.Queries.GetUserEntries;
using BlazorSozluk.Common.Models.Queries;
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

        [HttpGet("getById/{id}")]// Routeun 2. insiyatif kullanımı
        
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEntryDetailQuery(id, UserId));
            return Ok(result);
        }


        [HttpGet]
        [Route("Comments/{id}")]
        public async Task<IActionResult> GetEntryComments(Guid id, int page, int pageSize)
        {
            var result = await _mediator.Send(new GetEntryCommentsQuery(id, UserId, page, pageSize));
            return Ok(result);
        }

        [HttpGet]
        [Route("UserEntries")]
        public async Task<IActionResult> GetUserEntries(Guid userId, int page, int pageSize, string userName)
        {
            if (userId == Guid.Empty && string.IsNullOrEmpty(userName))
            {
                userId = UserId;
            }
            var result = await _mediator.Send(new GetUserEntriesDetailQuery(page, pageSize, userId, userName));
            return Ok(result);
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

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search([FromQuery] SearchEntryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

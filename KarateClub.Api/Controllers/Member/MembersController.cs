using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Handlers.Member;
using KarateClub.Application.Handlers.Member.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.Member
{
    [Route("api/members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly GetMemberHandler _getMemberHandler;
        private readonly GetMembersHandler _getMembersHandler;

        public MembersController(GetMemberHandler getMemberHandler, GetMembersHandler getMembersHandler)
        {
            _getMemberHandler = getMemberHandler;
            _getMembersHandler = getMembersHandler;
        }

        [Authorize(Policy = Permissions.Members.View)]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDetailsDto>> GetMemberById(int id)
        {
            return Ok(await _getMemberHandler.ExecuteAsync(new GetMemberQuery(id)));
        }

        [Authorize(Policy = Permissions.Members.View)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            return Ok(await _getMembersHandler.ExecuteAsync(new GetMembersQuery()));
        }
    }
}

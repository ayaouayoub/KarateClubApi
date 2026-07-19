using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Member;
using KarateClub.Application.Handlers.Member.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Application.Handlers.Member.Commands;
using KarateClub.Api.Controllers.Instructor.Requests;

namespace KarateClub.Api.Controllers.Member
{
    [Route("api/members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly GetMemberHandler _getMemberHandler;
        private readonly GetMembersHandler _getMembersHandler;
        private readonly DeactivateMemberHandler _deactivateMemberHandler;
        private readonly ActivateMemberHandler _activateMemberHandler;
        private readonly UpdateMemberCurrentBletRankHandler _updateMemberCurrentBletRankHandler;

        public MembersController(GetMemberHandler getMemberHandler, GetMembersHandler getMembersHandler, DeactivateMemberHandler deactivateMemberHandler, ActivateMemberHandler activateMemberHandler, UpdateMemberCurrentBletRankHandler updateMemberCurrentBletRankHandler)
        {
            _getMemberHandler = getMemberHandler;
            _getMembersHandler = getMembersHandler;
            _deactivateMemberHandler = deactivateMemberHandler;
            _activateMemberHandler = activateMemberHandler;
            _updateMemberCurrentBletRankHandler = updateMemberCurrentBletRankHandler;
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

        [Authorize(Policy = Permissions.Members.Delete)]
        [HttpPatch("{id:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivateMember(int id)
        {
            await _deactivateMemberHandler.ExecuteAsync(new DeactivateMemberCommand(id));
            return NoContent();
        }

        [Authorize(Policy = Permissions.Members.Update)]
        [HttpPatch("{id:int}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateMember(int id)
        {
            await _activateMemberHandler.ExecuteAsync(new ActivateMemberCommand(id));
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id:int}/blet-rank")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCurrentBletRank(int id, [FromBody] UpdateCurrentBletRankRequest request)
        {
            await _updateMemberCurrentBletRankHandler.ExecuteAsync(new UpdateMemberCurrentBletRankCommand(id, request.BeltRank));
            return NoContent();
        }
    }
}

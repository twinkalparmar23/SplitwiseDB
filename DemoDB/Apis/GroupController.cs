using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDB.Database;
using DemoDB.DataModel;
using DemoDB.Model;
using DemoDB.Repository;
using DemoDB.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoDB.Apis
{
    [Produces("application/json")]
    [Route("api/group")]
    public class GroupController : Controller
    {
        IGroupRepository _GroupRepository;
        ILogger _Logger;
        private DemoDbContext _Context;

        public GroupController(IGroupRepository groupRepo, ILoggerFactory loggerFactory, DemoDbContext context)
        {
            _GroupRepository = groupRepo;
            _Logger = loggerFactory.CreateLogger(nameof(UserController));
            _Context = context;
        }


        // GET api/group/all/userid
        [HttpGet("all/{id}")]
        [ProducesResponseType(typeof(List<GroupResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Groups(int id)
        {

            try
            {
                var groups = await _GroupRepository.GetGroupsAsync(id);
                return Ok(groups);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/group/5
        [HttpGet("{id}", Name = "GetGroupRoute")]
        [ProducesResponseType(typeof(GroupResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Group(int id)
        {
            try
            {
                var group = await _GroupRepository.GetGroupAsync(id);
                return Ok(group);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/group/all/userid/friendid
        [HttpGet("all/{Userid}/{Friendid}")]
        [ProducesResponseType(typeof(List<GroupResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> CommenGroups(int Userid,int Friendid)
        {

            try
            {
                var groups = await _GroupRepository.GetCommenGroupsAsync(Userid, Friendid);
                return Ok(groups);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // POST api/group
        [HttpPost]
        [ProducesResponseType(typeof(ApiCommonResponse), 201)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> CreateGroup([FromBody]GroupModel group )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false});
            }

            try
            {
                var newGroup = await _GroupRepository.InsertGroupAsync(group);
                if (newGroup == null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return CreatedAtRoute("GetGroupRoute", new { id = newGroup.GroupId },
                        new ApiCommonResponse { Status=true , id=newGroup.GroupId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // POST api/group/Groupid/Memberid
        [HttpPost("{Groupid}/{Memberid}")]
        [ProducesResponseType(typeof(GroupMember), 201)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> CreateGroupMember(int Groupid, int Memberid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var newMember = await _GroupRepository.InsertGroupMemberAsync(Groupid, Memberid);
                if (newMember == null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
               
                 return CreatedAtAction("GetGroupMemberRoute", new { id = newMember.User_Id },
                           new ApiCommonResponse { Status = true, id = newMember.User_Id });
               
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // PUT api/group/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> UpdateGroup(int id, [FromBody]Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var status = await _GroupRepository.UpdateGroupAsync(group);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id = group.GroupId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            try
            {
                var status = await _GroupRepository.DeleteGroupAsync(id);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id= id });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // DELETE api/group/Groupid/Memberid
        [HttpDelete("{Groupid}/{Memberid}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> DeleteUser(int Groupid, int Memberid)
        {
            try
            {
                var status = await _GroupRepository.DeleteGroupMemberAsync(Groupid, Memberid);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id = Memberid });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

    }
}

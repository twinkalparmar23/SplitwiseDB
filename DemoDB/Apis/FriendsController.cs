﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDB.Database;
using DemoDB.Model;
using DemoDB.Repository;
using DemoDB.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoDB.Apis
{
    [Produces("application/json")]
    [Route("api/friends")]
    public class FriendsController : Controller
    {
        IFriendListRepository _FriendListRepository;
        ILogger _Logger;
        private DemoDbContext _Context;

        public FriendsController(IFriendListRepository friendRepo, ILoggerFactory loggerFactory, DemoDbContext context)
        {
            _FriendListRepository = friendRepo;
            _Logger = loggerFactory.CreateLogger(nameof(FriendsController));
            _Context = context;
        }

        // GET api/friends/id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FriendResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Friend(int id)
        {
            try
            {
                var friends = await _FriendListRepository.GetFriendAsync(id);
                return Ok(friends);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/friends/all/id
        [HttpGet("all/{id}")]
        [ProducesResponseType(typeof(FriendResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Friends(int id)
        {
            try
            {
                var friends = await _FriendListRepository.GetAllFriendsAsync(id);
                return Ok(friends);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }


        // POST api/friends/id(user)/username(friend)/email(friend)
        [HttpPost("{id}/{userName}/{email}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 201)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> CreateFriend(int id, string userName, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var newUser = await _FriendListRepository.InsertFriendAsync(id,userName,email);
                if (newUser == null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                if (newUser.UserId != 0)
                {
                    return CreatedAtAction("GetFriendRoute", new { id = newUser.UserId },
                           new ApiCommonResponse { Status = true, id = newUser.FriendListId });
                }
                else
                {
                    return BadRequest( new ApiCommonResponse { Status = false, id = 0 });
                }
                        
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }


        // DELETE api/friends/uid/fid
        [HttpDelete("{uid}/{fid}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> DeleteUser(int fid, int uid)
        {
            try
            {
                var status = await _FriendListRepository.DeleteFriendAsync(fid,uid);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id =fid });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

    }
}

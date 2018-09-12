using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDB.Database;
using DemoDB.Model;
using DemoDB.Repository;
using DemoDB.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoDB.Apis
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        IUserRepository _UserRepository;
        ILogger _Logger;
        private DemoDbContext _Context;

        public UserController(IUserRepository userRepo, ILoggerFactory loggerFactory, DemoDbContext context)
        {
            _UserRepository = userRepo;
            _Logger = loggerFactory.CreateLogger(nameof(UserController));
            _Context = context;
        }
        

        // GET api/user
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Users()
        {

            try
            {
                var Users = await _UserRepository.GetUsersAsync();
                return Ok(Users);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/user/id
        [HttpGet("{id}", Name = "GetUserRoute")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Users(int id)
        {
            try
            {
                var user = await _UserRepository.GetUserAsync(id);
                return Ok(user);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // POST api/user
        [HttpPost]
        [ProducesResponseType(typeof(ApiCommonResponse), 201)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> CreateUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var newUser = await _UserRepository.InsertUserAsync(user);
                if (newUser == null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return CreatedAtRoute("GetUserRoute", new { id = newUser.UserId },
                        new ApiCommonResponse { Status = true, id = newUser.UserId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // PUT api/user/id
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> UpdateUser(int id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var status = await _UserRepository.UpdateUserAsync(user);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id = user.UserId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // DELETE api/user/id
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var status = await _UserRepository.DeleteUserAsync(id);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id = id });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        //GET api/user/login/email/password
        [Route("api/user/login/{email}/{password}")]
        [HttpGet("login/{email}/{password}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> Loginuser(string email, string password)
        {
            try
            {
                var user = await _UserRepository.LoginUserAsync(email,password);
                if (user== null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true,id= user.UserId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }

        }
    }
}

using System;
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
    [Route("api/settle")]
    public class SettlementController : Controller
    {
        ISettlementRepository _SettlementRepository;
        ILogger _Logger;
        private DemoDbContext _Context;

        public SettlementController(ISettlementRepository settleRepo, ILoggerFactory loggerFactory, DemoDbContext context)
        {
            _SettlementRepository = settleRepo;
            _Logger = loggerFactory.CreateLogger(nameof(SettlementController));
            _Context = context;
        }

        // GET api/settle/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<SettlementResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetSettlement(int id)
        {

            try
            {
                var data = await _SettlementRepository.GetSettlementAsync(id);
                return Ok(data);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/settle/group/groupid
        [HttpGet("group/{id}")]
        [ProducesResponseType(typeof(List<SettlementResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetGroupSettlement(int id)
        {

            try
            {
                var data = await _SettlementRepository.GetGroupSettlementAsync(id);
                return Ok(data);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/settle/1/2
        [HttpGet("{uid}/{fid}")]
        [ProducesResponseType(typeof(List<SettlementResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetSettlement(int uid,int fid)
        {

            try
            {
                var data = await _SettlementRepository.GetSettlementAsync(uid,fid);
                return Ok(data);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        
        // GET api/settle/all/userid
        [HttpGet("all/{id}")]
        [ProducesResponseType(typeof(List<SettlementResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetAllSettlements(int id)
        {

            try
            {
                var data = await _SettlementRepository.GetAllSettlementAsync(id);
                return Ok(data);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }
    }
}

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
    [Route("api/bill")]
    public class BillController : Controller
    {
        IBillRepository _BillRepository;
        ILogger _Logger;
        private DemoDbContext _Context;

        public BillController(IBillRepository billRepo, ILoggerFactory loggerFactory, DemoDbContext context)
        {
            _BillRepository = billRepo;
            _Logger = loggerFactory.CreateLogger(nameof(BillController));
            _Context = context;
        }

        // GET api/bill/5
        [HttpGet("{id}", Name = "GetBillRoute")]
        [ProducesResponseType(typeof(BillResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> SingleBill(int id)
        {
            try
            {
                var bill = await _BillRepository.GetBillAsync(id);
                return Ok(bill);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // POST api/bill
        [HttpPost]
        [ProducesResponseType(typeof(ApiCommonResponse), 201)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> CreateBill([FromBody]BillModel bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var newBill = await _BillRepository.InsertBillAsync(bill);
                if (newBill == null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return CreatedAtRoute("GetBillRoute", new { id = newBill.BillId },
                        new ApiCommonResponse { Status = true, id = newBill.BillId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // PUT api/bill/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> UpdateBill(int id, [FromBody]Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var status = await _BillRepository.UpdateBillAsync(bill);
                if (!status)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return Ok(new ApiCommonResponse { Status = true, id = bill.BillId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }


        // GET api/bill/all/userid
        [HttpGet("allbill/{id}")]
        [ProducesResponseType(typeof(List<BillResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> AllBills(int id)
        {

            try
            {
                var bills = await _BillRepository.GetAllExpenses(id);
                return Ok(bills);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/bill/all/userid/friendid
        [HttpGet("all/{Userid}/{Friendid}")]
        [ProducesResponseType(typeof(List<BillResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> IndividualBills(int Userid, int Friendid)
        {

            try
            {
                var bills = await _BillRepository.GetIndividualExpenses(Userid, Friendid);
                return Ok(bills);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/bill/all/groupid
        [HttpGet("all/{Groupid}")]
        [ProducesResponseType(typeof(List<BillResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GroupBills(int Groupid)
        {

            try
            {
                var bills = await _BillRepository.GetGroupExpenses(Groupid);
                return Ok(bills);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // DELETE api/bill/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiCommonResponse), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            try
            {
                var status = await _BillRepository.DeleteBillAsync(id);
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
    }
}

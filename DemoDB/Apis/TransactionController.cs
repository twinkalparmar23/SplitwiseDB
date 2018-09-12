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
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        ITransactionRepository _TransactionRepository;
        ILogger _Logger;
        private DemoDbContext _Context;

        public TransactionController(ITransactionRepository transRepo, ILoggerFactory loggerFactory, DemoDbContext context)
        {
            _TransactionRepository = transRepo;
            _Logger = loggerFactory.CreateLogger(nameof(TransactionController));
            _Context = context;
        }

        // GET api/transaction/id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetTransaction(int id)
        {

            try
            {
                var transaction = await _TransactionRepository.GetTransactionAsync(id);
                return Ok(transaction);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/transaction/all/groupid
        [HttpGet("all/{Groupid}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetGroupTransactions(int Groupid)
        {

            try
            {
                var transactions = await _TransactionRepository.GetGroupTransactionsAsync(Groupid);
                return Ok(transactions);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/transaction/all/userid/friendid
        [HttpGet("all/{Userid}/{Friendid}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetIndividualTransactions(int Userid, int Friendid)
        {

            try
            {
                var transactions = await _TransactionRepository.GetIndividualTransactionsAsync(Userid,Friendid);
                return Ok(transactions);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // GET api/transaction/alltrans/userid
        [HttpGet("alltrans/{Userid}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> GetAllTransactions(int Userid)
        {

            try
            {
                var transactions = await _TransactionRepository.GetAllTransactionsAsync(Userid);
                return Ok(transactions);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }

        // POST api/transaction && PUT settlement
        [HttpPost]
        [ProducesResponseType(typeof(ApiCommonResponse), 201)]
        [ProducesResponseType(typeof(ApiCommonResponse), 400)]
        public async Task<ActionResult> RecordPayment([FromBody]Transactions payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiCommonResponse { Status = false });
            }

            try
            {
                var newTrans = await _TransactionRepository.RecordPaymentAsync(payment);
                if (newTrans == null)
                {
                    return BadRequest(new ApiCommonResponse { Status = false });
                }
                return CreatedAtAction("GetTransactionRoute", new { id = newTrans.TransactionId },
                            new ApiCommonResponse { Status = true, id = newTrans.TransactionId });
                //return Ok(newTrans);
                //return CreatedAtRoute("GetBillRoute", new { id = newTrans.TransactionId },
                //        new ApiCommonResponse { Status = true, id = newTrans.TransactionId });
                //return new ApiCommonResponse { Status = true, id = newTrans.TransactionId };
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiCommonResponse { Status = false });
            }
        }
    }
}

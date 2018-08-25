﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDB.Database;
using DemoDB.Model;
using DemoDB.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoDB.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DemoDbContext _Context;
        private readonly ILogger _Logger;

        public TransactionRepository(DemoDbContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("TransactionRepository");
        }
        
        public async Task<TransactionResponse> GetTransactionAsync(int id)
        {
            // return await _Context.Transactions.SingleOrDefaultAsync(c => c.TransactionId == id);

            TransactionResponse transaction = new TransactionResponse();
            var payer = new MemberResponse();
            var receiver = new MemberResponse();

            var tData = _Context.Transactions.SingleOrDefault(c => c.TransactionId == id);
            var payerData = _Context.User.SingleOrDefault(c => c.UserId == tData.TransPayersId);
            payer.Id = payerData.UserId;
            payer.Name = payerData.UserName;

            var recData = _Context.User.SingleOrDefault(c => c.UserId == tData.TransReceiversId);
            receiver.Id = recData.UserId;
            receiver.Name = recData.UserName;

            transaction.Id = tData.TransactionId;
            transaction.Payer = payer;
            transaction.Receiver = receiver;
            transaction.GroupId = tData.GroupId.GetValueOrDefault();

            if (transaction.GroupId != 0)
            {
                var name = _Context.Group.SingleOrDefault(c => c.GroupId == transaction.GroupId);
                transaction.GroupName = name.GroupName;
            }

            transaction.PaidAmount = tData.PaidAmount;
            transaction.CreatedDate = tData.CreatedDate;
            return transaction;
        }

        public async Task<Transactions> RecordPaymentAsync(Transactions payment)
        {
            
            _Context.Transactions.Add(payment);

            if (payment.GroupId == null)
            {
                var settleData = _Context.Settlement.SingleOrDefault(c => c.PayerId == payment.TransPayersId && c.SharedMemberId == payment.TransReceiversId && c.GroupId==null);
                if (settleData != null)
                {
                    settleData.TotalAmount = settleData.TotalAmount - payment.PaidAmount;
                    _Context.Settlement.Attach(settleData);
                }
                else
                {
                    
                    //settle.TotalAmount = payment.PaidAmount;
                    //_Context.Settlement.Add(settle);

                    var set = _Context.Settlement.SingleOrDefault(c => c.PayerId == payment.TransReceiversId && c.SharedMemberId == payment.TransPayersId && c.GroupId == null);
                    if (set != null)
                    {
                        set.TotalAmount = set.TotalAmount + payment.PaidAmount;
                        _Context.Settlement.Attach(set);
                    }
                    else
                    {
                        Settlement settle = new Settlement();
                        settle.PayerId = payment.TransReceiversId;
                        settle.SharedMemberId = payment.TransPayersId;
                        settle.TotalAmount = payment.PaidAmount;
                        _Context.Settlement.Add(settle);
                    }
                }
            }
            else
            {
                var settleData = _Context.Settlement.SingleOrDefault(c => c.PayerId == payment.TransPayersId && c.SharedMemberId == payment.TransReceiversId && c.GroupId == payment.GroupId);
                if (settleData != null)
                {
                    settleData.TotalAmount = settleData.TotalAmount - payment.PaidAmount;
                    _Context.Settlement.Attach(settleData);
                }
                else
                {
                    var set = _Context.Settlement.SingleOrDefault(c => c.PayerId == payment.TransReceiversId && c.SharedMemberId == payment.TransPayersId && c.GroupId == payment.GroupId);
                    if (set != null)
                    {
                        set.TotalAmount = set.TotalAmount + payment.PaidAmount;
                        _Context.Settlement.Attach(set);
                    }
                    else
                    {
                        Settlement settle = new Settlement();
                        settle.SharedMemberId = payment.TransPayersId;
                        settle.PayerId = payment.TransReceiversId;
                        settle.GroupId = payment.GroupId;
                        settle.TotalAmount = payment.PaidAmount;
                        _Context.Settlement.Add(settle);
                    }

                    
                }
            }
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(RecordPaymentAsync)}: " + exp.Message);
            }

            return payment;
        }

        public async Task<List<TransactionResponse>> GetGroupTransactionsAsync(int Groupid)
        {
            //return await _Context.Transactions.Where(c => c.GroupId == Groupid).ToListAsync();

            List<TransactionResponse> transactions = new List<TransactionResponse>();
            
            var tData = _Context.Transactions.Where(c => c.GroupId == Groupid).ToList();
            for(var i = 0; i < tData.Count; i++)
            {
                var trans = new TransactionResponse();
                trans =await GetTransactionAsync(tData[i].TransactionId);
                transactions.Add(trans);
            }

            return transactions;
        }

        public async Task<List<TransactionResponse>> GetIndividualTransactionsAsync(int Userid, int Friendid)
        {
            List<TransactionResponse> transactions = new List<TransactionResponse>();

            var tData = _Context.Transactions.Where(c=>(c.TransPayersId==Userid || c.TransPayersId==Friendid)&& (c.TransReceiversId==Userid||c.TransReceiversId==Friendid)).ToList();
            for (var i = 0; i < tData.Count; i++)
            {
                var trans = new TransactionResponse();
                trans = await GetTransactionAsync(tData[i].TransactionId);
                transactions.Add(trans);
            }
            return transactions;
        }

        public async Task<List<TransactionResponse>> GetAllTransactionsAsync(int Userid)
        {
            //return await _Context.Transactions.Where(c => (c.TransPayersId == Userid || c.TransReceiversId == Userid) || c.groupsId.groupMembers.Any(aa => aa.User_Id == Userid)).ToListAsync();
            List<TransactionResponse> transactions = new List<TransactionResponse>();

            var tData = _Context.Transactions.Where(c => (c.TransPayersId == Userid || c.TransReceiversId == Userid) || c.groupsId.groupMembers.Any(aa => aa.User_Id == Userid)).ToList();
            for (var i = 0; i < tData.Count; i++)
            {
                var trans = new TransactionResponse();
                trans = await GetTransactionAsync(tData[i].TransactionId);
                transactions.Add(trans);
            }
            return transactions;
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DemoDB.Database;
using DemoDB.DataModel;
using DemoDB.Model;
using Microsoft.Extensions.Logging;
using DemoDB.Response;

namespace DemoDB.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly DemoDbContext _Context;
        private readonly ILogger _Logger;

        public BillRepository(DemoDbContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("BillRepository");
        }

       
        public async Task<BillResponse> GetBillAsync(int id)
        {
            BillResponse bill = new BillResponse();
            var payers = new List<BillMemberResponse>();
            var members = new List<BillMemberResponse>();


            var billData = _Context.Bill.SingleOrDefault(c => c.BillId == id);
            bill.BillId = billData.BillId;
            bill.BillName = billData.BillName;
            bill.CreatedDate = billData.CreatedDate;
            bill.GroupId = billData.GroupId.GetValueOrDefault();

            if (bill.GroupId != 0)
            {
                var groupname = _Context.Group.SingleOrDefault(c => c.GroupId == billData.GroupId);
                bill.GroupName = groupname.GroupName;
            }
            var name = _Context.User.SingleOrDefault(c => c.UserId == billData.CreatorId);
            bill.CreatorName = name.UserName;

            var data = _Context.BillMember.Where(c => c.Billid == id).ToList();
            for (var i = 0; i < data.Count; i++)
            {
                var member = _Context.User.SingleOrDefault(c => c.UserId == data[i].SharedMemberId);
                members.Add(new BillMemberResponse(member.UserId,member.UserName, data[i].AmountToPay));
            }

            var payer = _Context.Payer.Where(c => c.BillId == id).ToList();
            for (var i = 0; i < payer.Count; i++)
            {
                var p = _Context.User.SingleOrDefault(c => c.UserId == payer[i].PayerId);
                payers.Add(new BillMemberResponse(p.UserId, p.UserName, payer[i].PaidAmount));
               
            }

            bill.Payers = payers;
            bill.BillMembers = members;
            return  bill;

            //return await _Context.Bill
            //    .Include(c => c.Payers)
            //    .SingleOrDefaultAsync(c => c.BillId == id);
        }

        public async Task<Bill> InsertBillAsync(BillModel bill)
        {
            Bill newBill = new Bill();
            newBill.BillName = bill.BillName;
            newBill.CreatorId = bill.CreatorId;
            newBill.CreatedDate = bill.CreatedDate;
            newBill.GroupId = bill.GroupId;
            _Context.Bill.Add(newBill);


            foreach (var person in bill.Payer)
            {
                Payer payer = new Payer();
                payer.BillId = newBill.BillId;
                payer.PayerId = person.Id;
                payer.PaidAmount = person.Amount;
                _Context.Payer.Add(payer);
            }


            foreach (var person in bill.SharedMember)
            {
                BillMember member = new BillMember();
                member.Billid = newBill.BillId;
                member.SharedMemberId = person.Id;
                member.AmountToPay = person.Amount;
                _Context.BillMember.Add(member);
            }

            foreach (var data in bill.SettleModels)
            {
                if (data.GroupId == null)
                {
                    var settle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.PayerId && c.SharedMemberId == data.SharedMemberId && c.GroupId==null);
                    if (settle != null)
                    {
                        settle.TotalAmount = settle.TotalAmount + data.TotalAmount;
                        _Context.Settlement.Attach(settle);
                    }
                    else
                    {
                        var setle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.SharedMemberId && c.SharedMemberId == data.PayerId && c.GroupId == null);
                        if (setle != null)
                        {
                            setle.TotalAmount = setle.TotalAmount - data.TotalAmount;
                            _Context.Settlement.Attach(setle);
                        }
                        else
                        {
                            var newSettle = new Settlement();
                            newSettle.PayerId = data.PayerId;
                            newSettle.SharedMemberId = data.SharedMemberId;
                            newSettle.TotalAmount = data.TotalAmount;
                            _Context.Settlement.Add(newSettle);
                        }
                    }
                }
                else
                {
                    var settle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.PayerId && c.SharedMemberId == data.SharedMemberId && c.GroupId == data.GroupId);
                    if (settle != null)
                    {
                        settle.TotalAmount = settle.TotalAmount + data.TotalAmount;
                        _Context.Settlement.Attach(settle);
                    }
                    else
                    {
                        var setle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == data.SharedMemberId && c.SharedMemberId == data.PayerId && c.GroupId == data.GroupId);
                        if (setle != null)
                        {
                            setle.TotalAmount = setle.TotalAmount - data.TotalAmount;
                            _Context.Settlement.Attach(setle);
                        }
                        else
                        {
                            var newSettle = new Settlement();
                            newSettle.PayerId = data.PayerId;
                            newSettle.SharedMemberId = data.SharedMemberId;
                            newSettle.TotalAmount = data.TotalAmount;
                            newSettle.GroupId = data.GroupId;
                            _Context.Settlement.Add(newSettle);
                        }
                    }
                }
            }
            //if (newBill.GroupId == null)
            //{
            //    foreach (var person in bill.Payer)
            //    {
            //        foreach (var member in bill.SharedMember)
            //        {
            //            if (person.Id != member.Id)
            //            {
            //                Settlement settlement = new Settlement();
            //                settlement.PayerId = member.Id;
            //                settlement.SharedMemberId = person.Id;
            //                settlement.TotalAmount = member.Amount;

            //                var settle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == settlement.PayerId && c.SharedMemberId == settlement.SharedMemberId && c.GroupId==null);
            //                if (settle != null)
            //                {
            //                    settle.TotalAmount = settle.TotalAmount + settlement.TotalAmount;
            //                    _Context.Settlement.Attach(settle);
            //                }
            //                else
            //                {
            //                    var setle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == settlement.SharedMemberId && c.SharedMemberId == settlement.PayerId && c.GroupId == null);
            //                    if (setle != null)
            //                    {
            //                        setle.TotalAmount = setle.TotalAmount - settlement.TotalAmount;
            //                        _Context.Settlement.Attach(setle);
            //                    }
            //                    else
            //                    {
            //                        _Context.Settlement.Add(settlement);
            //                    }
            //                }

            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var person in bill.Payer)
            //    {
            //        foreach (var member in bill.SharedMember)
            //        {
            //            if (person.Id != member.Id)
            //            {
            //                Settlement settlement = new Settlement();
            //                settlement.GroupId = newBill.GroupId;
            //                settlement.PayerId = member.Id;
            //                settlement.SharedMemberId = person.Id;
            //                settlement.TotalAmount = member.Amount;

            //                var settle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == settlement.PayerId && c.SharedMemberId == settlement.SharedMemberId && c.GroupId == settlement.GroupId);
            //                if (settle != null)
            //                {
            //                    settle.TotalAmount = settle.TotalAmount + settlement.TotalAmount;
            //                    _Context.Settlement.Attach(settle);
            //                }
            //                else
            //                {
            //                    var setle = await _Context.Settlement.SingleOrDefaultAsync(c => c.PayerId == settlement.SharedMemberId && c.SharedMemberId == settlement.PayerId && c.GroupId == settlement.GroupId);
            //                    if (setle != null)
            //                    {
            //                        setle.TotalAmount = setle.TotalAmount - settlement.TotalAmount;
            //                        _Context.Settlement.Attach(setle);
            //                    }
            //                    else
            //                    {
            //                        _Context.Settlement.Add(settlement);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(InsertBillAsync)}: " + exp.Message);
            }

            return newBill;
        }

        public async Task<bool> DeleteBillAsync(int id)
        {
            var bill = await _Context.Bill.SingleOrDefaultAsync(c => c.BillId == id);
            _Context.Remove(bill);


            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteBillAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<List<BillResponse>> GetAllExpenses(int id)
        {

            //return await _Context.Bill
            //    .Where(c=>c.BillMembers.Any(aa=>aa.SharedMemberId==id) ||c.CreatorId==id || c.Payers.Any(aa=>aa.PayerId==id))
            //    .ToListAsync();

            List<BillResponse> bills = new List<BillResponse>();

            var billData = _Context.Bill.Where(c => c.BillMembers.Any(aa => aa.SharedMemberId == id) || c.CreatorId == id || c.Payers.Any(aa => aa.PayerId == id)).ToList();

            for (var i = 0; i < billData.Count; i++)
            {
                var bill = new BillResponse();
                bill = await GetBillAsync(billData[i].BillId);
                bills.Add(bill);
            }
            
            return bills;

        }

        public async Task<List<BillResponse>> GetIndividualExpenses(int Userid, int Friendid)
        {
            //return await _Context.Bill
            //    .Where(c => c.BillMembers.Any(aa => aa.SharedMemberId == Friendid || aa.SharedMemberId==Userid) && c.Payers.Any(aa => aa.PayerId == Userid || aa.PayerId==Friendid))
            //    .ToListAsync();
            List<BillResponse> bills = new List<BillResponse>();
            var billData = _Context.Bill.Where(c => c.BillMembers.Any(aa => aa.SharedMemberId == Friendid || aa.SharedMemberId == Userid) && c.Payers.Any(aa => aa.PayerId == Userid || aa.PayerId == Friendid)).ToList();
            
            for (var i = 0; i < billData.Count; i++)
            {
                var bill = new BillResponse();
                bill = await GetBillAsync(billData[i].BillId);
                bills.Add(bill);
            }
            
            return bills;

        }

        public async Task<List<BillResponse>> GetGroupExpenses(int Groupid)
        {
            //return await _Context.Bill
            //    .Where(c => c.GroupId == Groupid)
            //    .Include(c => c.Payers)
            //    .Include(c => c.BillMembers)
            //    .ToListAsync();
            //var count = _Context.Bill.Where(c => c.GroupId == Groupid).Count();

            List<BillResponse> bills = new List<BillResponse>();
            var billData = _Context.Bill.Where(c => c.GroupId == Groupid).ToList();

            for(var i = 0; i < billData.Count; i++)
            {
                var bill = new BillResponse();
                bill = await GetBillAsync(billData[i].BillId);
                bills.Add(bill);
            }
            
            return bills;
        }

        public async Task<bool> UpdateBillAsync(Bill bill)
        {
            _Context.Bill.Attach(bill);
            _Context.Entry(bill).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(UpdateBillAsync)}: " + exp.Message);
            }
            return false;
        }
    }
}

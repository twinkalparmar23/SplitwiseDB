using DemoDB.Database;
using DemoDB.Model;
using DemoDB.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Repository
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly DemoDbContext _Context;
        private readonly ILogger _Logger;

        public SettlementRepository(DemoDbContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("SettlementRepository");
        }


        public async  Task<SettlementResponse> GetSettlementAsync(int id)
        {
            SettlementResponse settlement = new SettlementResponse();

            var sData = await _Context.Settlement.SingleOrDefaultAsync(c => c.SettlementId == id);
            settlement.Id = sData.SettlementId;
           // var Pname = await _Context.User.SingleOrDefaultAsync(c => c.UserId == sData.PayerId);
            settlement.Payer_id=sData.PayerId;
           // var Rname = await _Context.User.SingleOrDefaultAsync(c => c.UserId == sData.SharedMemberId);
            settlement.Receiver_id = sData.SharedMemberId;

            if (sData.GroupId != null)
            {
               // var Gname = await _Context.Group.SingleOrDefaultAsync(c => c.GroupId == sData.GroupId);
                settlement.Group_id = sData.GroupId.GetValueOrDefault();
            }

            settlement.Amount = sData.TotalAmount;

            return settlement;
        }

        public async Task<List<SettlementResponse>> GetSettlementAsync(int Userid, int Friendid)
        {
            //return await _Context.Settlement.Where(c => (c.PayerId == Userid && c.SharedMemberId == Friendid)||(c.PayerId == Friendid && c.SharedMemberId == Userid)).ToListAsync();

            List<SettlementResponse> settlements = new List<SettlementResponse>();
            var sData= await _Context.Settlement.Where(c => (c.PayerId == Userid && c.SharedMemberId == Friendid) || (c.PayerId == Friendid && c.SharedMemberId == Userid)).ToListAsync();

            for(var i = 0; i < sData.Count; i++)
            {
                var settle = new SettlementResponse();
                settle = await GetSettlementAsync(sData[i].SettlementId);
                settlements.Add(settle);
            }

            return settlements;

        }


        public async Task<List<SettlementResponse>> GetGroupSettlementAsync(int Groupid)
        {
            List<SettlementResponse> settlements = new List<SettlementResponse>();
            var sData = await _Context.Settlement.Where(c => c.GroupId==Groupid).ToListAsync();

            for (var i = 0; i < sData.Count; i++)
            {
                var settle = new SettlementResponse();
                settle = await GetSettlementAsync(sData[i].SettlementId);
                settlements.Add(settle);
            }

            return settlements;
        }

    }
}

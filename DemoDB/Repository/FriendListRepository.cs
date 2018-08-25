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
    public class FriendListRepository :IFriendListRepository
    {
        private readonly DemoDbContext _Context;
        private readonly ILogger _Logger;

        public FriendListRepository(DemoDbContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("FriendListRepository");
        }

        public async Task<List<FriendResponse>> GetAllFriendsAsync(int id)
        {
            return await _Context.FriendList
                     .Where(c => c.UserId == id)
                    .Select(c => new FriendResponse()
                    {
                        
                        Id = c.FriendId,
                        Name= c.friend.UserName,
                        Email=c.friend.Email

                    }
                ).ToListAsync();

        }

        public async Task<FriendList> InsertFriendAsync(int Userid, int Friendid)
        {
            var member = _Context.FriendList.SingleOrDefault(c => c.UserId == Userid && c.FriendId == Friendid);

            if(member== null)
            {
                FriendList newFriend = new FriendList
                {
                    UserId = Userid,
                    FriendId = Friendid
                };

                _Context.FriendList.Add(newFriend);
                await _Context.SaveChangesAsync();
                return newFriend;
            }

            else
            {
                _Context.FriendList.Attach(member);
                await _Context.SaveChangesAsync();
                return member;
            }
          
        }

        public async Task<bool> DeleteFriendAsync(int fid, int uid)
        {
            var data = _Context.FriendList.SingleOrDefault(c => c.UserId == uid && c.FriendId == fid);
            _Context.Remove(data);

            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteFriendAsync)}: " + exp.Message);
            }
            return false;
        }
    }
}

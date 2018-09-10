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

        public async Task<FriendResponse> GetFriendAsync(int id)
        {
            //return await _Context.User
            //    .SingleOrDefaultAsync(c => c.UserId == id);

            var userData = await _Context.User.SingleOrDefaultAsync(c => c.UserId == id);
            var user = new FriendResponse();
            user.UserId = userData.UserId;
            user.UserName = userData.UserName;
            user.Email = userData.Email;
            return user;
        }

        public async Task<List<FriendResponse>> GetAllFriendsAsync(int id)
        {
            //return await _Context.FriendList
            //         .Where(c => c.UserId == id )
            //        .Select(c => new FriendResponse()
            //        {

            //            UserId = c.FriendId,
            //            UserName= c.friend.UserName,
            //            Email=c.friend.Email

            //        }
            //    ).ToListAsync();

            List<FriendResponse> friends = new List<FriendResponse>();

            var fData = await _Context.FriendList.Where(c => c.UserId == id || c.FriendId == id).ToListAsync();
            for(var i = 0; i < fData.Count; i++)
            {
                if (fData[i].UserId == id)
                {
                    var x = new FriendResponse();
                    x.UserId = fData[i].FriendId;
                    var data = await _Context.User.SingleOrDefaultAsync(c => c.UserId == x.UserId);
                    x.UserName = data.UserName;
                    x.Email = data.UserName;
                    friends.Add(x);
                }
                else
                {
                    var x = new FriendResponse();
                    x.UserId = fData[i].UserId;
                    var data = await _Context.User.SingleOrDefaultAsync(c => c.UserId == x.UserId);
                    x.UserName = data.UserName;
                    x.Email = data.UserName;
                    friends.Add(x);
                }
            }
            return friends;

        }

        public async Task<FriendList> InsertFriendAsync(int id, string userName, string email)
        {
            var userExist = _Context.User.SingleOrDefault(c => c.UserName == userName && c.Email == email);
            if (userExist != null)
            {
                var member = _Context.FriendList.SingleOrDefault(c => c.UserId == id && c.FriendId == userExist.UserId);
                if (member == null)
                {
                    var memExist= _Context.FriendList.SingleOrDefault(c => c.UserId==userExist.UserId && c.FriendId==id);
                    if (memExist == null)
                    {
                        FriendList newFriend = new FriendList
                        {
                            UserId = id,
                            FriendId = userExist.UserId
                        };
                        _Context.FriendList.Add(newFriend);
                        await _Context.SaveChangesAsync();
                        return newFriend;
                    }
                    else
                    {
                        _Context.FriendList.Attach(memExist);
                        await _Context.SaveChangesAsync();
                        return memExist;
                    }
                }
                else
                {
                    _Context.FriendList.Attach(member);
                    await _Context.SaveChangesAsync();
                    return member;
                }
            }
            else
            {
                FriendList notExist = new FriendList
                {
                    UserId = 0,
                    FriendId = 0
                };
                return notExist;
            }
            //var member = _Context.FriendList.SingleOrDefault(c => c.UserId == Userid && c.FriendId == Friendid);

            //if(member== null)
            //{
            //    var exist = _Context.FriendList.SingleOrDefault(c => c.FriendId == Userid && c.UserId == Friendid);
            //    if (exist == null)
            //    {
            //        FriendList newFriend = new FriendList
            //        {
            //            UserId = Userid,
            //            FriendId = Friendid
            //        };

            //        _Context.FriendList.Add(newFriend);
            //        await _Context.SaveChangesAsync();
            //        return newFriend;
            //    }
            //    else
            //    {
            //        _Context.FriendList.Attach(exist);
            //        await _Context.SaveChangesAsync();
            //        return  exist;
            //    }
            //}

            //else
            //{
            //    _Context.FriendList.Attach(member);
            //    await _Context.SaveChangesAsync();
            //    return member;
            //}
          
        }

        public async Task<bool> DeleteFriendAsync(int fid, int uid)
        {
            var data = _Context.FriendList.SingleOrDefault(c => c.UserId == uid && c.FriendId == fid);
            if (data == null)
            {
                var fData = _Context.FriendList.SingleOrDefault(c => c.UserId == fid && c.FriendId == uid);
                _Context.Remove(fData);
            }
            else
            {
                _Context.Remove(data);
            }
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

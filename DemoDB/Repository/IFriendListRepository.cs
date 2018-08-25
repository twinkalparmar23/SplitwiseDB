using DemoDB.Model;
using DemoDB.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Repository
{
    public interface IFriendListRepository
    {
       
        Task<List<FriendResponse>> GetAllFriendsAsync(int id);
        Task<FriendList> InsertFriendAsync(int Userid, int Friendid);
        Task<bool> DeleteFriendAsync(int uid, int fid);
        
    }
}

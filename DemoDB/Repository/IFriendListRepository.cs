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
        Task<FriendResponse> GetFriendAsync(int id);
        Task<List<FriendResponse>> GetAllFriendsAsync(int id);
        Task<FriendList> InsertFriendAsync(int id,string userName, string email);
        Task<bool> DeleteFriendAsync(int uid, int fid);
        
    }
}

using DemoDB.DataModel;
using DemoDB.Model;
using DemoDB.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Repository
{
    public interface IGroupRepository
    {
        Task<Group> InsertGroupAsync(GroupModel group);
        
        Task<GroupResponse> GetGroupAsync(int id);
        Task<List<GroupResponse>> GetGroupsAsync(int id);

        Task<bool> UpdateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int id);
        Task<GroupMember> InsertGroupMemberAsync(int Groupid, int Memberid);
        Task<bool> DeleteGroupMemberAsync(int Groupid, int Memberid);
    }
}

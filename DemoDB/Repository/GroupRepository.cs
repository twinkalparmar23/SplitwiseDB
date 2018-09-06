using DemoDB.Database;
using DemoDB.DataModel;
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
    public class GroupRepository :IGroupRepository
    {
        private readonly DemoDbContext _Context;
        private readonly ILogger _Logger;

        public GroupRepository(DemoDbContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("FriendListRepository");
        }


        public async Task<GroupResponse> GetGroupAsync(int id)
        {
            //return await _Context.Group
            //    .Include(c=>c.groupMembers)
            //    .Where(c=>c.GroupId==id)
            //   .SingleOrDefaultAsync(c => c.GroupId == id);

            GroupResponse group = new GroupResponse();
            List<MemberResponse> members = new List<MemberResponse>();

            var groupData = _Context.Group.SingleOrDefault(c => c.GroupId == id);
            group.GroupId = groupData.GroupId;
            group.GroupName = groupData.GroupName;
            group.CreatedDate = groupData.CreatedDate;
            group.CreatorId = groupData.CreatorId;

            var name = _Context.User.SingleOrDefault(c => c.UserId == groupData.CreatorId);
            group.CreatorName = name.UserName;

            var memberData = _Context.GroupMember.Where(c => c.Group_Id == id).ToList();
            for (var i = 0; i < memberData.Count; i++)
            {
                var member = _Context.User.SingleOrDefault(c => c.UserId == memberData[i].User_Id);
                members.Add(new MemberResponse(member.UserId,member.UserName));
               
            }
            group.Members = members;
            return group;
        }

        public async Task<List<GroupResponse>> GetGroupsAsync(int id)
        {
            //return await _Context.GroupMember
            //    .Where(c => c.User_Id == id)
            //    .Include(c=>c.Group)
            //    .Select(c => new GroupListResponse()
            //    {
            //        GroupId=c.Group_Id,
            //        GroupName=c.Group.GroupName
            //    }
            //    ).ToListAsync();

            List<GroupResponse> groups = new List<GroupResponse>();

            var groupData = _Context.Group.Where(c => c.groupMembers.Any(aa => aa.User_Id == id)).ToList();
            for(var i = 0; i < groupData.Count; i++)
            {
                var group = new GroupResponse();
                group = await GetGroupAsync(groupData[i].GroupId);
                groups.Add(group);
            }

            return groups;
        }

        public async Task<Group> InsertGroupAsync(GroupModel group)
        {
            Group grp = new Group();
            grp.GroupName = group.GroupName;
            grp.CreatedDate = group.CreatedDate;
            grp.CreatorId = group.CreatorId;
            _Context.Group.Add(grp);

            foreach(var member in group.Members)
            {
                GroupMember grpMember = new GroupMember();
                grpMember.Group_Id = grp.GroupId;
                grpMember.User_Id = member;
                _Context.GroupMember.Add(grpMember);
            }
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(InsertGroupAsync)}: " + exp.Message);
            }

            return grp;
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            var group = await _Context.Group.SingleOrDefaultAsync(c => c.GroupId == id);
            _Context.Remove(group);


            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteGroupAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<GroupMember> InsertGroupMemberAsync(int Groupid, int Memberid)
        {
            var member = _Context.GroupMember.SingleOrDefault(c => c.Group_Id == Groupid && c.User_Id == Memberid);

            if (member == null)
            {

                GroupMember newMember = new GroupMember
                {
                    Group_Id = Groupid,
                    User_Id = Memberid
                };

                _Context.GroupMember.Add(newMember);
                await _Context.SaveChangesAsync();
                return  newMember;
            }
            else
            {
                _Context.GroupMember.Attach(member);
                await _Context.SaveChangesAsync();
                return member;
            }

            //try
            //{
            //    await _Context.SaveChangesAsync();
            //}
            //catch (System.Exception exp)
            //{
            //    _Logger.LogError($"Error in {nameof(InsertGroupMemberAsync)}: " + exp.Message);
            //}

            //return newMember;
        }

        public async Task<bool> DeleteGroupMemberAsync(int Groupid, int Memberid)
        {
            var data = _Context.GroupMember.SingleOrDefault(c => c.Group_Id==Groupid && c.User_Id==Memberid);
            _Context.Remove(data);

            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteGroupMemberAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> UpdateGroupAsync(Group group)
        {
           
            _Context.Group.Attach(group);
            _Context.Entry(group).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(UpdateGroupAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<List<GroupResponse>> GetCommenGroupsAsync(int Userid, int Friendid)
        {
            List<GroupResponse> groups = new List<GroupResponse>();
            var groupData = _Context.Group.Where(c => c.groupMembers.Any(aa => aa.User_Id == Userid)).Include(c=>c.groupMembers).ToList();

            var gData = groupData.Where(c => c.groupMembers.Any(aa => aa.User_Id == Friendid)).ToList();

            for (var i = 0; i < gData.Count; i++)
            {
                var group = new GroupResponse();
                group = await GetGroupAsync(gData[i].GroupId);
                groups.Add(group);
            }

            return groups;
        }
    }
}

using DemoDB.Database;
using DemoDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Repository
{
    public class UserRepository :IUserRepository
    {
        private readonly DemoDbContext _Context;
        private readonly ILogger _Logger;

        public UserRepository(DemoDbContext context, ILoggerFactory loggerFactory)
        {
            _Context = context;
            _Logger = loggerFactory.CreateLogger("UserRepository");
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _Context.User.ToListAsync();
            
        }

        public async Task<User> GetUserAsync(int id)
        {
            //var result = (from p in _Context.User
            //              join e in _Context.FriendList
            //              on p.UserId equals e.UserId
            //              select e.FriendId).ToListAsync();

            return await _Context.User
                //.Include(c => c.Friends)

                .SingleOrDefaultAsync(c => c.UserId == id);
        }

        public async Task<User> InsertUserAsync(User user)
        {
            _Context.User.Add(user);
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(InsertUserAsync)}: " + exp.Message);
            }

            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            //Will update all properties of the user
            _Context.User.Attach(user);
            _Context.Entry(user).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(UpdateUserAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _Context.User
                .SingleOrDefaultAsync(c => c.UserId == id);
            _Context.Remove(user);

            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DeleteUserAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {

            List<User> users = await _Context.User.ToListAsync();
            var user=users.SingleOrDefault(c => c.Email == email && c.Password == password);
            return user;
            //foreach (var user in users)
            //{
            //    if (user.Email == email && user.Password == password)
            //    { return true;}
            //}
            //return false;

            //return await _Context.User.SingleOrDefault(c =>( (c.Email == email )&& ( c.Password == password)));



        }
    }
}

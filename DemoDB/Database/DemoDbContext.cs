using DemoDB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Database
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            
        }

        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupMember> GroupMember {get; set;}
        public DbSet<Bill> Bill { get; set; }
        public DbSet<BillMember> BillMember { get; set; }
        public DbSet<GroupPayer> GroupPayer { get; set; }
        public DbSet<IndividualPayer> IndividualPayer { get; set; }
        public DbSet<FriendList> FriendList { get; set; }
        public DbSet<Settlement> Settlement { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

    }
}

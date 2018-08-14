using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class Group
    {
       [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }


        public int CreatorId { get; set; }
        public User User { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("Group_Id")]
        public ICollection<GroupMember> groupMembers { get; set; }

        [ForeignKey("GroupId")]
        public ICollection<Bill> Bills { get; set; }

        [ForeignKey("GroupId")]
        public ICollection<Transaction> Transactions { get; set; }
    }
}

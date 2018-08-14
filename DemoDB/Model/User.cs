using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("User_Id")]
        public ICollection<GroupMember> groupMembers { get; set; }

        [ForeignKey("CreatorId")]
        public ICollection<Bill> Bills { get; set; }

        [ForeignKey("CreatorId")]
        public ICollection<Group> Groups { get; set; }

        [ForeignKey("SharedMemberId")]
        public ICollection<BillMember> BillMembers { get; set; }

        [ForeignKey("PayerId")]
        public ICollection<GroupPayer> GroupPayers { get; set; }

        [ForeignKey("PayerId")]
        public ICollection<IndividualPayer> IndividualPayers { get; set; }

        [InverseProperty("Payer")]
        public List<Settlement> Payers { get; set; }

        [InverseProperty("SharedMember")]
        public List<Settlement> SharedMembers { get; set; }

        [InverseProperty("user")]
        public List<FriendList> Users { get; set; }

        [InverseProperty("friend")]
        public List<FriendList> Friends { get; set; }

        [InverseProperty("payers")]
        public List<Transaction> payers { get; set; }

        [InverseProperty("receivers")]
        public List<Transaction> Receivers { get; set; }
    }
}

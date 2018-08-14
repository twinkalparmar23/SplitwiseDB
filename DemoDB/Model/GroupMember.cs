using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class GroupMember
    {
        [Key]
        public int GroupMemberId { get; set; }

        //[ForeignKey("GroupId")]
        public int Group_Id { get; set; }
        //[ForeignKey("GroupId")]
        public Group Group { get; set; }

        //[ForeignKey("UserId")]
        public int User_Id { get; set; }
        //[ForeignKey("UserId")]
        public User User { get; set; }
    }
}

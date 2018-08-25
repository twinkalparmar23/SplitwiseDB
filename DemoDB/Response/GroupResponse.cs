using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Response
{
    public class GroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string CreatorName { get; set; } 
        public DateTime CreatedDate { get; set; }
        public List<MemberResponse> Members { get; set; }
    }
}

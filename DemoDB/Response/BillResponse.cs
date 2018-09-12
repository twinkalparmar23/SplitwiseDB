using DemoDB.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Response
{
    public class BillResponse
    {
        public int BillId { get; set; }
        public string BillName { get; set; }
        public string CreatorName { get; set; }
        public decimal Amount { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<BillMemberResponse> Payers { get; set; }
        public List<BillMemberResponse> BillMembers { get; set; }

    }
}

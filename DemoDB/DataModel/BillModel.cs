using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.DataModel
{
    public class BillModel
    {
        public string BillName { get; set; }

        public int CreatorId { get; set; }
       
        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }

        public int? GroupId { get; set; }
        public List<PayerModel> Payer { get; set; }
        public List<PayerModel> SharedMember { get; set; }

    }
}

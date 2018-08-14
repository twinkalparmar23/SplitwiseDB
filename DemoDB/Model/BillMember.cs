using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class BillMember
    {
        [Key]
        public int BillMemberId { get; set; }

        public int Billid { get; set; }
        public Bill Bill { get; set; }

        public int SharedMemberId { get; set; }
        public User User { get; set; }

        public decimal AmountToPay { get; set; }
    }
}

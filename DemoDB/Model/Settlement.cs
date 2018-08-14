using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class Settlement
    {
        [Key]
        public int SettlementId { get; set; }

        public int PayerId { get; set; }
        public User Payer { get; set; }

        public int SharedMemberId { get; set; }
        public User SharedMember { get; set; }
    }
}

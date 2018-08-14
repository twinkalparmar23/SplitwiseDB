using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class IndividualPayer
    {
        [Key]
        public int IndividualPayerid { get; set; }

        public int BillId { get; set; }
        public Bill Bill { get; set; }

        public int PayerId { get; set; }
        public User User { get; set; }

        public decimal PaidAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

       // public int PayerId { get; set; }
        public User payers { get; set; }

       // public int ReceiverId { get; set; }
        public User receivers { get; set; }

        public int GroupId { get; set; }
        public Group groupsId { get; set; }

        public decimal PaidAmount { get; set; }
    }
}

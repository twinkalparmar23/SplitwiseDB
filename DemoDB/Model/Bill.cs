using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Model
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        public string BillName { get; set; }

        public int CreatorId { get; set; }
        public User User { get; set; }

        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        [ForeignKey("BillId")]
        public ICollection<GroupPayer> GroupPayers { get; set; }

        [ForeignKey("BillId")]
        public ICollection<IndividualPayer> IndividualPayers { get; set; }
    }
}

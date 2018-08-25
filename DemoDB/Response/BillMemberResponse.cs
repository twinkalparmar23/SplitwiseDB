using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Response
{
    public class BillMemberResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public BillMemberResponse()
        {

        }

        public BillMemberResponse(int id, string name, decimal amount)
        {
            this.Id = id;
            this.Name = name;
            this.Amount = amount;
        }
    }
}

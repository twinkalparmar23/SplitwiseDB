using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Response
{
    public class SettlementResponse
    {
        public int Id { get; set; }
        public string Payer { get; set; }
        public string Receiver { get; set; }
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
    }
}

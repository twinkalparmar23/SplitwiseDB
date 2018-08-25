using DemoDB.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDB.Response
{
    public class ApiUserResponse
    {
        public bool Status { get; set; }
        public User User { get; set; }
        public ModelStateDictionary ModelState { get; set; }
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fastbank2.Api.Model;

namespace Fastbank2.UI.controllers
{
    public class BankController : Controller 
    {

        // GET api/values
        [HttpGet]
        public IEnumerable<Bank> Main() 
        {
            return new Bank[] { 
                new Bank {Id = 1, Name = "Bank of America" }, 
                new Bank {Id = 2, Name = "Citi" } };
        }
    }
}


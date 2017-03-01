using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fastbank2.Api.Interfaces;
using Fastbank2.Api.Model;
using Microsoft.EntityFrameworkCore;
using Fastbank2.Api.Repo;
using System.Collections.Generic;
using System;

namespace PrototypeApi.Controllers
{
    [Route("api/")]
    public class FastBankApiController : Controller
    {
        IUnitofWork _contxt = null;
        public FastBankApiController(IUnitofWork contxt)
        {
            _contxt = contxt;
        }
 
        [HttpGet("Bank/{bankId:int}/Users")]
        public IActionResult Get(int bankId)
        {
            Console.WriteLine("bank Id: " + bankId.ToString());
            ApiHelper hlpr = new ApiHelper(_contxt);
            var response = hlpr.GetUsers(bankId);
            return Ok(response);
            //return new string[] { "value1_" + bankId, "value2_" + bankId };
        }

        [HttpGet("{bankId}")]
        public IActionResult Get1(int bankId)
        {
            ApiHelper hlpr = new ApiHelper(_contxt);
            var response = hlpr.GetUsers(bankId);
            return Ok(response);
            //return new string[] { "value1_" + bankId, "value2_" + bankId };
        }

        [HttpGet()]
        [Route("Bank/{bankId}/Users")]
        public IActionResult GetUsersOfBank(int bankId)
        {
            ApiHelper hlpr = new ApiHelper(_contxt);
            var response = hlpr.GetUsers(bankId);
            return Ok(response);
        }

        [HttpGet()]
        [Route("User/{userId:int}")]
        public IActionResult GetUsersById(int userId)
        {
            ApiHelper hlpr = new ApiHelper(_contxt);
            var response = hlpr.GetUser(userId);
            return Ok(response);
        }
    }
}
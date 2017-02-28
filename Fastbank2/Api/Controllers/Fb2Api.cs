using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fastbank2.Api.Interfaces;
using Fastbank2.Api.Model;
using Microsoft.EntityFrameworkCore;
using Fastbank2.Api.Repo;

namespace PrototypeApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        IUnitofWork _contxt = null;
        public UsersController(IUnitofWork contxt)
        {
            _contxt = contxt;
        }
 
        public IActionResult GetUsers(int bankId)
        {
            ApiHelper hlpr = new ApiHelper(_contxt);
            var response = hlpr.GetUsers(bankId);
            return Ok(response);
        }
    }
}
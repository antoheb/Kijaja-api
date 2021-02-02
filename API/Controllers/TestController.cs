using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [Route("/api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public TestController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<Item>>> Index()
        {
            var items = await _dataContext.Items.ToListAsync();
            return items;
        }
    }
}
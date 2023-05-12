using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkoMon.DomainModel.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EkoMon.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly EntityContext entityContext;
        public ApiController(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }
        
        [HttpGet]
        public async Task<List<Location>> GetInfo()
        {
            return entityContext.Locations.ToList();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansAW.Grains.Interfaces;
using OrleansAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController
    {
        private IClusterClient _client;

        public HomeController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
           var startupGrain = _client.GetGrain<IStartupGrain>(0);
            await startupGrain.KeepAlive();
            return "Your API is running!";
        }        
    }
}

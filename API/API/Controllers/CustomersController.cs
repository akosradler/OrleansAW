using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansAW.Grains.Interfaces;
using OrleansAW.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController
    {
        private IClusterClient _client;

        public CustomersController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("/")]
        public async Task<ActionResult<string>> GetCustomers()
        {
            var customersGrain = _client.GetGrain<ICustomersGrain>(0);
            var customers = await customersGrain.GetCustomers();
            return JsonConvert.SerializeObject(customers);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<string>> GetCustomer(int id)
        {
            var customerGrain = _client.GetGrain<ICustomerGrain>(id);
            var customer = await customerGrain.GetCustomer();
            return JsonConvert.SerializeObject(customer);
        }

        [HttpPut]
        [Route("/")]
        public async Task<ActionResult<bool>> AddCustomer([FromBody] Customer customer)
        {
            var customerManagerGrain = _client.GetGrain<ICustomerManagerGrain>(0);
            return await customerManagerGrain.AddCustomer(customer);
        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Concurrency;
using OrleansAW.Grains.Interfaces;
using OrleansAW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansAW.Grains
{
    class CustomersGrain : Grain, ICustomersGrain
    {
        private IConfiguration _configuration;
        private int[] _customerIds;

        public CustomersGrain(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override Task OnActivateAsync()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("AzureDatabase")))
            {
                _customerIds = db.Query<int[]>($"Select CustomerID from SalesLT.Customer").FirstOrDefault();
            }
            return base.OnActivateAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var grainCallTasks = _customerIds.Select(customerId =>
                GrainFactory.GetGrain<ICustomerGrain>(customerId).GetCustomer());

            return await Task.WhenAll(grainCallTasks);
        }
    }
}

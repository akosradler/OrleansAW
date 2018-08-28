using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private IEnumerable<int> _customerIds;
        private ILogger _logger;

        public CustomersGrain(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
        }

        public override Task OnActivateAsync()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("AzureDatabase")))
            {
                try
                {
                    _customerIds = db.Query<int>($"Select CustomerID from SalesLT.Customer");
                }
                catch (Exception e)
                {
                    _logger.LogError($"SQL Error: {e.Message}");
                }
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

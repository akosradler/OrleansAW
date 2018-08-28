using Orleans;
using OrleansAW.Grains.Interfaces;
using System.Threading.Tasks;
using OrleansAW.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace OrleansAW.Grains
{
    public class CustomerGrain : Grain, ICustomerGrain
    {
        private Customer _customer;
        private IConfiguration _configuration;
        private ILogger _logger;

        public CustomerGrain(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        public override Task OnActivateAsync()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("AzureDatabase")))
            {
                try
                {
                    _customer = db.Query<Customer>($"Select * from SalesLT.Customer where CustomerID = {this.GetPrimaryKeyLong()}").FirstOrDefault();
                }
                catch(Exception e)
                {
                    _logger.LogError($"SQL Error: {e.Message}");
                }
            }
            return base.OnActivateAsync();
        }

        public async Task<Customer> GetCustomer()
        {
            return await Task.FromResult(_customer);
        }
    }
}

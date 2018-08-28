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
using System.Text;
using System.Threading.Tasks;

namespace OrleansAW.Grains
{
    [StatelessWorker]
    public class CustomerManagerGrain : Grain, ICustomerManagerGrain
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        public CustomerManagerGrain(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
        }

        public async Task<bool> AddCustomer(Customer customer)
        {
            int rowsChanged = 0;
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("AzureDatabase")))
            {
                try
                {
                    rowsChanged = await db.ExecuteAsync($"INSERT INTO SalesLT.Customer (Title, FirstName, LastName, EmailAddress, Phone, PasswordHash, PasswordSalt) " +
                        $"VALUES('Mr.', \'{customer.FirstName}\', \'{customer.LastName}\', \'{customer.EmailAddress}\', \'{customer.Phone}\', '1234', '1234')");
                }
                catch(Exception e)
                {
                    _logger.LogError($"SQL Error: {e.Message}");
                }
            }
            return await Task.FromResult(rowsChanged == 1);
        }
    }
}

using Orleans;
using OrleansAW.Grains.Interfaces;
using System.Threading.Tasks;
using OrleansAW.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace OrleansAW.Grains
{
    public class CustomerGrain : Grain, ICustomerGrain
    {
        private Customer _customer;
        private IConfiguration _configuration;

        public CustomerGrain(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public override Task OnActivateAsync()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("AzureDatabase")))
            {
                _customer = (db.Query<Customer>($"Select * from SalesLT.Customer where CustomerID = {this.GetPrimaryKeyLong()}")).FirstOrDefault();
            }
            return base.OnActivateAsync();
        }

        public async Task<Customer> GetCustomer()
        {
            return await Task.FromResult(_customer);
        }
    }
}

using Orleans;
using OrleansAW.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansAW.Grains.Interfaces
{
    public interface ICustomerGrain : IGrainWithIntegerKey
    {
        Task<Customer> GetCustomer();
    }
}

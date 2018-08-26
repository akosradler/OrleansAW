using Orleans;
using OrleansAW.Models;
using System.Threading.Tasks;

namespace OrleansAW.Grains.Interfaces
{
    public interface ICustomerManagerGrain : IGrainWithIntegerKey
    {
        Task<bool> AddCustomer(Customer customer);
    }
}

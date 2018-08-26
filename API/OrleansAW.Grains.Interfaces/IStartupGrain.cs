using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansAW.Grains.Interfaces
{
    public interface IStartupGrain : IGrainWithIntegerKey
    {
        Task KeepAlive();
    }
}

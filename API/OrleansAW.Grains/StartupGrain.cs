using Orleans;
using OrleansAW.Grains.Interfaces;
using System.Threading.Tasks;

namespace OrleansAW.Grains
{
    public class StartupGrain : Grain, IStartupGrain
    {
        public async Task KeepAlive()
        {
            await Task.CompletedTask;
        }
    }
}

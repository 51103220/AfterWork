using ServiceStack;
using CoolWebService.ServiceModel;
using System.Threading.Tasks;
using System;

namespace CoolWebService.ServiceInterface
{
    public class MyServices : Service
    {
        //private static readonly int NUMBER_OF_THREADS = 200;
        //private static SemaphoreSlim S = new SemaphoreSlim(NUMBER_OF_THREADS, NUMBER_OF_THREADS);

        public async Task<object> Post(Hello request)
        {

            await Task.Delay(TimeSpan.FromSeconds(15));

            return new HelloResponse { Result = $"Hello, {request.Name}!"};
        }
    }
}
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MotivateMe.Core.DIConfiguration;

[assembly: FunctionsStartup(typeof(MotivateMe.Push.Startup))]
namespace MotivateMe.Push
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = builder.GetContext().Configuration;
            builder.Services.ConfigureFormCore(configuration);
        }
    }
}

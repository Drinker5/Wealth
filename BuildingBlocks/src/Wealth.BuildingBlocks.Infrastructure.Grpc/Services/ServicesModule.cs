using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application.Services;
using Wealth.BuildingBlocks.InstrumentManagement;

namespace Wealth.BuildingBlocks.Infrastructure.Grpc.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInstrumentService, GrpcInstrumentService>();
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o =>
        {
            o.Address = new Uri("http://instrument");
        }).AddServiceDiscovery();
    }
}
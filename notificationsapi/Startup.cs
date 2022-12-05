using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NotificationsApi.Database;

[assembly: FunctionsStartup(typeof(NotificationsApi.Startup))]

namespace NotificationsApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<AspenContext>();
            builder.Services.AddAutoMapper(typeof(NotificationsApi.Startup).Assembly);
            builder.Services.AddSwaggerGen(m => {
                m.UseInlineDefinitionsForEnums();
            });
        }
    }
}
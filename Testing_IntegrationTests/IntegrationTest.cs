using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using wdpr_project.Data;

namespace Testing_IntegrationTests;

[Trait("Category", "Integration")]
public abstract class IntegrationTest: IClassFixture<ApiWebApplicationFactory>
{
    private readonly RespawnerOptions _respawnerOptions = new RespawnerOptions {
        WithReseed = true
    };

    protected readonly ApiWebApplicationFactory _factory;
    protected readonly HttpClient _client;
    protected readonly ApplicationDbContext _context;
    protected readonly IMapper _mapper;

    public IntegrationTest(ApiWebApplicationFactory fixture)
    {
        _factory = fixture;
        _client = _factory.CreateClient();
        _context = _factory.Services.GetService<IServiceScopeFactory>()!.CreateScope().ServiceProvider
            .GetService<ApplicationDbContext>()!;
        _mapper = _factory.Services.GetService<IServiceScopeFactory>()!.CreateScope().ServiceProvider
            .GetService<IMapper>()!;
        
        string connString = ApiWebApplicationFactory.GetConnectionString();

        Respawner checkpoint = Respawner.CreateAsync(connString, _respawnerOptions).Result;

        checkpoint.ResetAsync(connString).Wait();
        
    }

}
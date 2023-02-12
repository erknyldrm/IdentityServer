using IdentityServer.AuthServer;
using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Seed;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<CustomDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"));
});

var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddIdentityServer()
    .AddConfigurationStore(config =>
    {
        config.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"),
            sqlOpt => sqlOpt.MigrationsAssembly(assemblyName));
    })
    .AddOperationalStore(config =>
    {
        config.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"),
            sqlOpt => sqlOpt.MigrationsAssembly(assemblyName));
    })
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddTestUsers(Config.GetTestUsers())
    .AddDeveloperSigningCredential();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var context = services.GetRequiredService<ConfigurationDbContext>();

    IdentityServerSeedData.Seed(context);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();


app.Run();

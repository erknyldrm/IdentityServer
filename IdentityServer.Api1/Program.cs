using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
    {
        opt.Authority = "https://localhost:7059";
        opt.Audience = "resource_api1";
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("ReadProduct", conf =>
    {
        conf.RequireClaim("scope", "api1.read");
    });

    opt.AddPolicy("CreateOrUpdate", conf =>
    {
        conf.RequireClaim("scope", new[] { "api1.create", "api1.update" });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

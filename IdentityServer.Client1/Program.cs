var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = "Cookies";
        opt.DefaultChallengeScheme = "oidc";
    }
).AddCookie("Cookies")
 .AddOpenIdConnect("oidc", opt =>
{
    opt.SignInScheme = "Cookies";
    opt.Authority = "https://localhost:7059";
    opt.ClientId = "Client1-Mvc";
    opt.ClientSecret = "secret";
    opt.ResponseType = "code id_token";
    opt.GetClaimsFromUserInfoEndpoint = true;
    opt.SaveTokens = true;
    opt.Scope.Add("api1.read");
    opt.Scope.Add("offline_access");
});


var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

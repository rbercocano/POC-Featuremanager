using FeaturesManagerPoC;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Azure.AppConfiguration.AspNetCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;


var config = new ConfigurationBuilder().Build();
var builder = WebApplication.CreateBuilder(args);
builder.Host
    .ConfigureAppConfiguration(config => config
        .AddAzureAppConfiguration(options => options
            .Connect("Endpoint=https://poc-core.azconfig.io;Id=Lz6A-l6-s0:mMC85TF+9U8kZ4pgmtPE;Secret=D00IZT2F/4xds1SBXrRBkcwigE/QuXdsbslCCFLccA0=")
            .ConfigureRefresh(c =>
            {
                c.Register("cqrs", true).SetCacheExpiration(TimeSpan.FromSeconds(2));
            })
            .UseFeatureFlags()));
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ITargetingContextAccessor, TestTargetingContextAccessor>();
builder.Services.AddFeatureManagement().AddFeatureFilter<TargetingFilter>();
builder.Services.AddAzureAppConfiguration();
// Add services to the container.
builder.Services.AddRazorPages();
var app = builder.Build();

app.UseAzureAppConfiguration();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

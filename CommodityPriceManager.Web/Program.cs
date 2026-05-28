using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CommodityPriceManager.Web;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Setup HttpClient configured for the ApiService
builder.Services.AddHttpClient("api", client =>
{
    // Prefer an explicit API URL when provided, then fall back to Aspire-injected values.
    var endpoint = builder.Configuration["ApiBaseUrl"]
                   ?? builder.Configuration["services:apiservice:https:0"] 
                   ?? builder.Configuration["services:apiservice:http:0"]
                   ?? builder.Configuration["services:apiservice:0"]
                   ?? "https://localhost:7160"; // User mentioned 7160
    
    client.BaseAddress = new Uri(endpoint);
});


// For default injection
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

builder.Services.AddMudServices();

await builder.Build().RunAsync();


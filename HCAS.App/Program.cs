using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HCAS.App;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<HCAS.App.Services.IndexedDbService>();
builder.Services.AddScoped<HCAS.App.Services.DoctorService>();
builder.Services.AddScoped<HCAS.App.Services.PatientService>();
builder.Services.AddScoped<HCAS.App.Services.SpecializationService>();
builder.Services.AddScoped<HCAS.App.Services.AppointmentService>();

var host = builder.Build();
var indexedDbService = host.Services.GetRequiredService<HCAS.App.Services.IndexedDbService>();
await indexedDbService.InitializeAsync();

await host.RunAsync();

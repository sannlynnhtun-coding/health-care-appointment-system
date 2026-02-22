using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HCAS.WasmApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<HCAS.WasmApp.Services.IndexedDbService>();
builder.Services.AddScoped<HCAS.WasmApp.Services.DoctorService>();
builder.Services.AddScoped<HCAS.WasmApp.Services.PatientService>();
builder.Services.AddScoped<HCAS.WasmApp.Services.SpecializationService>();
builder.Services.AddScoped<HCAS.WasmApp.Services.AppointmentService>();

var host = builder.Build();
var indexedDbService = host.Services.GetRequiredService<HCAS.WasmApp.Services.IndexedDbService>();
await indexedDbService.InitializeAsync();

await host.RunAsync();

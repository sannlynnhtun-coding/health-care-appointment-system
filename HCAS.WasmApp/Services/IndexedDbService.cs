using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCAS.WasmApp.Services;

public class IndexedDbService
{
    private readonly IJSRuntime _jsRuntime;
    private const string DbName = "HCAS_DB";
    private const int DbVersion = 1;

    public IndexedDbService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        var stores = new[]
        {
            new { name = "Doctors", keyPath = "id", autoIncrement = true },
            new { name = "Patients", keyPath = "id", autoIncrement = true },
            new { name = "Appointments", keyPath = "id", autoIncrement = true },
            new { name = "Specializations", keyPath = "id", autoIncrement = true },
            new { name = "Staff", keyPath = "id", autoIncrement = true },
            new { name = "DoctorSchedules", keyPath = "id", autoIncrement = true }
        };

        await _jsRuntime.InvokeVoidAsync("indexedDbInterop.initDb", DbName, DbVersion, stores);

        // Seed Specializations if empty
        var specs = await GetAllAsync<HCAS.WasmApp.Models.Specializations.SpecializationResModel>("Specializations");
        if (!specs.Any())
        {
            await AddOrUpdateAsync("Specializations", new { id = 1, name = "Cardiology" });
            await AddOrUpdateAsync("Specializations", new { id = 2, name = "Neurology" });
            await AddOrUpdateAsync("Specializations", new { id = 3, name = "Pediatrics" });
            await AddOrUpdateAsync("Specializations", new { id = 4, name = "Dermatology" });
        }
    }

    public async Task<List<T>> GetAllAsync<T>(string storeName)
    {
        return await _jsRuntime.InvokeAsync<List<T>>("indexedDbInterop.getAll", DbName, storeName);
    }

    public async Task<T> GetByIdAsync<T>(string storeName, object id)
    {
        return await _jsRuntime.InvokeAsync<T>("indexedDbInterop.getById", DbName, storeName, id);
    }

    public async Task AddOrUpdateAsync<T>(string storeName, T item)
    {
        await _jsRuntime.InvokeVoidAsync("indexedDbInterop.addOrUpdate", DbName, storeName, item);
    }

    public async Task DeleteAsync(string storeName, object id)
    {
        await _jsRuntime.InvokeVoidAsync("indexedDbInterop.deleteItem", DbName, storeName, id);
    }
}

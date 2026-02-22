using HCAS.App.Models.Specializations;
using HCAS.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAS.App.Services;

public class SpecializationService
{
    private readonly IndexedDbService _dbService;
    private const string StoreName = "Specializations";

    public SpecializationService(IndexedDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<List<SpecializationResModel>> GetSpecializationsAsync()
    {
        var all = await _dbService.GetAllAsync<SpecializationResModel>(StoreName);
        return all.OrderBy(s => s.Name).ToList();
    }

    public async Task<Result<SpecializationResModel>> CreateSpecializationAsync(SpecializationReqModel dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<SpecializationResModel>.ValidationError("Name is required");

        try
        {
            var all = await _dbService.GetAllAsync<SpecializationResModel>(StoreName);
            int nextId = all.Any() ? all.Max(s => s.Id) + 1 : 1;

            var specialization = new SpecializationResModel
            {
                Id = nextId,
                Name = dto.Name
            };

            await _dbService.AddOrUpdateAsync(StoreName, specialization);
            return Result<SpecializationResModel>.Success(specialization, "Specialization created successfully");
        }
        catch (System.Exception ex)
        {
            return Result<SpecializationResModel>.SystemError(ex.Message);
        }
    }
}

using HCAS.App.Models.Doctors;
using HCAS.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAS.App.Services;

public class DoctorService
{
    private readonly IndexedDbService _dbService;
    private const string StoreName = "Doctors";

    public DoctorService(IndexedDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<Result<PagedResult<DoctorsResModel>>> GetDoctorsAsync(
        int page = 1, int pageSize = 10, string? search = null, int? specializationId = null)
    {
        try
        {
            var allDoctors = await _dbService.GetAllAsync<DoctorsResModel>(StoreName);
            var query = allDoctors.Where(d => !d.DelFlg);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d => d.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase));

            if (specializationId.HasValue)
                query = query.Where(d => d.SpecializationId == specializationId);

            var total = query.Count();
            var items = query
                .OrderByDescending(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedResult = new PagedResult<DoctorsResModel>
            {
                Items = items,
                TotalCount = total
            };

            return items.Any()
                ? Result<PagedResult<DoctorsResModel>>.Success(pagedResult, $"Success. Total: {total}")
                : Result<PagedResult<DoctorsResModel>>.NotFound("No doctors found");
        }
        catch (System.Exception ex)
        {
            return Result<PagedResult<DoctorsResModel>>.SystemError(ex.Message);
        }
    }

    public async Task<Result<DoctorsResModel>> RegisterDoctorAsync(DoctorsReqModel dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<DoctorsResModel>.ValidationError("Name is required");

        try
        {
            var allDoctors = await _dbService.GetAllAsync<DoctorsResModel>(StoreName);
            int nextId = allDoctors.Any() ? allDoctors.Max(d => d.Id) + 1 : 1;

            var doctor = new DoctorsResModel
            {
                Id = nextId,
                Name = dto.Name,
                SpecializationId = dto.SpecializationId,
                DelFlg = false
            };

            await _dbService.AddOrUpdateAsync(StoreName, doctor);
            return Result<DoctorsResModel>.Success(doctor, "Doctor registered successfully");
        }
        catch (System.Exception ex)
        {
            return Result<DoctorsResModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<DoctorsResModel>> UpdateDoctorAsync(int id, DoctorsReqModel dto)
    {
        try
        {
            var doctor = await _dbService.GetByIdAsync<DoctorsResModel>(StoreName, id);
            if (doctor is null || doctor.DelFlg)
                return Result<DoctorsResModel>.NotFound("Doctor not found");

            doctor.Name = dto.Name;
            doctor.SpecializationId = dto.SpecializationId;

            await _dbService.AddOrUpdateAsync(StoreName, doctor);
            return Result<DoctorsResModel>.Success(doctor, "Doctor updated successfully");
        }
        catch (System.Exception ex)
        {
            return Result<DoctorsResModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteDoctorAsync(int id)
    {
        try
        {
            var doctor = await _dbService.GetByIdAsync<DoctorsResModel>(StoreName, id);
            if (doctor is null || doctor.DelFlg)
                return Result<bool>.NotFound("Doctor not found");

            doctor.DelFlg = true;
            await _dbService.AddOrUpdateAsync(StoreName, doctor);
            return Result<bool>.DeleteSuccess("Doctor deleted successfully");
        }
        catch (System.Exception ex)
        {
            return Result<bool>.SystemError(ex.Message);
        }
    }
}

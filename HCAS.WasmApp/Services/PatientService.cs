using HCAS.WasmApp.Models.Patients;
using HCAS.WasmApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAS.WasmApp.Services;

public class PatientService
{
    private readonly IndexedDbService _dbService;
    private const string StoreName = "Patients";

    public PatientService(IndexedDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<Result<PagedResult<PatientResModel>>> GetPatientsAsync(
        int page = 1, int pageSize = 10, string? search = null)
    {
        try
        {
            var allPatients = await _dbService.GetAllAsync<PatientResModel>(StoreName);
            var query = allPatients.Where(p => !p.DelFlg);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase) || 
                                         p.Phone.Contains(search) || 
                                         p.Email.Contains(search, System.StringComparison.OrdinalIgnoreCase));

            var total = query.Count();
            var items = query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedResult = new PagedResult<PatientResModel>
            {
                Items = items,
                TotalCount = total
            };

            return items.Any()
                ? Result<PagedResult<PatientResModel>>.Success(pagedResult, $"Success. Total: {total}")
                : Result<PagedResult<PatientResModel>>.NotFound("No patients found");
        }
        catch (System.Exception ex)
        {
            return Result<PagedResult<PatientResModel>>.SystemError(ex.Message);
        }
    }

    public async Task<Result<PatientResModel>> RegisterPatientAsync(PatientReqModel dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<PatientResModel>.ValidationError("Name is required");

        try
        {
            var allPatients = await _dbService.GetAllAsync<PatientResModel>(StoreName);
            int nextId = allPatients.Any() ? allPatients.Max(p => p.Id) + 1 : 1;

            var patient = new PatientResModel
            {
                Id = nextId,
                Name = dto.Name,
                DateOfBirth = dto.DateOfBirth ?? System.DateTime.Now,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Email = dto.Email,
                DelFlg = false
            };

            await _dbService.AddOrUpdateAsync(StoreName, patient);
            return Result<PatientResModel>.Success(patient, "Patient registered successfully");
        }
        catch (System.Exception ex)
        {
            return Result<PatientResModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<PatientResModel>> UpdatePatientAsync(int id, PatientReqModel dto)
    {
        try
        {
            var patient = await _dbService.GetByIdAsync<PatientResModel>(StoreName, id);
            if (patient is null || patient.DelFlg)
                return Result<PatientResModel>.NotFound("Patient not found");

            patient.Name = dto.Name;
            patient.DateOfBirth = dto.DateOfBirth ?? patient.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.Phone = dto.Phone;
            patient.Email = dto.Email;

            await _dbService.AddOrUpdateAsync(StoreName, patient);
            return Result<PatientResModel>.Success(patient, "Patient updated successfully");
        }
        catch (System.Exception ex)
        {
            return Result<PatientResModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<bool>> DeletePatientAsync(int id)
    {
        try
        {
            var patient = await _dbService.GetByIdAsync<PatientResModel>(StoreName, id);
            if (patient is null || patient.DelFlg)
                return Result<bool>.NotFound("Patient not found");

            patient.DelFlg = true;
            await _dbService.AddOrUpdateAsync(StoreName, patient);
            return Result<bool>.DeleteSuccess("Patient deleted successfully");
        }
        catch (System.Exception ex)
        {
            return Result<bool>.SystemError(ex.Message);
        }
    }
}

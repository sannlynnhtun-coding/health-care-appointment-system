namespace HCAS.WasmApp.Models.Specializations;

public class SpecializationReqModel
{
    public string Name { get; set; } = string.Empty;
}

public class SpecializationResModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

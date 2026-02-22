namespace HCAS.WasmApp.Models.Patients;

public class PatientReqModel
{
    public string Name { get; set; } = string.Empty;
    public System.DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class PatientResModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public System.DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool DelFlg { get; set; }
}

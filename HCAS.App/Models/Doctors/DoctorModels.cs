namespace HCAS.App.Models.Doctors;

public class DoctorsReqModel
{
    public string Name { get; set; } = string.Empty;
    public int SpecializationId { get; set; }
}

public class DoctorsResModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SpecializationId { get; set; }
    public bool DelFlg { get; set; }
}

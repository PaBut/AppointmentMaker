using AppointmentMaker.Domain.Entities.Common;

namespace AppointmentMaker.Domain.Entities;

public class FileModel : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Extention { get; set; } = string.Empty;
    public byte[]? Content { get; set; }
    public string DoctorId { get; set; }
}

namespace AppointmentMaker.Application.Features.FileModel.Queries.Shared;

public record FileModelDto
{
    public string Extension { get; set; } = string.Empty;
    public byte[] Content { get; set; }
}

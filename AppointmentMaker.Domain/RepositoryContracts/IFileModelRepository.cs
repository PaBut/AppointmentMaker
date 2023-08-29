using AppointmentMaker.Domain.Entities;

namespace AppointmentMaker.Domain.RepositoryContracts;

public interface IFileModelRepository : IGenericRepository<FileModel>
{
    Task<FileModel?> GetByDoctorIdAsync(string doctorId);
}

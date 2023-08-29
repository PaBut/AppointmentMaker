using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.FileModel.Commands.RegisterCreate;

public class FileModelRegisterCreateCommandHandler : IResultRequestHandler<FileModelRegisterCreateCommand, Guid>
{
    private readonly IFileModelRepository _fileModelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDoctorService _doctorService;

    public FileModelRegisterCreateCommandHandler(IUnitOfWork unitOfWork,
        IFileModelRepository fileModelRepository,
        IDoctorService doctorService)
    {
        _unitOfWork = unitOfWork;
        _fileModelRepository = fileModelRepository;
        _doctorService = doctorService;
    }

    public async Task<Result<Guid>> Handle(FileModelRegisterCreateCommand command, CancellationToken cancellationToken)
    {
        var formFile = command.FormFile;
        using var stream = new MemoryStream();
        formFile.CopyTo(stream);
        byte[] data = stream.ToArray();

        var fileModel = new Domain.Entities.FileModel
        {
            Name = formFile.FileName,
            Extention = formFile.ContentType,
            Content = data
        };

        await _fileModelRepository.CreateAsync(fileModel);
        await _unitOfWork.SaveChangesAsync();

        return fileModel.Id;
    }
}

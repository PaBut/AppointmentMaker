using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.FileModel.Commands.Create;

public class FileModelCreateCommandHandler : IResultRequestHandler<FileModelCreateCommand, Guid>
{
    private readonly IFileModelRepository _fileModelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDoctorService _doctorService;

    public FileModelCreateCommandHandler(IUnitOfWork unitOfWork,
        IFileModelRepository fileModelRepository,
        IDoctorService doctorService)
    {
        _unitOfWork = unitOfWork;
        _fileModelRepository = fileModelRepository;
        _doctorService = doctorService;
    }

    public async Task<Result<Guid>> Handle(FileModelCreateCommand command, CancellationToken cancellationToken)
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

        await _unitOfWork.BeginTransaction();

        await _fileModelRepository.CreateAsync(fileModel);
        await _unitOfWork.SaveChangesAsync();

        var assignResult = await _doctorService.AssignProfilePicture(command.doctorId, fileModel.Id);

        if (assignResult.IsFailure)
        {
            await _unitOfWork.RollBackTransaction();
            return Result.Failure<Guid>(assignResult.Error);
        }

        await _unitOfWork.CommitTransaction();

        return fileModel.Id;
    }
}

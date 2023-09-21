using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.FileModel.Commands.Update;

public class FileModelUpdateCommandHandler : IResultRequestHandler<FileModelUpdateCommand>
{
    private readonly IFileModelRepository _fileModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FileModelUpdateCommandHandler(IUnitOfWork unitOfWork,
        IFileModelRepository fileModelRepository)
    {
        _unitOfWork = unitOfWork;
        _fileModelRepository = fileModelRepository;
    }

    public async Task<Result> Handle(FileModelUpdateCommand command, CancellationToken cancellationToken)
    {
        var fileModel = await _fileModelRepository.GetByIdAsync(command.Id);

        if(fileModel == null)
        {
            return Result.Failure(Error.NotFound("File"));
        }

        var formFile = command.FormFile;
        using var stream = new MemoryStream();
        await formFile.CopyToAsync(stream, cancellationToken);
        byte[] data = stream.ToArray();

        fileModel.Name = formFile.FileName;
        fileModel.Extension = formFile.ContentType;
        fileModel.Content = data;

        await _fileModelRepository.UpdateAsync(fileModel);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}

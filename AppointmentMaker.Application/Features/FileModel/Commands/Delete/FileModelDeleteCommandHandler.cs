using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.FileModel.Commands.Delete;

public class FileModelDeleteCommandHandler : IResultRequestHandler<FileModelDeleteCommand>
{
    private readonly IFileModelRepository _fileModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FileModelDeleteCommandHandler(IUnitOfWork unitOfWork,
        IFileModelRepository fileModelRepository)
    {
        _unitOfWork = unitOfWork;
        _fileModelRepository = fileModelRepository;
    }

    public async Task<Result> Handle(FileModelDeleteCommand command, CancellationToken cancellationToken)
    {
        var fileModel = await _fileModelRepository.GetByIdAsync(command.Id);

        if(fileModel == null)
        {
            return Result.Failure(Error.NotFound("File"));
        }

        await _fileModelRepository.DeleteAsync(fileModel);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}

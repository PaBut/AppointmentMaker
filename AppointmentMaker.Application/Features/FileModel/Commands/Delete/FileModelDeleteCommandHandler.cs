using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

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
            return Result.Failure(new Error("FileModel.Delete", "File with specified id not found"));
        }

        await _fileModelRepository.DeleteAsync(fileModel);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}

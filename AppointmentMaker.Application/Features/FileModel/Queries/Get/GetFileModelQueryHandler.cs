using AppointmentMaker.Application.Features.FileModel.Queries.Shared;
using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;

namespace AppointmentMaker.Application.Features.FileModel.Queries.Get;

public class GetFileModelQueryHandler
    : IResultRequestHandler<GetFileModelQuery, FileModelDto>
{
    private readonly IFileModelRepository _fileModelRepository;
    private readonly IMapper _mapper;

    public GetFileModelQueryHandler(IFileModelRepository fileModelRepository, 
        IMapper mapper)
    {
        _fileModelRepository = fileModelRepository;
        _mapper = mapper;
    }

    public async Task<Result<FileModelDto>> Handle(GetFileModelQuery command, CancellationToken cancellationToken)
    {
        var fileModel = await _fileModelRepository.GetByIdAsync(command.Id);

        if(fileModel == null)
        {
            return Result.Failure<FileModelDto>(Error.NotFound("File"));
        }

        var fileModelDto = _mapper.Map<FileModelDto>(fileModel);

        return fileModelDto;
    }
}

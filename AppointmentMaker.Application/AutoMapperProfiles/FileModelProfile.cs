using AppointmentMaker.Application.Features.FileModel.Queries.Shared;
using AppointmentMaker.Domain.Entities;
using AutoMapper;

namespace AppointmentMaker.Application.AutoMapperProfiles;

public class FileModelProfile : Profile
{
    public FileModelProfile()
    {
        CreateMap<FileModel, FileModelDto>();
    }
}

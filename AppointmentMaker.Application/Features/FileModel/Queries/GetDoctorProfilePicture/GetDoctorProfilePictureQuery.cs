using AppointmentMaker.Application.Features.FileModel.Queries.Shared;
using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.FileModel.Queries.GetDoctorProfilePicture;

public record GetDoctorProfilePictureQuery(string doctorId) : IResultRequest<FileModelDto>;


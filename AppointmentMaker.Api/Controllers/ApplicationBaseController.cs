using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ApplicationBaseController : ControllerBase
{
}

namespace AppointmentMaker.Api.Swagger.Models;

public class SwaggerApplicationSettings
{
    public string Title { get; set; }
    public List<SwaggerVersionDescription> Descriptions { get; set; } = new();
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
}

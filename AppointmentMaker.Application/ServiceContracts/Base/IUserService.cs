namespace AppointmentMaker.Application.ServiceContracts.Base;

public interface IUserService
{
    Task<bool> IsEmailAvailable(string email);
    Task<string?> GetUserId();
    Task<bool> UserExists(string Id);
}

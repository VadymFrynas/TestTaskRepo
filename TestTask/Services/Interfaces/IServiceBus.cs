using TestTask.Models;

namespace TestTask.Services.Interfaces
{
    public interface IServiceBus
    {
        Task SendMessageAsync(int userId);
    }
}
